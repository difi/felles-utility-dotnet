using System.Linq;
using System.Security.Cryptography.X509Certificates;
using ApiClientShared;
using Difi.Felles.Utility.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Difi.Felles.Utility.Tester
{
    [TestClass]
    public class SertifikatkjedevalidatorProduksjonTester
    {
        private static readonly ResourceUtility ResourceUtility = new ResourceUtility("Difi.Felles.Utility.Tester.Testdata.Sertifikater");

        [TestClass]
        public class ErGyldigResponssertifikatMethod : SertifikatkjedevalidatorFunksjoneltTestmiljøTester
        {
            [TestMethod]
            public void ErGyldigSertifikat()
            {
                //Arrange
                var produksjonssertifikat = new X509Certificate2(ResourceUtility.ReadAllBytes(true, "Prod", "produksjonsmottakersertifikatFraOppslagstjenesten.pem"));

                //Act
                var sertifikatValidator = new Sertifikatkjedevalidator(SertifikatkjedeUtility.ProduksjonsSertifikater());
                var erGyldigResponssertifikat = sertifikatValidator.ErGyldigResponssertifikat(produksjonssertifikat);

                //Assert
                Assert.IsTrue(erGyldigResponssertifikat);
            }

            [TestMethod]
            public void ErGyldigSertifikatOgKjedestatus()
            {
                //Arrange
                var produksjonssertifikat = new X509Certificate2(ResourceUtility.ReadAllBytes(true, "Prod", "produksjonsmottakersertifikatFraOppslagstjenesten.pem"));
                X509ChainStatus[] kjedestatus;

                //Act
                var sertifikatValidator = new Sertifikatkjedevalidator(SertifikatkjedeUtility.ProduksjonsSertifikater());
                var erGyldigResponssertifikat = sertifikatValidator.ErGyldigResponssertifikat(produksjonssertifikat, out kjedestatus);

                //Assert
                const int forventetAntallStatusElementer = 0;
                Assert.IsTrue(erGyldigResponssertifikat);
                Assert.AreEqual(forventetAntallStatusElementer, kjedestatus.Length);
            }

            [TestMethod]
            public void FeilerMedSertifikatUtenGyldigKjede()
            {
                //Arrange
                var selvsignertSertifikat = new X509Certificate2(ResourceUtility.ReadAllBytes(true, "Enhetstester", "difi-enhetstester.cer"));

                //Act
                var sertifikatValidator = new Sertifikatkjedevalidator(SertifikatkjedeUtility.ProduksjonsSertifikater());

                var erGyldigResponssertifikat = sertifikatValidator.ErGyldigResponssertifikat(selvsignertSertifikat);

                //Assert
                Assert.IsFalse(erGyldigResponssertifikat);
            }

            [TestMethod]
            public void FeilerMedSertifikatUtenGyldigKjedeReturnererKjedestatus()
            {
                //Arrange
                var selvsignertSertifikat = new X509Certificate2(ResourceUtility.ReadAllBytes(true, "Enhetstester", "difi-enhetstester.cer"));

                //Act
                var sertifikatValidator = new Sertifikatkjedevalidator(SertifikatkjedeUtility.ProduksjonsSertifikater());

                X509ChainStatus[] kjedestatus;
                var erGyldigResponssertifikat = sertifikatValidator.ErGyldigResponssertifikat(selvsignertSertifikat, out kjedestatus);

                //Assert
                Assert.IsFalse(erGyldigResponssertifikat);
                Assert.IsTrue(kjedestatus.ElementAt(0).Status == X509ChainStatusFlags.UntrustedRoot);
            }
        }
    }
}