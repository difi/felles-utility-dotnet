using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Schema;
using ApiClientShared;

namespace Difi.Felles.Utility.Tester.Validation
{
    internal static class TestGenerator
    {
        private static readonly ResourceUtility ResourceUtility = new ResourceUtility("Difi.Felles.Utility.Tester.Testdata");

        public static XmlSchemaSet XmlSchemaSet()
        {
            var xmlSchemaSet = new XmlSchemaSet();
            xmlSchemaSet.Add("http://tempuri.org/po.xsd", XmlReader.Create(new MemoryStream(ResourceUtility.ReadAllBytes(true, "Xsd.Sample.xsd"))));
            return xmlSchemaSet;
        }

        public interface ITestCouple
        {
            List<string> ExpectedValidationMessages { get; }

            string Input();
        }

        public class ValidTestCouple : ITestCouple
        {
            public List<string> ExpectedValidationMessages => new List<string>();

            public string Input()
            {
                return Encoding.UTF8.GetString(ResourceUtility.ReadAllBytes(true, "Xml.Valid.xml"));
            }
        }

        public class InvalidContentTestCouple : ITestCouple
        {
            public List<string> ExpectedValidationMessages
            {
                get
                {
                    const string validationMessageEn = "The 'http://tempuri.org/po.xsd:Identifikator' element is invalid - The value 'invalidContent' is invalid according to its datatype 'String' - The Pattern constraint failed.";
                    const string validationMessageNb = "Elementet http://tempuri.org/po.xsd:Identifikator er ugyldig - Verdien invalidContent er ugyldig i henhold til datatypen String - Pattern-begrensningen mislyktes.";
                    return new List<string> {validationMessageEn, validationMessageNb};
                }
            }

            public string Input()
            {
                return Encoding.UTF8.GetString(ResourceUtility.ReadAllBytes(true, "Xml.InvalidIdentifikatorContent.xml"));
            }
        }

        public class InvalidSyntaxTestCouple : ITestCouple
        {
            public List<string> ExpectedValidationMessages
            {
                get
                {
                    const string validationMessageEn = "The element 'Forespoersel' in namespace 'http://tempuri.org/po.xsd' has invalid child element 'blabla' in namespace 'http://tempuri.org/po.xsd'. List of possible elements expected: 'Identifikator' in namespace 'http://tempuri.org/po.xsd'.";
                    const string validationMessageNb = "Elementet Forespoersel i navneområdet http://tempuri.org/po.xsd har ugyldig underordnet element blabla i navneområdet http://tempuri.org/po.xsd. Forventet liste over mulige elementer: Identifikator i navneområdet http://tempuri.org/po.xsd.";
                    return new List<string> {validationMessageEn, validationMessageNb};
                }
            }

            public string Input()
            {
                return Encoding.UTF8.GetString(ResourceUtility.ReadAllBytes(true, "Xml.UnknownElement.xml"));
            }
        }
    }
}