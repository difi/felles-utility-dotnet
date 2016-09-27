using System.Xml;

namespace Difi.Felles.Utility.Resources.Xml
{
    public class XmlUtility
    {
        public static XmlDocument ToXmlDocument(string xml)
        {
            var xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(xml);

            return xmlDocument;
        }
    }
}