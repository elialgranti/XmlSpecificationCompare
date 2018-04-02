# XmlSpecificationCompare
Compare Xml Specifications and XPath discovery

This project contains:

* A library for loosely comparing XML documents, useful for comparing the XML documents used as messages in various specifications and for configuration files:
  - Order of sibling elements is ignored.</li>
  - Namespace prefixes are ignored (namespace must still match but the prefix used can be different).
  - Namespace definition (xmlns attribute) location is ignored: as long as the namespaces are correctly defined it doesn't matter where the xmlns attribute is located in the file.
  - Assumes a single text node at the bottom of the XML document tree; so text nodes in an element are concatenated before comparison.
  - Comments are ignored.
* NUnit support.
* Utility classes for discovering the XPath of supported .Net XObjects (XElement, XAttribute, XText, XCData and XComment). These classes can also be used standalone.
You can read about how this project came to be in <a href="http://formaldev.blogspot.com.au/2013/08/introducing-xml-specification-compare.html">this blog post</a>.

Code for this project is available as Nuget Packages:

<a href="http://www.nuget.org/packages/XmlSpecificationCompare/">XmlSpecificationCompare full binary package</a>
<a href="http://www.nuget.org/packages/XmlSpecificationCompare.Code/">XmlSpecificationCompare compare class only</a>
<a href="http://www.nuget.org/packages/XPathDiscovery.Code/">XPath discovery utility classes</a>
