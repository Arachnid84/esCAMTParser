using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace esCAMTParser.Models.CAMT._053
{
    /// <summary>
    /// CAMT v2+: Unstructured remittance information (Unstrd)
    /// CAMT v8+: Structured remittance fields (CdtrRefInf, RfrdDocInf, AddtlRmtInf)
    /// </summary>
    public class Remittance
    {
        public string? Unstructured { get; set; }
        /// <summary>
        /// CAMT v8+: Structured remittance fields (CdtrRefInf, RfrdDocInf, AddtlRmtInf)
        /// </summary>
        public string? CreditorReference { get; set; }
        /// <summary>
        /// CAMT v8+: Structured remittance fields (CdtrRefInf, RfrdDocInf, AddtlRmtInf)
        /// </summary>
        public string? InvoiceReference { get; set; }
        /// <summary>
        /// CAMT v8+: Structured remittance fields (CdtrRefInf, RfrdDocInf, AddtlRmtInf)
        /// </summary>
        public decimal? ReferencedDocumentAmount { get; set; }
    }
}
