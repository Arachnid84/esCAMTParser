/*
 * MIT License
 * 
 * Copyright (c) 2025 Extreme Solutions
 * 
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in all
 * copies or substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
 * SOFTWARE.
 */

using esCAMTParser.Models.CAMT._053;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using esCAMTParser.Classes;

namespace esCAMTParser.Parsers.CAMT._053.V2
{
    public class CAMT053ParserV2 : IStatementParser
    {
        public List<Statement> Parse(Stream xmlStream)
        {
            xmlStream.Position = 0;

            var document = XDocument.Load(xmlStream);
            var root = document.Root;

            var stmtElem = root?.Descendants().Where(e => e.Name.LocalName == "Stmt");
            if (stmtElem == null)
                throw new InvalidDataException("No <Stmt> element found in CAMT file.");

            var statement = new List<Statement>();

            foreach (var stmt in stmtElem)
            {
                statement.Add(ParseStatement(stmt));
            }

            return statement;
        }

        internal Statement ParseStatement(XElement stmt)
        {
            var balances = stmt.ElementsAnyNs("Bal");

            var openingBalElem = balances.FirstOrDefault(b =>
                b.ElementAnyNs("Tp")?
                 .ElementAnyNs("CdOrPrtry")?
                 .ElementAnyNs("Cd")?.Value == "OPBD");

            var closingBalElem = balances.FirstOrDefault(b =>
                b.ElementAnyNs("Tp")?
                 .ElementAnyNs("CdOrPrtry")?
                 .ElementAnyNs("Cd")?.Value == "CLBD");

            var openingBalance = DecimalParser.ParseInvariant(openingBalElem?.ElementAnyNs("Amt")?.Value) ?? 0;
            var closingBalance = DecimalParser.ParseInvariant(closingBalElem?.ElementAnyNs("Amt")?.Value) ?? 0;
            DateTime? fromDate = DateTime.TryParse(openingBalElem?.ElementAnyNs("Dt")?.ElementAnyNs("Dt")?.Value, out var fdt) ? fdt : null;
            DateTime? toDate = DateTime.TryParse(closingBalElem?.ElementAnyNs("Dt")?.ElementAnyNs("Dt")?.Value, out var tdt) ? tdt : null;

            string stmtSequenceNumber = (stmt.ElementAnyNs("ElctrncSeqNb")?.Value ?? stmt.ElementAnyNs("SeqNb")?.Value) ?? string.Empty;

            var statement = new Statement
            {
                StatementId = stmt.ElementAnyNs("Id")?.Value ?? throw new InvalidOperationException("Statement ID is required."),
                SequenceNumber = stmtSequenceNumber,
                Reference = !string.IsNullOrWhiteSpace(stmtSequenceNumber) ? stmt.ElementAnyNs("Id")?.Value + '-' + stmtSequenceNumber : stmt.ElementAnyNs("Id")?.Value,
                CreationDateTime = DateTime.Parse(stmt.ElementAnyNs("CreDtTm")?.Value ?? throw new InvalidOperationException("Creation date and time is required.")),
                FromDate = stmt.ElementAnyNs("FrToDt")?.ElementAnyNs("FrDt")?.Value is string fromDateValue ? DateTime.Parse(fromDateValue) : fromDate,
                EndDate = stmt.ElementAnyNs("FrToDt")?.ElementAnyNs("ToDt")?.Value is string endDateValue ? DateTime.Parse(endDateValue) : fromDate,
                AccountIBAN = stmt.ElementAnyNs("Acct")?.ElementAnyNs("Id")?.ElementAnyNs("IBAN")?.Value ?? throw new InvalidOperationException("Account IBAN is required."),
                Currency = stmt.ElementAnyNs("Acct")?.ElementAnyNs("Ccy")?.Value ?? throw new InvalidOperationException("Currency is required."),
                OpeningBalance = openingBalance,
                ClosingBalance = closingBalance,
                AdditionalInformation = stmt.ElementAnyNs("AddtlInf")?.Value
            };

            foreach (var ntry in stmt.ElementsAnyNs("Ntry"))
            {
                var tx = ParseTransaction(ntry);
                statement.Transactions.Add(tx);
            }

            return statement;
        }

        internal Transaction ParseTransaction(XElement ntry)
        {
            var transaction = new Transaction
            {
                BookingDate = DateTime.Parse(ntry.ElementAnyNs("BookgDt")?.ElementAnyNs("Dt")?.Value ?? throw new InvalidOperationException("Booking date is required.")),
                ValueDate = DateTime.Parse(ntry.ElementAnyNs("ValDt")?.ElementAnyNs("Dt")?.Value ?? throw new InvalidOperationException("Value date is required.")),
                CreditDebitIndicator = ntry.ElementAnyNs("CdtDbtInd")?.Value ?? throw new InvalidOperationException("Credit/Debit indicator is required."),
                Amount = DecimalParser.ParseInvariantOrThrow(ntry.ElementAnyNs("Amt")?.Value, "Amount is required."),
                Currency = ntry.ElementAnyNs("Amt")?.Attribute("Ccy")?.Value ?? throw new InvalidOperationException("Currency is required."),
            };

            var txDtl = ntry
                .ElementAnyNs("NtryDtls")
                ?.ElementAnyNs("TxDtls");

            if (txDtl != null)
            {
                transaction.References = ParseReferences(txDtl.ElementAnyNs("Refs"));
                transaction.Parties = ParseParties(txDtl.ElementAnyNs("RltdPties"), txDtl.ElementAnyNs("RltdAgts"));
                transaction.Remittance = new Remittance { Unstructured = txDtl.ElementAnyNs("RmtInf")?.ElementAnyNs("Ustrd")?.Value};
                transaction.Reversal = ParseReversal(txDtl.ElementAnyNs("RvsdAmt"));
                transaction.RelatedDates = ParseRelatedDates(txDtl.ElementAnyNs("RltdDts"));
                transaction.Attributes = ParseAttributes(txDtl);
            }

            return transaction;
        }

        internal References ParseReferences(XElement? refs)
        {
            return new References
            {
                EndtoEndId = refs?.ElementAnyNs("EndToEndId")?.Value,
                InstructionId = refs?.ElementAnyNs("InstrId")?.Value,
                PaymentInfoId = refs?.ElementAnyNs("PmtInfId")?.Value,
                TransactionId = refs?.ElementAnyNs("TxId")?.Value,
                AccountServicerId = refs?.ElementAnyNs("AcctSvcrRef")?.Value
            };
        }

        internal Parties ParseParties(XElement? rltdPties, XElement? rltdAgts)
        {
            return new Parties
            {
                DebtorName = rltdPties?.ElementAnyNs("Dbtr")?.ElementAnyNs("Nm")?.Value,
                CreditorName = rltdPties?.ElementAnyNs("Cdtr")?.ElementAnyNs("Nm")?.Value,
                UltimateDebtor = rltdPties?.ElementAnyNs("UltmtDbtr")?.ElementAnyNs("Nm")?.Value,
                UltimateCreditor = rltdPties?.ElementAnyNs("UltmtCdtr")?.ElementAnyNs("Nm")?.Value,
                InitiatingParty = rltdAgts?.ElementAnyNs("InitgPty")?.ElementAnyNs("Nm")?.Value
            };
        }

        internal RelatedDates ParseRelatedDates(XElement? rltdDts)
        {
            return new RelatedDates
            {
                RequestedExecutionDate = rltdDts.ElementAnyNs("ReqdExctnDt")?.Value is string executionDateValue ? DateOnly.Parse(executionDateValue) : null,
                InterbankSettlementDate = rltdDts.ElementAnyNs("ReqdExctnDt")?.Value is string settlementDateValue ? DateOnly.Parse(settlementDateValue) : null,
                AcceptanceDateTime = rltdDts.ElementAnyNs("AccptncDtTm")?.Value is string acceptanceDateValue ? DateTime.Parse(acceptanceDateValue) : null
            };
        }

        internal Reversal ParseReversal(XElement? rtrInf)
        {
            return new Reversal
            {
                Amount = DecimalParser.ParseInvariant(rtrInf?.ElementAnyNs("Amt")?.Value) ?? (decimal?)null,
                ReasonCode = rtrInf?.ElementAnyNs("Rsn")?.ElementAnyNs("Cd")?.Value,
                ReasonText = rtrInf?.ElementAnyNs("AddtlInf")?.Value
            };
        }

        internal Attributes ParseAttributes(XElement? txDtls)
        {
            return new Attributes
            {
                BankTransactionCode = txDtls?.ElementAnyNs("BkTxCd")?.ElementAnyNs("Cd")?.Value,
                PurposeCode = txDtls?.ElementAnyNs("Purp")?.ElementAnyNs("Cd")?.Value,
            };
        }
    }
}
