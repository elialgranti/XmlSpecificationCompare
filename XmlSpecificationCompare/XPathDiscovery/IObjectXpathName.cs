using System.Collections.Generic;
using System.Xml.Linq;

namespace XmlSpecificationCompare.XPathDiscovery
{
    internal interface IObjectXpathName
    {
        string GetXpathName(XObject node, IDictionary<string, string> namespacePrefixes);
    }
}
