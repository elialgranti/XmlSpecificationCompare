using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace XmlSpecificationCompare.XPathDiscovery
{
    public static class XpathExtension
    {
        private static readonly IDictionary<Type, IObjectXpathName> XpathNames = 
            new Dictionary<Type, IObjectXpathName>
            {
                {typeof(XElement), new ElementXPathName()},
                {typeof(XAttribute), new AttributeXPathName()},
                {typeof(XText), new TextXPathName()},
                {typeof(XCData), new TextXPathName()},
                {typeof(XComment), new CommentXPathName()}
            };

        public static string GetXPath(this XObject xObject)
        {
            var builder = new StringBuilder();

            var namespacePrefixes = new Dictionary<string, string>();
            var curObject = xObject;
            while (curObject != null)
            {
                var xElem = curObject as XElement;
                if (xElem != null)
                    StoreNamespacePrefixed(xElem, namespacePrefixes);
                curObject = curObject.Parent;
            }

            curObject = xObject;

            while (curObject != null)
            {
                builder.Insert(0, GetXPathName(curObject, namespacePrefixes));
                builder.Insert(0, '/');
                curObject = curObject.Parent;
            }

            
            return builder.ToString();
        }


        private static string GetXPathName(XObject xObject, IDictionary<string, string> namespacePrefixes)
        {
            return XpathNames[xObject.GetType()].GetXpathName(xObject, namespacePrefixes);
        }

        public static int GetCardinality(this XElement xElement)
        {
            return xElement.ElementsBeforeSelf().Count(sib => sib.Name.Equals(xElement.Name)) + 1;
        }

        internal static string BuildXpathName(string preffix, string name, int cardinality=0)
        {
            var builder = new StringBuilder();

            if (!String.IsNullOrEmpty(preffix))
            {
                builder.Append(preffix);
                builder.Append(':');
            }

            builder.Append(name);

            if (cardinality > 0)
            {
                builder.Append('[');
                builder.Append(cardinality);
                builder.Append(']');
            }
            return builder.ToString();
        }

        private static void StoreNamespacePrefixed(XElement xElem, IDictionary<string, string> namespacePrefixes)
        {
            var nsAttributes = xElem.Attributes().Where(a => a.IsNamespaceDeclaration);

            foreach (var nsAttribute in nsAttributes)
            {
                var prefix = nsAttribute.Name.NamespaceName == String.Empty
                    ? String.Empty
                    : nsAttribute.Name.LocalName;
                namespacePrefixes.Add(nsAttribute.Value, prefix);
            }
        }
    }
}
