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
