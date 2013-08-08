using System.Linq;
using System.Xml.Linq;
using NUnit.Framework;
using XmlSpecificationCompare;

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
            Assert.That(((XElement)result.FailObject).Name, Is.EqualTo(new XElement("{http://test.com/testing}root").Name));
            Assert.That(result.FailObject.Parent, Is.Null);
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
            Assert.That(((XElement)result.FailObject).Name, Is.EqualTo(new XElement("{http://test.com/testingA}root").Name));
            Assert.That(result.FailObject.Parent, Is.Null);
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
            var attr = result.FailObject as XAttribute;
            Assert.That(attr, Is.Not.Null);
            // ReSharper disable once PossibleNullReferenceException
            Assert.That(attr.Name, Is.EqualTo(new XAttribute("attr", "attr").Name));
            var elem = attr.Parent;
            Assert.That(elem, Is.Not.Null);
            // ReSharper disable once PossibleNullReferenceException
            Assert.That(elem.Name, Is.EqualTo(new XElement("{http://test.com/testing}elem2").Name));
            elem = elem.Parent;
            Assert.That(elem, Is.Not.Null);
            // ReSharper disable once PossibleNullReferenceException
            Assert.That(elem.Name, Is.EqualTo(new XElement("{http://test.com/testing}root").Name));
            Assert.That(elem.Parent, Is.Null);
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
            var attr = result.FailObject as XAttribute;
            Assert.That(attr, Is.Not.Null);
            // ReSharper disable once PossibleNullReferenceException
            Assert.That(attr.Name, Is.EqualTo(new XAttribute("attr", "attr").Name));
            var elem = attr.Parent;
            Assert.That(elem, Is.Not.Null);
            // ReSharper disable once PossibleNullReferenceException
            Assert.That(elem.Name, Is.EqualTo(new XElement("{http://test.com/testing}elem2").Name));
            elem = elem.Parent;
            Assert.That(elem, Is.Not.Null);
            // ReSharper disable once PossibleNullReferenceException
            Assert.That(elem.Name, Is.EqualTo(new XElement("{http://test.com/testing}root").Name));
            Assert.That(elem.Parent, Is.Null);
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
            var elem = result.FailObject as XElement;
            Assert.That(elem, Is.Not.Null);
            // ReSharper disable once PossibleNullReferenceException
            Assert.That(elem.Name, Is.EqualTo(new XElement("{http://test.com/testing}elem2").Name));
            elem = elem.Parent;
            Assert.That(elem, Is.Not.Null);
            // ReSharper disable once PossibleNullReferenceException
            Assert.That(elem.Name, Is.EqualTo(new XElement("{http://test.com/testing}root").Name));
            Assert.That(elem.Parent, Is.Null);
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
            var elem = result.FailObject as XElement;
            Assert.That(elem, Is.Not.Null);
            // ReSharper disable once PossibleNullReferenceException
            Assert.That(elem.Name, Is.EqualTo(new XElement("{http://test.com/testing}elem3").Name));
            elem = elem.Parent;
            Assert.That(elem, Is.Not.Null);
            // ReSharper disable once PossibleNullReferenceException
            Assert.That(elem.Name, Is.EqualTo(new XElement("{http://test.com/testing}root").Name));
            Assert.That(elem.Parent, Is.Null);
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
            var text = result.FailObject as XText;
            Assert.That(text, Is.Not.Null);
            // ReSharper disable once PossibleNullReferenceException
            var elem = text.Parent;
            Assert.That(elem, Is.Not.Null);
            // ReSharper disable once PossibleNullReferenceException
            Assert.That(elem.Name, Is.EqualTo(new XElement("{http://test.com/testing}elem1").Name));
            elem = elem.Parent;
            Assert.That(elem, Is.Not.Null);
            // ReSharper disable once PossibleNullReferenceException
            Assert.That(elem.Name, Is.EqualTo(new XElement("{http://test.com/testing}root").Name));
            Assert.That(elem.Parent, Is.Null);
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
            var text = result.FailObject as XCData;
            Assert.That(text, Is.Not.Null);
            // ReSharper disable once PossibleNullReferenceException
            var elem = text.Parent;
            Assert.That(elem, Is.Not.Null);
            // ReSharper disable once PossibleNullReferenceException
            Assert.That(elem.Name, Is.EqualTo(new XElement("{http://test.com/testing1}elem2").Name));
            elem = elem.Parent;
            Assert.That(elem, Is.Not.Null);
            // ReSharper disable once PossibleNullReferenceException
            Assert.That(elem.Name, Is.EqualTo(new XElement("{http://test.com/testing}elem1").Name));
            elem = elem.Parent;
            Assert.That(elem, Is.Not.Null);
            // ReSharper disable once PossibleNullReferenceException
            Assert.That(elem.Name, Is.EqualTo(new XElement("{http://test.com/testing}root").Name));
            Assert.That(elem.Parent, Is.Null);
        }
    }
}
