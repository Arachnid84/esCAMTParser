using esCAMTParser.Models.CAMT._053;
using esCAMTParser.Parsers.CAMT._053.V2;
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
            var document = XDocument.Load(xmlStream);
            var root = document.Root;

            var schema = root?.GetDefaultNamespace()?.NamespaceName ?? string.Empty;

            if (schema.Contains("camt.053.001.02"))
                return new CAMT053ParserV2();

            //if (schema.Contains("camt.053.001.08"))
                //return new CAMT053V8Parser();

            throw new NotSupportedException($"Unsupported CAMT version: {schema}");
        }
    }
}
