using System.Collections;
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
