//Eli Algranti Copyright ©  2013
using NUnit.Framework.Constraints;

namespace XmlSpecificationCompare.NUnit
{
    public static class NUnitExtension
    {
        public static Constraint XmlEquals(this ConstraintExpression exp, object expected)
        {
            return new XmlSpecificationEqualityConstraint(expected);
        }
    }
}
