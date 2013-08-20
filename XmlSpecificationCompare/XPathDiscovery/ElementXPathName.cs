//Eli Algranti Copyright ©  2013
using System.Collections.Generic;
using System.Xml.Linq;

namespace XmlSpecificationCompare.XPathDiscovery
{
    internal class ElementXPathName : IObjectXpathName
    {
        public string GetXpathName(XObject node, IDictionary<string, string> namespacePrefixes)
        {
            var xElem = (XElement)node;
            string preffix;
            namespacePrefixes.TryGetValue(xElem.Name.NamespaceName, out preffix);

            return XpathExtension.BuildXpathName(preffix,
                                                 xElem.Name.LocalName,
                                                 xElem.GetCardinality());
        }
    }
}
