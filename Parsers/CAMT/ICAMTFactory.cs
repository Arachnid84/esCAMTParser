using esCAMTParser.Models.CAMT._053;
using esCAMTParser.Parsers.CAMT._053.V2;
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
                // 8 => new CAMT053ParserV8(),
                _ => throw new NotSupportedException($"Unsupported CAMT.053 version: {version}")
            };
        }
    }
}
