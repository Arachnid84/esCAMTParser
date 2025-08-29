using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace esCAMTParser.Models.CAMT._053
{
    public class Statement
    {
        public required string StatementId { get; set; }
        public required DateTime CreationDateTime { get; set; }
        public string? SequenceNumber { get; set; }
        public string? Reference { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? EndDate { get; set; }
        public required string AccountIBAN { get; set; }
        public required string Currency { get; set; }
        public decimal? OpeningBalance { get; set; }
        public decimal? ClosingBalance { get; set; }
        public string? AdditionalInformation { get; set; }

        public List<Transaction> Transactions { get; set; } = new List<Transaction>();
    }
}
