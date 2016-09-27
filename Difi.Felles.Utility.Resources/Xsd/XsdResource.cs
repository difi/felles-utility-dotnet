using System.IO;
using ApiClientShared;

namespace Difi.Felles.Utility.Resources.Xsd
{
    internal class XsdResource
    {
        private static readonly ResourceUtility ResourceUtility = new ResourceUtility("Difi.Felles.Utility.Resources.Xsd.Data");

        private static Stream GetResource(params string[] path)
        {
            var bytes = ResourceUtility.ReadAllBytes(true, path);
            return new MemoryStream(bytes);
        }

        public static Stream Sample()
        {
            return GetResource("Sample.xsd");
        }
    }
}