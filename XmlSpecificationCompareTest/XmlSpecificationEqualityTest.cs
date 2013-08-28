//Eli Algranti Copyright ©  2013
using System.Xml.Linq;
using NUnit.Framework;
using XmlSpecificationCompare;
using XmlSpecificationCompare.XPathDiscovery;

namespace XmlSpecificationCompareTest
{
    [TestFixture]
    public class XmlSpecificationEqualityTest
    {
        [Test]
        public void ShouldSucceedForEqualRoot()
        {
            const string xmlA = @"<?xml version=""1.0"" encoding=""UTF-8""?>
              <a:root xmlns:a=""http://test.com/testing""/>";

            const string xmlB = @"<?xml version=""1.0"" encoding=""UTF-8""?>
              <root xmlns=""http://test.com/testing"">
              </root>";

            var result = XmlSpecificationEquality.AreEqual(xmlA, xmlB);

            Assert.That(result.Success, Is.True);
            Assert.That(result.FailObject, Is.Null);
        }

        [Test]
        public void ShouldFailForRootWithDifferentNamespace()
        {
            const string xmlA = @"<?xml version=""1.0"" encoding=""UTF-8""?>
              <a:root xmlns:a=""http://test.com/testing"">
              </a:root>";

            const string xmlB = @"<?xml version=""1.0"" encoding=""UTF-8""?>
              <root>
              </root>";

            var result = XmlSpecificationEquality.AreEqual(xmlA, xmlB);

            Assert.That(result.Success, Is.False);
            Assert.That(result.FailObject, Is.TypeOf<XElement>());
            Assert.That(result.FailObject.GetXPath(), Is.EqualTo("/a:root[1]"));
        }

        [Test]
        public void ShouldSucceedForEqualXmlsSimple()
        {
            const string xmlA = @"<?xml version=""1.0"" encoding=""UTF-8""?>
              <a:root xmlns:a=""http://test.com/testing"">
                  <a:elem1 attr1=""attr1"" attr2=""attr2"">
                    value
                  </a:elem1>
                  <c:elem2 xmlns:c=""http://test.com/testing1"">
                  </c:elem2>
              </a:root>";

            const string xmlB = @"<?xml version=""1.0"" encoding=""UTF-8""?>
              <root xmlns=""http://test.com/testing"">
                  <b:elem2 xmlns:b=""http://test.com/testing1""/>
                  <elem1 attr2=""attr2"" attr1=""attr1"">value</elem1>
              </root>";


            var result = XmlSpecificationEquality.AreEqual(xmlA, xmlB);

            Assert.That(result.Success, Is.True);
            Assert.That(result.FailObject, Is.Null);
        }

        [Test]
        public void ShouldSucceedForEqualXmlsComplex()
        {
            const string xmlA = @"<?xml version=""1.0"" encoding=""UTF-8""?>
              <a:root xmlns:a=""http://test.com/testing"">
                  <a:elem1 attr1=""attr1"" attr2=""attr2"">
                    value
                  </a:elem1>
                  <a:elem1 attr1=""attr1"" attr2=""attr2"">
                      <c:elem2 xmlns:c=""http://test.com/testing1"">
                      </c:elem2>
                      <d:elem2 xmlns:d=""http://test.com/testing1"">
                      <![CDATA[Text]]>
                      </d:elem2>
                  </a:elem1>
              </a:root>";

            const string xmlB = @"<?xml version=""1.0"" encoding=""UTF-8""?>
              <root xmlns=""http://test.com/testing"">
                  <elem1 attr1=""attr1"" attr2=""attr2"" xmlns:b=""http://test.com/testing1"">
                      <b:elem2 xmlns:c=""http://test.com/testing1"">Text</b:elem2>
                      <b:elem2 />
                  </elem1>
                  <elem1 attr1=""attr1"" attr2=""attr2"">value</elem1>
              </root>";

            var result = XmlSpecificationEquality.AreEqual(xmlA, xmlB);

            Assert.That(result.Success, Is.True);
            Assert.That(result.FailObject, Is.Null);
        }

        [Test]
        public void ShouldfailForXmlsDifferingNamespace()
        {
            const string xmlA = @"<?xml version=""1.0"" encoding=""UTF-8""?>
              <root xmlns=""http://test.com/testingA"">
                  <elem1 attr1=""attr1"" attr2=""attr2"">
                    value
                  </elem1>
                  <elem2 >
                  </elem2>
              </root>";

            const string xmlB = @"<?xml version=""1.0"" encoding=""UTF-8""?>
              <root xmlns=""http://test.com/testingB"">
                  <elem2/>
                  <elem1 attr2=""attr2"" attr1=""attr1"">value</elem1>
              </root>";

            var result = XmlSpecificationEquality.AreEqual(xmlA, xmlB);

            Assert.That(result.Success, Is.False);
            Assert.That(result.FailObject, Is.TypeOf<XElement>());
            Assert.That(result.FailObject.GetXPath(), Is.EqualTo("/root[1]"));
        }

        [Test]
        public void ShouldfailForXmlsDifferingInAttributes()
        {
            const string xmlA = @"<?xml version=""1.0"" encoding=""UTF-8""?>
              <root xmlns=""http://test.com/testing"">
                  <elem1 attr1=""attr1"" attr2=""attr2"">
                    value
                  </elem1>
                  <elem2 attr=""attr"">
                  </elem2>
              </root>";

            const string xmlB = @"<?xml version=""1.0"" encoding=""UTF-8""?>
              <root xmlns=""http://test.com/testing"">
                  <elem2 otherAttr=""attr"" />
                  <elem1 attr2=""attr2"" attr1=""attr1"">value</elem1>
              </root>";

            var result = XmlSpecificationEquality.AreEqual(xmlA, xmlB);

            Assert.That(result.Success, Is.False);
            Assert.That(result.FailObject, Is.TypeOf<XAttribute>());
            Assert.That(result.FailObject.GetXPath(), Is.EqualTo("/root[1]/elem2[1]/@attr"));
        }

        [Test]
        public void ShouldfailForXmlsDifferingInAttributeValue()
        {
            const string xmlA = @"<?xml version=""1.0"" encoding=""UTF-8""?>
              <root xmlns=""http://test.com/testing"">
                  <elem1 attr1=""attr1"" attr2=""attr2"">
                    value
                  </elem1>
                  <elem2 attr=""attr"">
                  </elem2>
              </root>";

            const string xmlB = @"<?xml version=""1.0"" encoding=""UTF-8""?>
              <root xmlns=""http://test.com/testing"">
                  <elem2 attr=""otherattr"" />
                  <elem1 attr2=""attr2"" attr1=""attr1"">value</elem1>
              </root>";

            var result = XmlSpecificationEquality.AreEqual(xmlA, xmlB);


            Assert.That(result.Success, Is.False);
            Assert.That(result.FailObject, Is.TypeOf<XAttribute>());
            Assert.That(result.FailObject.GetXPath(), Is.EqualTo("/root[1]/elem2[1]/@attr"));
        }

        [Test]
        public void ShouldfailForXmlsDifferingInAttributeCount()
        {
            const string xmlA = @"<?xml version=""1.0"" encoding=""UTF-8""?>
              <root xmlns=""http://test.com/testing"">
                  <elem1 attr1=""attr1"" attr2=""attr2"">
                    value
                  </elem1>
                  <elem2 attr=""attr"">
                  </elem2>
              </root>";

            const string xmlB = @"<?xml version=""1.0"" encoding=""UTF-8""?>
              <root xmlns=""http://test.com/testing"">
                  <elem2/>
                  <elem1 attr2=""attr2"" attr1=""attr1"">value</elem1>
              </root>";

            var result = XmlSpecificationEquality.AreEqual(xmlA, xmlB);

            Assert.That(result.Success, Is.False);
            Assert.That(result.FailObject, Is.TypeOf<XElement>());
            Assert.That(result.FailObject.GetXPath(), Is.EqualTo("/root[1]/elem2[1]"));
        }


        [Test]
        public void ShouldfailForXmlsDifferingInElement()
        {
            const string xmlA = @"<?xml version=""1.0"" encoding=""UTF-8""?>
              <root xmlns=""http://test.com/testing"">
                  <elem1 attr1=""attr1"" attr2=""attr2"">
                    value
                  </elem1>
                  <elem3>
                  </elem3>
              </root>";

            const string xmlB = @"<?xml version=""1.0"" encoding=""UTF-8""?>
              <root xmlns=""http://test.com/testing"">
                  <elem2/>
                  <elem1 attr2=""attr2"" attr1=""attr1"">value</elem1>
              </root>";

            var result = XmlSpecificationEquality.AreEqual(xmlA, xmlB);

            Assert.That(result.Success, Is.False);
            Assert.That(result.FailObject, Is.TypeOf<XElement>());
            Assert.That(result.FailObject.GetXPath(), Is.EqualTo("/root[1]/elem3[1]"));
        }

        [Test]
        public void ShouldfailForXmlsDifferingInValue()
        {
            const string xmlA = @"<?xml version=""1.0"" encoding=""UTF-8""?>
              <root xmlns=""http://test.com/testing"">
                  <elem1 attr1=""attr1"" attr2=""attr2"">
                    value1
                  </elem1>
                  <elem2>
                  </elem2>
              </root>";

            const string xmlB = @"<?xml version=""1.0"" encoding=""UTF-8""?>
              <root xmlns=""http://test.com/testing"">
                  <elem2/>
                  <elem1 attr2=""attr2"" attr1=""attr1"">value</elem1>
              </root>";

            var result = XmlSpecificationEquality.AreEqual(xmlA, xmlB);


            Assert.That(result.Success, Is.False);
            Assert.That(result.FailObject, Is.TypeOf<XText>());
            Assert.That(result.FailObject.GetXPath(), Is.EqualTo("/root[1]/elem1[1]/text()"));
        }

        [Test]
        public void ShouldFailForCDataMismatch()
        {
            const string xmlA = @"<?xml version=""1.0"" encoding=""UTF-8""?>
              <a:root xmlns:a=""http://test.com/testing"">
                  <a:elem1 attr1=""attr1"" attr2=""attr2"">
                    value
                  </a:elem1>
                  <a:elem1 attr1=""attr1"" attr2=""attr2"">
                      <c:elem2 xmlns:c=""http://test.com/testing1"">
                      </c:elem2>
                      <d:elem2 xmlns:d=""http://test.com/testing1"">
                      <![CDATA[
                        Text]]>
                      </d:elem2>
                  </a:elem1>
              </a:root>";

            const string xmlB = @"<?xml version=""1.0"" encoding=""UTF-8""?>
              <root xmlns=""http://test.com/testing"">
                  <elem1 attr1=""attr1"" attr2=""attr2"" xmlns:b=""http://test.com/testing1"">
                      <b:elem2 xmlns:c=""http://test.com/testing1"">
                        <![CDATA[Text]]>
                      </b:elem2>
                      <b:elem2 />
                  </elem1>
                  <elem1 attr1=""attr1"" attr2=""attr2"">value</elem1>
              </root>";

            var result = XmlSpecificationEquality.AreEqual(xmlA, xmlB);


            Assert.That(result.Success, Is.False);
            Assert.That(result.FailObject, Is.TypeOf<XCData>());
            Assert.That(result.FailObject.GetXPath(), Is.EqualTo("/a:root[1]/a:elem1[2]/d:elem2[2]/text()"));
        }
    }
}
