using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace esCAMTParser.Models.CAMT._053
{
    public class Transaction
    {
        public required DateTime BookingDate { get; set; }
        public required DateTime ValueDate { get; set; }
        public required string CreditDebitIndicator { get; set; }
        public required decimal Amount { get; set; }
        public required string Currency { get; set; }
        public string? Status { get; set; }

        public References? References { get; set; }
        public Parties? Parties { get; set; }
        public RelatedDates? RelatedDates { get; set; }
        /// <summary>
        /// CAMT v8+: Structured remittance fields (CdtrRefInf, RfrdDocInf, AddtlRmtInf)
        /// </summary>
        public Remittance? Remittance { get; set; }
        public Reversal? Reversal { get; set; }      
        /// <summary>
        /// CAMT v8+: Transaction charges information (Chrgs)
        /// </summary>
        public Charges? Charges { get; set; }
        public Attributes? Attributes { get; set; }
    }
}
