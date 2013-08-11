//Eli Algranti Copyright ©  2013
using NUnit.Framework;
using XmlSpecificationCompare.NUnit;

namespace XmlSpecificationCompareTest
{
    [TestFixture]
    public class NUnitSupportTest
    {
        [Test]
        public void ShouldSucceed()
        {
            const string xmlA = @"<?xml version=""1.0"" encoding=""UTF-8""?>
              <a:root xmlns:a=""http://test.com/testing""/>";

            const string xmlB = @"<?xml version=""1.0"" encoding=""UTF-8""?>
              <root xmlns=""http://test.com/testing"">
              </root>";

            var action = new TestDelegate(() => Assert.That(xmlA, new XmlSpecificationEqualityConstraint(xmlB)));

            Assert.That(action, Throws.Nothing);
        }

        [Test]
        public void ShouldFail()
        {
            const string xmlA = @"<?xml version=""1.0"" encoding=""UTF-8""?>
              <a:root xmlns:a=""http://test.com/testing""/>";

            const string xmlB = @"<?xml version=""1.0"" encoding=""UTF-8""?>
              <root1 xmlns=""http://test.com/testing"">
              </root1>";

            var action = new TestDelegate(() => Assert.That(xmlA, new XmlSpecificationEqualityConstraint(xmlB)));

            Assert.That(action, Throws.TypeOf<AssertionException>());
        }

        [Test]
        public void ShouldSucceedWithXmlIs()
        {
            const string xmlA = @"<?xml version=""1.0"" encoding=""UTF-8""?>
              <a:root xmlns:a=""http://test.com/testing""/>";

            const string xmlB = @"<?xml version=""1.0"" encoding=""UTF-8""?>
              <root xmlns=""http://test.com/testing"">
              </root>";

            var action = new TestDelegate(() => Assert.That(xmlA, XmlIs.SpecificationEquals(xmlB)));

            Assert.That(action, Throws.Nothing);
        }

        [Test]
        public void ShouldFailWithIsNot()
        {
            const string xmlA = @"<?xml version=""1.0"" encoding=""UTF-8""?>
              <a:root xmlns:a=""http://test.com/testing""/>";

            const string xmlB = @"<?xml version=""1.0"" encoding=""UTF-8""?>
              <root1 xmlns=""http://test.com/testing"">
              </root1>";

            var action = new TestDelegate(() => Assert.That(xmlA, Is.Not.XmlEquals(xmlB)));

            Assert.That(action, Throws.TypeOf<AssertionException>());
        }
    }
}
