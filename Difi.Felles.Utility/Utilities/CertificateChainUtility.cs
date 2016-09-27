using System.Security.Cryptography.X509Certificates;
using Difi.Felles.Utility.Resources.Certificate;

namespace Difi.Felles.Utility.Utilities
{
    public static class CertificateChainUtility
    {
        public static X509Certificate2Collection FunksjoneltTestmiljøSertifikater()
        {
            return new X509Certificate2Collection(CertificateResource.Chain.GetDifiTestChain().ToArray());
        }

        public static X509Certificate2Collection ProduksjonsSertifikater()
        {
            return new X509Certificate2Collection(CertificateResource.Chain.GetDifiProductionChain().ToArray());
        }
    }
}