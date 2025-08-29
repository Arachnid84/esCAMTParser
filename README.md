# Extreme Solutions CAMT053 Parser

Developed to provide a simple, easy-to-use CAMT053 parser that works across multiple banks in the SEPA zone.  
The package has been compiled and tested for use within **Microsoft Visual Studio 2022**, and also in near production-ready internal applications for CRM, Accounting, and Sales pipelines.

Currently supports parsing of CAMT053.001.002 and CAMT053.001.008 formats with CAMT053.001.002 as base class;

**Source code**: [GitHub](https://github.com/Arachnid84/esCAMTParser) | **NuGet**: [esCAMTParser](https://www.nuget.org/packages/esCAMTParser/)

---
## Getting Started

### Install the package
Install esCAMTParser via NuGet: `dotnet add package esCAMTParser`

## How to use
To parse an CAMT file you can use the following basic example

    using esCAMTParser;
    
    public function ImportCAMT(string camtFile)
    {
        var statement = esCAMTParser.Parser.Parse(camtFile);
    
        foreach (var entry in statement.Result)
        {
            //do mapping of fields
            foreach (var transaction in entry.Transactions)
            {
                //do mapping of individual transactions contained in a statement
            }
        }
    }

`Parser.Parse()` supports either a full file input (e.g. file path) or a stream by preceding with `var stream = camtFile.OpenReadStream();`

## Class exposure
The CAMT parser casts the content of a CAMT file into a single object containing one or more transactions as subclass with the related details on a statement:

### Statement (Stmt tag)
The statement class returns the following values:

- String: StatementId: Tag Id - Id of the processed statement.
- String: SequenceNumber: Tag ElctrncSeqNb or SeqNb - Sequence number for statement. Returns empty if not provided
- String: Reference: Generated statement reference comprised of either StatementId or StatementId-SequenceNumber if SequenceNumber is provided
- DateTime: CreationDateTime: Tag CreDtTm - Statement file creation date and time
- DateTime: FromDate: Tag FrToDt/FrDtTm - The date from which the statement is valid
- DateTime: EndDate: Tag FrToDt/ToDtTm - The date until which the statement is valid
- String: AccountIBAN: Tag Acct/Id/IBAN - The IBAN of the account for which the statement is provided
- String: Currency: Tag Acct/@Ccy - Returns the statements primary currency if provided
- Decimal: OpeningBalance: Tag Bal/Amt (CD=OPBD) - The openening balance of the statement
- Decimal: ClosingBalance: Tag Bal/Amt (CD=CLBD) - The closing balance of the statement
- String: AdditionalInformation: Tag AddtlInf - Additional information on the statement. Returns empty if not proviced
- ICollection Transaction: Transactions: Tag Ntry - Collection of transactions contained in the current statement

###Transaction
The transactions contain the following values as collection within the statement:

- DateTime: BookingDate: Tag BookgDt/Dt - The date on which the transaction was booked
- DateTime: ValueDate: Tag ValDt/Dt - The date on which the transaction was valued
- String: CreditDebitIndicator: Tag CdtDbtInd - The credit or debit indicator of the transaction
- Decimal: Amount: Tag Amt - The amount of the transaction
- String: Currency: Tag Amt/@Ccy - The currency of the transaction
- String: Status: Tag TxDtls/Sts - The status of the transaction (e.g. ACTC, PDNG, RJCT, etc.)
- ICollection: Parties: Tag RltdPties - List of related parties to the transaction
- ICollection: RelatedDates: Tag RltdDates - List of related dates to the transaction
- ICollection: Remittance: Tag RmtInf - List of remittance information related to the transaction
- ICollection: Reversal: Tag RtrInfo - List of reversal information related to the transaction
- ICollection: References: Tag Refs - List of references related to the transaction
- ICollection: Charges: Tag Chrgs - List of transaction charges related to the transaction
- ICollection: Attributes: Tag multiple - List of additional attributes related to the transaction