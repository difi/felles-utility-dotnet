using System.IO;
using Digipost.Api.Client.Shared.Resources.Resource;

namespace Difi.Felles.Utility.Resources.Xsd
{
    internal class XsdResource
    {
        private static readonly ResourceUtility ResourceUtility = new ResourceUtility("Difi.Felles.Utility.Resources.Xsd.Data");

        private static Stream GetResource(params string[] path)
        {
            var bytes = ResourceUtility.ReadAllBytes(path);
            return new MemoryStream(bytes);
        }

        public static Stream Sample()
        {
            return GetResource("Sample.xsd");
        }
    }
}