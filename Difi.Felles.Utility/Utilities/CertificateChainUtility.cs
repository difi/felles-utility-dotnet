using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using ApiClientShared;

namespace Difi.Felles.Utility.Utilities
{
    public static class CertificateChainUtility
    {
        private static readonly ResourceUtility ResourceUtility = new ResourceUtility("Difi.Felles.Utility.Resources.Certificates");

        public static X509Certificate2Collection TestCertificates()
        {
            var difiTestkjedesertifikater = new List<X509Certificate2>
            {
                new X509Certificate2(ResourceUtility.ReadAllBytes(true, "Test", "Buypass_Class_3_Test4_CA_3.cer")),
                new X509Certificate2(ResourceUtility.ReadAllBytes(true, "Test", "Buypass_Class_3_Test4_Root_CA.cer")),
                new X509Certificate2(ResourceUtility.ReadAllBytes(true, "Test", "intermediate - commfides cpn enterprise-norwegian sha256 ca - test2.crt")),
                new X509Certificate2(ResourceUtility.ReadAllBytes(true, "Test", "root - cpn root sha256 ca - test.crt"))
            };
            return new X509Certificate2Collection(difiTestkjedesertifikater.ToArray());
        }

        public static X509Certificate2Collection ProductionCertificates()
        {
            var difiProduksjonssertifikater = new List<X509Certificate2>
            {
                new X509Certificate2(ResourceUtility.ReadAllBytes(true, "Production", "BPClass3CA3.cer")),
                new X509Certificate2(ResourceUtility.ReadAllBytes(true, "Production", "BPClass3RootCA.cer")),
                new X509Certificate2(ResourceUtility.ReadAllBytes(true, "Production", "cpn enterprise sha256 class 3.crt")),
                new X509Certificate2(ResourceUtility.ReadAllBytes(true, "Production", "cpn rootca sha256 class 3.crt"))
            }
                ;
            return new X509Certificate2Collection(difiProduksjonssertifikater.ToArray());
        }
    }
}