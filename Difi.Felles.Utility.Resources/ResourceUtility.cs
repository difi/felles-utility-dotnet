using System.IO;

namespace Difi.Felles.Utility.Resources
{
    internal class ResourceUtility
    {
        private readonly string _ns;

        public ResourceUtility(string ns)
        {
            _ns = ns;
        }

        public MemoryStream GetMemoryStream(string[] path)
        {
            var ms = new MemoryStream();
            var assembly = GetType().Assembly;
            assembly.GetManifestResourceStream($"{_ns}.{string.Join(".", path)}").CopyTo(ms);
            ms.Seek(0, SeekOrigin.Begin);
            return ms;
        }
    }
}