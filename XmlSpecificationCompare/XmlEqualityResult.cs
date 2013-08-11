//Eli Algranti Copyright ©  2013
using System.Xml.Linq;
using XmlSpecificationCompare.XPathDiscovery;

namespace XmlSpecificationCompare
{
    /// <summary>
    /// The result of an equiality comparison with <see cref="XmlSpecificationEquality"/>
    /// </summary>
    public class XmlEqualityResult
    {
        private const string DefaultError = "Can'f find match for subtree.";
        private const string SuccessMessage = "Success";

        /// <summary>
        /// Returns the XPath of the object that failed the match or the empty string if
        /// the match was successful.
        /// </summary>
        /// <returns></returns>
        public string GetXPath()
        {

            return Success ? "" : FailObject.GetXPath();
        }

        /// <summary>
        /// Gets whether the match was successful
        /// </summary>
        public bool Success
        {
            get { return FailObject == null; }
        }

        /// <summary>
        /// Gets or sets the object that failed the match
        /// </summary>
        public XObject FailObject { get; set; }


        private string _errorMessage;

        /// <summary>
        /// Gets or sets a descriptive error message if the match failed.
        /// </summary>
        /// <remarks>
        /// If set to null or not set the default Error Message is returned.
        /// </remarks>
        public string ErrorMessage
        {
            get
            {
                return _errorMessage ?? (Success ? SuccessMessage : DefaultError);
            }

            set
            {
                _errorMessage = value;
            }
        }
    }
}
