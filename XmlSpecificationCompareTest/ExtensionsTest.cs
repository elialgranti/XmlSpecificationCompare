using System.Collections;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;
using NUnit.Framework;
using XmlSpecificationCompare.XPathDiscovery;

namespace XmlSpecificationCompareTest
{
    [TestFixture]
    public class ExtensionsTest
    {
        private const string XPathSource =
      @"<?xml version=""1.0""?>
        <Root xmlns=""http://default.com"" xmlns:a=""http://a.com"">
            <Child>
                <SubChild>Text 1</SubChild>
                <SubChild><![CDATA[Text 2]]></SubChild>
                <!-- comment -->
            </Child>
            <a:Child>
                <b:SubChild attribute=""attribute"" xmlns:b=""http://b.com""/>
            </a:Child>
        </Root>";

        private XDocument _document;
        private XmlNamespaceManager _namespaceManager;


        [TestFixtureSetUp]
        public void Setup()
        {
            _document = XDocument.Parse(XPathSource);
            _namespaceManager = new XmlNamespaceManager(new NameTable());
            _namespaceManager.AddNamespace("empty", "http://default.com");
            _namespaceManager.AddNamespace("a", "http://a.com");
            _namespaceManager.AddNamespace("b", "http://b.com");
        }


        [Test]
        [TestCase("/empty:Root[1]")]
        [TestCase("/empty:Root[1]/empty:Child[1]/empty:SubChild[1]/text()")]
        [TestCase("/empty:Root[1]/empty:Child[1]/empty:SubChild[2]/text()")]
        [TestCase("/empty:Root[1]/empty:Child[1]/comment()")]
        [TestCase("/empty:Root[1]/a:Child[1]/b:SubChild[1]/@attribute")]
        public void ShouldConvertXpath(string xpath)
        {
            var node = ((IEnumerable)_document.XPathEvaluate(xpath, _namespaceManager)).Cast<XObject>().First();
            var expected = xpath.Replace("empty:", "");

            var res = node.GetXPath();

            Assert.That(res, Is.EqualTo(expected));
        }
    }
}
