using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using XmlSpecificationCompare.XPathDiscovery;

namespace XmlSpecificationCompare
{
    public class XmlEqualityResult
    {
        private const string DefaultError = "Can'f find match for subtree.";
        private const string SuccessMessage = "Success";

        public string GetXPath()
        {

            return Success ? "" : FailObject.GetXPath();
        }

        public bool Success
        {
            get { return FailObject == null; }
        }

        public XObject FailObject { get; set; }


        private string _errorMessage;

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
