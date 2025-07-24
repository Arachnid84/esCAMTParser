using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace esCAMTParser.Models.CAMT._053
{
    public class Reversal
    {
        public decimal? Amount { get; set; }
        public string? ReasonCode { get; set; }
        public string? ReasonText { get; set; }
    }
}
