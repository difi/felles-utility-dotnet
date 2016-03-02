using System.Linq;
using System.Security.Cryptography.X509Certificates;
using ApiClientShared;
using Difi.Felles.Utility.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Difi.Felles.Utility.Tester
{
    [TestClass]
    public class SertifikatkjedevalidatorFunksjoneltTestmiljøTester
    {
        private static readonly ResourceUtility ResourceUtility = new ResourceUtility("Difi.Felles.Utility.Tester.Testdata.Sertifikater");

        [TestClass]
        public class ErGyldigResponssertifikatMethod : SertifikatkjedevalidatorFunksjoneltTestmiljøTester
        {
            [TestMethod]
            public void GodkjennerTestsertifikat()
            {
                //Arrange
                var testSertifikat = new X509Certificate2(ResourceUtility.ReadAllBytes(true, "Test", "testmottakersertifikatFraOppslagstjenesten.pem"));

                //Act
                var sertifikatValidator = new Sertifikatkjedevalidator(SertifikatkjedeUtility.FunksjoneltTestmiljøSertifikater());
                var erGyldigResponssertifikat = sertifikatValidator.ErGyldigSertifikatkjede(testSertifikat);

                //Assert
                Assert.IsTrue(erGyldigResponssertifikat);
            }

            [TestMethod]
            public void ErGyldigSertifikatOgKjedestatus()
            {
                //Arrange
                var testSertifikat = new X509Certificate2(ResourceUtility.ReadAllBytes(true, "Test", "testmottakersertifikatFraOppslagstjenesten.pem"));
                X509ChainStatus[] kjedestatus;

                //Act
                var sertifikatValidator = new Sertifikatkjedevalidator(SertifikatkjedeUtility.FunksjoneltTestmiljøSertifikater());
                var erGyldigResponssertifikat = sertifikatValidator.ErGyldigSertifikatkjede(testSertifikat, out kjedestatus);

                //Assert
                Assert.IsTrue(erGyldigResponssertifikat);
                Assert.IsTrue(kjedestatus.Length == 0 || kjedestatus.ElementAt(0).Status == X509ChainStatusFlags.UntrustedRoot);
            }

            [TestMethod]
            public void FeilerMedSertifikatUtenGyldigKjede()
            {
                //Arrange
                var selvsignertSertifikat = new X509Certificate2(ResourceUtility.ReadAllBytes(true, "Enhetstester", "difi-enhetstester.cer"));

                //Act
                var sertifikatValidator = new Sertifikatkjedevalidator(SertifikatkjedeUtility.FunksjoneltTestmiljøSertifikater());

                var erGyldigResponssertifikat = sertifikatValidator.ErGyldigSertifikatkjede(selvsignertSertifikat);

                //Assert
                Assert.IsFalse(erGyldigResponssertifikat);
            }

            [TestMethod]
            public void FeilerMedSertifikatUtenGyldigKjedeReturnererKjedestatus()
            {
                var selvsignertSertifikat = new X509Certificate2(ResourceUtility.ReadAllBytes(true, "Enhetstester", "difi-enhetstester.cer"));
                //Act
                var sertifikatValidator = new Sertifikatkjedevalidator(SertifikatkjedeUtility.FunksjoneltTestmiljøSertifikater());

                X509ChainStatus[] kjedestatus;
                var erGyldigResponssertifikat = sertifikatValidator.ErGyldigSertifikatkjede(selvsignertSertifikat, out kjedestatus);

                //Assert
                Assert.IsFalse(erGyldigResponssertifikat);
                Assert.IsTrue(kjedestatus.ElementAt(0).Status == X509ChainStatusFlags.UntrustedRoot);
            }
        }
    }
}