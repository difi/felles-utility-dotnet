using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using Digipost.Api.Client.Shared.Resources.Resource;

[assembly: InternalsVisibleTo("Difi.Felles.Utility,PublicKey=0024000004800000940000000602000000240000525341310004000001000100f71f491a4cebe0a3d18a61744f92edfca908e4d756aa1140ebceeffb1fc4aa2e7bbe4d672067e2c0a3afd8c4511ef84cc1267ba04d8041e24d96c3d93e268fd69abc712fa81bcbae729f1c0524eef0254705bb2fcf1ffd43a647e9306b93e8dd7afd094a61ca2761fe87c20fdda758ad55d2c5ba6ad6edc9493309a355e51f99")]
[assembly: InternalsVisibleTo("DynamicProxyGenAssembly2,PublicKey=0024000004800000940000000602000000240000525341310004000001000100c547cac37abd99c8db225ef2f6c8a3602f3b3606cc9891605d02baa56104f4cfc0734aa39b93bf7852f7d9266654753cc297e7d2edfe0bac1cdcf9f717241550e0a7b191195b7667bb4f64bcb8e2121380fd1d9d46ad2d92d2d15605093924cceaf74c4861eff62abf69b9291ed0a340e113be11e6a7d3113e92484cf7045cc7")]

namespace Difi.Felles.Utility.Resources.Certificate
{
    internal class CertificateResource
    {
        private static readonly ResourceUtility ResourceUtility = new ResourceUtility(Assembly.GetExecutingAssembly(),"Certificate.Data");

        internal static X509Certificate2 GetCertificate(params string[] path)
        {
            byte[] cert = ResourceUtility.ReadAllBytes(path);
            return new X509Certificate2(cert, "", X509KeyStorageFlags.Exportable);
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
                return GetFunksjoneltTestmiljøMottakerSertifikatOppslagstjenesten();
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
