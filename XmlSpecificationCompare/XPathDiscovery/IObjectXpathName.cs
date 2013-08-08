using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace XmlSpecificationCompare.XPathDiscovery
{
    internal interface IObjectXpathName
    {
        string GetXpathName(XObject node, IDictionary<string, string> namespacePrefixes);
    }
}
