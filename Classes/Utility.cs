using System.Xml.Linq;

namespace esCAMTParser.Classes
{
    public static class Utility
    {
        public static XElement? ElementAnyNs(this XElement? parent, string localName)
        {
            return parent?.Elements().FirstOrDefault(e => e.Name.LocalName == localName);
        }
        public static IEnumerable<XElement> ElementsAnyNs(this XElement element, string localName)
        {
            return element.Elements().Where(e => e.Name.LocalName == localName);
        }

    }
}
