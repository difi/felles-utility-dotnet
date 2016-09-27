using System.Xml;
using Difi.Felles.Utility.Resources.Xsd;

namespace Difi.Felles.Utility.Tester.Validation
{
    public class XmlValidatorTestImplementation : XmlValidator
    {
        public XmlValidatorTestImplementation()
        {
            AddXsd("http://tempuri.org/po.xsd", XmlReader.Create(XsdResource.Sample()));
        }
    }
}