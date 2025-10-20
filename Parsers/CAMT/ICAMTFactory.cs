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

using esCAMTParser.Models.CAMT._053;
using esCAMTParser.Parsers.CAMT._053.V2;
using esCAMTParser.Parsers.CAMT._053.V8;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace esCAMTParser.Parsers.CAMT
{
    public interface ICAMTFactory
    {
        IStatementParser GetParser(Stream xmlStream);
    }

    public interface IStatementParser
    {
        List<Statement> Parse(Stream xmlStream);
    }
    internal class CAMTFactory : ICAMTFactory
    {
        public IStatementParser GetParser(Stream xmlStream)
        {
            xmlStream.Position = 0;

            var document = XDocument.Load(xmlStream);
            var root = document.Root;

            var schema = root?.GetDefaultNamespace()?.NamespaceName ?? string.Empty;

            var match = Regex.Match(schema, @"camt\.053\.001\.(\d+)$");
            if (!match.Success)
                throw new NotSupportedException($"Unsupported CAMT schema: {schema}");

            var version = int.Parse(match.Groups[1].Value);

            return version switch
            {
                2 => new CAMT053ParserV2(),
                8 => new CAMT053ParserV8(),
                _ => throw new NotSupportedException($"Unsupported CAMT.053 version: {version}")
            };
        }
    }
}
