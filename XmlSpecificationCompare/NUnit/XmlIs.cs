//Eli Algranti Copyright ©  2013
using NUnit.Framework.Constraints;

namespace XmlSpecificationCompare.NUnit
{
    public static class XmlIs
    {
        public static Constraint SpecificationEquals(object expected)
        {
            return new XmlSpecificationEqualityConstraint(expected);
        }
    }
}
