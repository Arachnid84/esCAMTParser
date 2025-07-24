using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace esCAMTParser.Models.CAMT._053
{
    public class RelatedDates
    {
        public DateOnly? RequestedExecutionDate { get; set; }
        public DateOnly? InterbankSettlementDate { get; set; }
        public DateTime? AcceptanceDateTime { get; set; }
    }
}
