using System.IO;

namespace Difi.Felles.Utility.Resources.Xsd
{
    internal class XsdResource
    {
        private static readonly ResourceUtility ResourceUtility = new ResourceUtility("Difi.Felles.Utility.Resources.Xsd.Data");

        private static MemoryStream GetResource(params string[] path)
        {
            return ResourceUtility.GetMemoryStream(path);
        }

        public static MemoryStream Sample()
        {
            return GetResource("Sample.xsd");
        }
    }
}