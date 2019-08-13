using System.Text;
using Digipost.Api.Client.Shared.Resources.Resource;

namespace Difi.Felles.Utility.Resources.Xml
{
    public class XmlResource
    {
        private static readonly ResourceUtility ResourceUtility = new ResourceUtility("Difi.Felles.Utility.Resources.Xml.Data");

        private static string GetResource(params string[] path)
        {
            var bytes = ResourceUtility.ReadAllBytes(path);
            return XmlUtility.ToXmlDocument(Encoding.UTF8.GetString(bytes)).OuterXml;
        }

        internal class GetContent
        {
            public static string GetInvalid()
            {
                return GetResource("InvalidIdentifikatorContent.xml");
            }

            public static string GetContentWithUnknownElement()
            {
                return GetResource("UnknownElement.xml");
            }

            public static string GetValid()
            {
                return GetResource("Valid.xml");
            }
        }
    }
}