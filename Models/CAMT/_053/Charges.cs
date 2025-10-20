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
