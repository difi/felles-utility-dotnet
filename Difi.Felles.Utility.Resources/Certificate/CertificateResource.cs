using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using ApiClientShared;

namespace Difi.Felles.Utility.Resources.Certificate
{
    internal class CertificateResource
    {
        private static readonly ResourceUtility ResourceUtility = new ResourceUtility("Difi.Felles.Utility.Resources.Certificate.Data");

        internal static X509Certificate2 GetCertificate(params string[] path)
        {
            return new X509Certificate2(ResourceUtility.ReadAllBytes(true, path), "", X509KeyStorageFlags.Exportable);
        }

        public static class UnitTests
        {
            public static X509Certificate2 GetProduksjonsMottakerSertifikatOppslagstjenesten()
            {
                return GetCertificate("UnitTests", "produksjonsmottakersertifikatFraOppslagstjenesten.pem");
            }

            public static X509Certificate2 GetFunksjoneltTestmiljøMottakerSertifikatOppslagstjenesten()
            {
                return GetCertificate("UnitTests", "testmottakersertifikatFraOppslagstjenesten.pem");
            }

            public static X509Certificate2 NotActivatedSelfSignedTestCertificate()
            {
                return GetCertificate("UnitTests", "NotActivatedSelfSignedBringAs.cer");
            }

            public static X509Certificate2 GetExpiredSelfSignedTestCertificate()
            {
                return GetCertificate("UnitTests", "ExpiredSelfSignedBringAs.cer");
            }

            public static X509Certificate2 GetValidSelfSignedTestCertificate()
            {
                return GetCertificate("UnitTests", "ValidSelfSignedBringAs.cer");
            }

            public static X509Certificate2 TestIntegrasjonssertifikat()
            {
                return GetPostenCertificate();
            }

            public static X509Certificate2 GetEnhetstesterSelvsignertSertifikat()
            {
                return GetCertificate("UnitTests", "difi-enhetstester.cer");
            }

            public static X509Certificate2 GetPostenCertificate()
            {
                return GetCertificate("UnitTests", "PostenNorgeAs.cer");
            }

            internal static X509Certificate2 GetAvsenderEnhetstesterSertifikat()
            {
                return EvigTestSertifikatMedPrivatnøkkel();
            }

            internal static X509Certificate2 GetMottakerEnhetstesterSertifikat()
            {
                return EvigTestSertifikatUtenPrivatnøkkel();
            }

            private static X509Certificate2 EvigTestSertifikatUtenPrivatnøkkel()
            {
                return GetCertificate("UnitTests", "difi-enhetstester.cer");
            }

            private static X509Certificate2 EvigTestSertifikatMedPrivatnøkkel()
            {
                return GetCertificate("UnitTests", "difi-enhetstester.p12");
            }
        }

        public static class Chain
        {
            public static List<X509Certificate2> GetDifiTestChain()
            {
                return new List<X509Certificate2>
                {
                    new X509Certificate2(GetCertificate("TestChain", "Buypass_Class_3_Test4_CA_3.cer")),
                    new X509Certificate2(GetCertificate("TestChain", "Buypass_Class_3_Test4_Root_CA.cer")),
                    new X509Certificate2(GetCertificate("TestChain", "intermediate - commfides cpn enterprise-norwegian sha256 ca - test2.crt")),
                    new X509Certificate2(GetCertificate("TestChain", "root - cpn root sha256 ca - test.crt"))
                };
            }

            public static List<X509Certificate2> GetDifiProductionChain()
            {
                return new List<X509Certificate2>
                {
                    new X509Certificate2(GetCertificate("ProdChain", "BPClass3CA3.cer")),
                    new X509Certificate2(GetCertificate("ProdChain", "BPClass3RootCA.cer")),
                    new X509Certificate2(GetCertificate("ProdChain", "cpn enterprise sha256 class 3.crt")),
                    new X509Certificate2(GetCertificate("ProdChain", "cpn rootca sha256 class 3.crt"))
                };
            }
        }
    }
}