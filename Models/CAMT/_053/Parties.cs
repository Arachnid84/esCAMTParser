using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace esCAMTParser.Models.CAMT._053
{
    public class Parties
    {
        public string? DebtorName { get; set; }
        public string? CreditorName { get; set; }
        public string? UltimateDebtor { get; set; }
        public string? UltimateCreditor { get; set; }
        public string? InitiatingParty { get; set; }
    }
}
