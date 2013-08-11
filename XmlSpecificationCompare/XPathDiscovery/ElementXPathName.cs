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

    internal class AttributeXPathName : IObjectXpathName
    {
        public string GetXpathName(XObject node, IDictionary<string, string> namespacePrefixes)
        {
            var xAttr = (XAttribute)node;
            string preffix;
            namespacePrefixes.TryGetValue(xAttr.Name.NamespaceName, out preffix);

            return "@" + XpathExtension.BuildXpathName(preffix,
                                                 xAttr.Name.LocalName);
        }
    }

    internal class TextXPathName : IObjectXpathName
    {
        public string GetXpathName(XObject node, IDictionary<string, string> namespacePrefixes)
        {
            return "text()";
        }
    }

    internal class CommentXPathName : IObjectXpathName
    {
        public string GetXpathName(XObject node, IDictionary<string, string> namespacePrefixes)
        {
            return "comment()";
        }
    }


}
