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
