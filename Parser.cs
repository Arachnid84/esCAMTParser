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
using System.Diagnostics;
using System.Net.Security;
using System.Text;
using System.Xml.Linq;

namespace esCAMTParser
{
    public class Parser
    {
        public static ICollection<Statement> Parse(string fileName)
        {
            string data;
            StreamReader streamReader;
            using (streamReader = new StreamReader(fileName))
            {
                data = streamReader.ReadToEnd();
            }
            var stream = new MemoryStream(Encoding.UTF8.GetBytes(data));

            var factory = new Parsers.CAMT.CAMTFactory();
            var parser = factory.GetParser(stream);

            return parser.Parse(stream);
        }

        public static ICollection<Statement> Parse(Stream xmlStream)
        {
            xmlStream = EnsureSeekable(xmlStream);

            var factory = new Parsers.CAMT.CAMTFactory();
            var parser = factory.GetParser(xmlStream);

            return parser.Parse(xmlStream);
        }

        public static bool Validate(string fileName)
        {
            string data;
            StreamReader streamReader;
            using (streamReader = new StreamReader(fileName))
            {
                data = streamReader.ReadToEnd();
            }
            var stream = new MemoryStream(Encoding.UTF8.GetBytes(data));

            return Validate(stream);
        }

        public static bool Validate(Stream xmlStream)
        {
            xmlStream = EnsureSeekable(xmlStream);
            xmlStream.Seek(0, SeekOrigin.Begin);

            var document = XDocument.Load(xmlStream);
            var root = document.Root;

            var schema = root?.GetDefaultNamespace()?.NamespaceName ?? string.Empty;

            if (schema.StartsWith("urn:iso:std:iso:20022:tech:xsd:camt.053.001"))
                return true;

            return false;
        }

        internal static Stream EnsureSeekable(Stream input)
        {
            if (input.CanSeek)
                return input;

            var ms = new MemoryStream();
            input.CopyTo(ms);
            ms.Position = 0;
            return ms;
        }
    }
}
