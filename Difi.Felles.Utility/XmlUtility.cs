using System.Xml;

namespace Difi.Felles.Utility
{
    public class XmlUtility
    {
        public static XmlDocument TilXmlDokument(string xml)
        {
            var xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(xml);

            return xmlDocument;
        }
    }
}
