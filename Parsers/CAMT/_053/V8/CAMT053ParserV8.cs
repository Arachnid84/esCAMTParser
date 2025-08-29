using esCAMTParser.Classes;
using esCAMTParser.Models.CAMT._053;
using esCAMTParser.Parsers.CAMT._053.V2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace esCAMTParser.Parsers.CAMT._053.V8
{
    public class CAMT053ParserV8 : CAMT053ParserV2
    {
        internal new Transaction ParseTransaction(XElement ntry)
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
                transaction.Charges = ParseCharges(txDtl.ElementAnyNs("Chrgs"));
                transaction.References = ParseReferences(txDtl.ElementAnyNs("Refs"));
                transaction.Parties = ParseParties(txDtl.ElementAnyNs("RltdPties"), txDtl.ElementAnyNs("RltdAgts"));
                transaction.Remittance = ParseRemittance(txDtl.ElementAnyNs("RmtInf"));
                transaction.Reversal = ParseReversal(txDtl.ElementAnyNs("RvsdAmt"));
                transaction.RelatedDates = ParseRelatedDates(txDtl.ElementAnyNs("RltdDts"));
                transaction.Attributes = ParseAttributes(txDtl);
            }

            return transaction;
        }

        internal Remittance ParseRemittance(XElement? rmtInf)
        {
            return new Remittance
            {
                Unstructured = rmtInf?.ElementAnyNs("Ustrd")?.Value,
                CreditorReference = rmtInf?.ElementAnyNs("Strd")?.ElementAnyNs("CdtrRefInf")?.ElementAnyNs("Ref")?.Value,
                InvoiceReference = rmtInf?.ElementAnyNs("Strd")?.ElementAnyNs("RfrdDocInf")?.ElementAnyNs("Nb")?.Value,
                ReferencedDocumentAmount = DecimalParser.ParseInvariant(rmtInf?.ElementAnyNs("Strd")?.ElementAnyNs("RfrdDocAmt")?.ElementAnyNs("DuePyblAmt")?.Value)
            };
        }

        internal Charges ParseCharges(XElement? chrgs)
        {
            return new Charges
            {
                Amount = DecimalParser.ParseInvariant(chrgs?.ElementAnyNs("TtlChrgsAndTaxAmt")?.Value),
                Currency = chrgs?.ElementAnyNs("TtlChrgsAndTaxAmt")?.Attribute("Ccy")?.Value,
                Bearer = chrgs?.ElementAnyNs("ChrgsInf")?.ElementAnyNs("ChrgBr")?.Value,
                ChargeId = chrgs?.ElementAnyNs("ChrgsInf")?.ElementAnyNs("Id")?.Value
            };
        }

        internal new Parties ParseParties(XElement? rltdPties, XElement? rltdAgts)
        {
            return new Parties
            {
                DebtorName = rltdPties?.ElementAnyNs("Dbtr")?.ElementAnyNs("Pty")?.ElementAnyNs("Nm")?.Value,
                CreditorName = rltdPties?.ElementAnyNs("Cdtr")?.ElementAnyNs("Pty")?.ElementAnyNs("Nm")?.Value,
                UltimateDebtor = rltdPties?.ElementAnyNs("UltmtDbtr")?.ElementAnyNs("Pty")?.ElementAnyNs("Nm")?.Value,
                UltimateCreditor = rltdPties?.ElementAnyNs("UltmtCdtr")?.ElementAnyNs("Pty")?.ElementAnyNs("Nm")?.Value,
                InitiatingParty = rltdAgts?.ElementAnyNs("InitgPty")?.ElementAnyNs("Pty")?.ElementAnyNs("Nm")?.Value
            };
        }
    }
}
