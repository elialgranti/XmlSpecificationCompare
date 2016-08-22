//Eli Algranti Copyright ©  2013
using System;
using System.Xml.Linq;
using NUnit.Framework.Constraints;
using XmlSpecificationCompare.XPathDiscovery;

namespace XmlSpecificationCompare.NUnit
{
    public sealed class XmlSpecificationEqualityConstraint : Constraint
    {
        private readonly XElement _expected;

        public XmlSpecificationEqualityConstraint(object expected)
        {
            _expected = GetXElement(expected);
            Description = "XML documents have the same specification.";
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

        public override ConstraintResult ApplyTo<TActual>(TActual actual)
        {
            XmlEqualityResult result;
            try
            {
                result = XmlSpecificationEquality.AreEqual(GetXElement(actual), _expected);
            }
            catch (Exception e)
            {
                result = new XmlEqualityResult { ErrorMessage = e.Message, FailObject = _expected };
            }

            return new XmlSpecificationEqualityConstraintResult(this, actual, result);
        }

        protected override string GetStringRepresentation()
        {
            return "test";
        }
    }

    public class XmlSpecificationEqualityConstraintResult : ConstraintResult
    {
        private readonly XmlEqualityResult _result;

        public XmlSpecificationEqualityConstraintResult(XmlSpecificationEqualityConstraint constraint, object actual, XmlEqualityResult result)
            : base(constraint, actual, result.Success)
        {
            _result = result;
        }

        public override void WriteMessageTo(MessageWriter writer)
        {
            writer.WriteMessageLine("Actual XML differs from Expected XML at " + _result.FailObject.GetXPath());
            writer.WriteMessageLine("Error: " + _result.ErrorMessage);
        }
    }
    
}
