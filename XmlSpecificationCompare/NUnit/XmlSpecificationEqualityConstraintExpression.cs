using System;
using System.Xml.Linq;
using NUnit.Framework.Constraints;

namespace XmlSpecificationCompare.NUnit
{
    public class XmlSpecificationEqualityConstraint : Constraint
    {
        private readonly XElement _expected;
        private XmlEqualityResult _result;

        public XmlSpecificationEqualityConstraint(object expected)
        {
            _expected = GetXElement(expected);
        }

        private static XElement GetXElement(object element)
        {
            var xelement = element as XElement;
            if (xelement != null)
                return xelement;

            var s = element as string;
            if (s != null)
                return XmlSpecificationEquality.ParseXml(s).Root;

            throw new ArgumentException("Cannot test this type of object.");
        }

        public override bool Matches(object actualObject)
        {
            try
            {
                _result = XmlSpecificationEquality.AreEqual(GetXElement(actualObject), _expected);
                return _result.Success;
            }
            catch (Exception e)
            {
                _result = new XmlEqualityResult {ErrorMessage = e.Message, FailObject = _expected};
                return false;
            }
        }

        public override void WriteDescriptionTo(MessageWriter writer)
        {
            writer.WriteExpectedValue(_expected);
        }

        public override void WriteMessageTo(MessageWriter writer)
        {
            writer.WriteMessageLine(_result.ErrorMessage + "\r\n" + "Failed at: " + _result.GetXPath());
        }
    }
}
