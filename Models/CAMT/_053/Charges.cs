using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace esCAMTParser.Models.CAMT._053
{
    /// <summary>
    /// CAMT v8+: Represents charges associated with a transaction.
    /// Populated from the &lt;Chrgs&gt; element. Includes amount, currency, and bearer.
    /// </summary>
    public class Charges
    {
        /// <summary>
        /// Amount of the charge.
        /// CAMT v8: From &lt;Amt&gt; inside &lt;Chrgs&gt;
        /// </summary>
        public decimal? Amount { get; set; }
        /// <summary>
        /// Currency in which the charge is expressed.
        /// CAMT v8: Attribute of &lt;Amt&gt;
        /// </summary>
        public string? Currency { get; set; }
        /// <summary>
        /// Party bearing the charge cost.
        /// CAMT v8: &lt;Chrgs&gt;/&lt;ChrgsBr&gt;
        /// </summary>
        public string? Bearer { get; set; }
        /// <summary>
        /// Optional identifier or reference for the charge entry.
        /// Not always populated.
        /// </summary>
        public string? ChargeId { get; set; }
    }
}
