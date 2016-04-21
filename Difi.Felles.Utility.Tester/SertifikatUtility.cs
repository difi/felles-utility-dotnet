using System.Security.Cryptography.X509Certificates;
using ApiClientShared;

namespace Difi.Felles.Utility.Tester
{
    internal class SertifikatUtility
    {
        private static readonly ResourceUtility ResourceUtility = new ResourceUtility("Difi.Felles.Utility.Tester.Testdata.Sertifikater");

        public static X509Certificate2 GetProduksjonsMottakerSertifikatOppslagstjenesten()
        {
            return new X509Certificate2(ResourceUtility.ReadAllBytes(true, "Prod", "produksjonsmottakersertifikatFraOppslagstjenesten.pem"));
        }

        public static X509Certificate2 GetFunksjoneltTestmiljøMottakerSertifikatOppslagstjenesten()
        {
            return new X509Certificate2(ResourceUtility.ReadAllBytes(true, "Test", "testmottakersertifikatFraOppslagstjenesten.pem"));
        }

        public static X509Certificate2 NotActivatedTestCertificate()
        {
            return new X509Certificate2(ResourceUtility.ReadAllBytes(true, "Enhetstester", "NotActivatedTestCertificate.cer"));
        }

        public static X509Certificate2 GetExpiredTestCertificate()
        {
            return new X509Certificate2(ResourceUtility.ReadAllBytes(true, "Enhetstester", "ExpiredTestCertificate.cer"));
        }

        public static X509Certificate2 TestIntegrasjonssertifikat()
        {
            return GetPostenCertificate();
        }

        public static X509Certificate2 GetEnhetstesterSelvsignertSertifikat()
        {
            return new X509Certificate2(ResourceUtility.ReadAllBytes(true, "Enhetstester", "difi-enhetstester.cer"));
        }

        public static X509Certificate2 GetPostenCertificate()
        {
            return new X509Certificate2(ResourceUtility.ReadAllBytes(true, "Enhetstester", "PostenNorgeAs.cer"));
        }
    }
}