using esCAMTParser.Models.CAMT._053;
using System.Text;

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
            var factory = new Parsers.CAMT.CAMTFactory();
            var parser = factory.GetParser(xmlStream);

            return parser.Parse(xmlStream);
        }
    }
}
