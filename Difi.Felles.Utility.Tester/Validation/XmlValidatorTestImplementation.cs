using System.IO;
using System.Xml;
using ApiClientShared;

namespace Difi.Felles.Utility.Tester.Validation
{
    public class XmlValidatorTestImplementation : XmlValidator
    {
        private static readonly ResourceUtility ResourceUtility = new ResourceUtility("Difi.Felles.Utility.Tester.Testdata");

        public XmlValidatorTestImplementation()
        {
            AddXsd("http://tempuri.org/po.xsd", HentRessurs("Xsd.Sample.xsd"));
        }

        private static XmlReader HentRessurs(string path)
        {
            var bytes = ResourceUtility.ReadAllBytes(true, path);
            return XmlReader.Create(new MemoryStream(bytes));
        }
    }
}