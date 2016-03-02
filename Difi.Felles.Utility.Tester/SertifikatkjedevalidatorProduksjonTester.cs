using System.Linq;
using System.Security.Cryptography.X509Certificates;
using ApiClientShared;
using Difi.Felles.Utility.Tester.Utilities;
using Difi.Felles.Utility.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Difi.Felles.Utility.Tester
{
    [TestClass]
    public class SertifikatkjedevalidatorProduksjonTester
    {
        private static readonly ResourceUtility ResourceUtility = new ResourceUtility("Difi.Felles.Utility.Tester.Testdata.Sertifikater");

        [TestClass]
        public class ErGyldigSertifikatkjedeMethod : SertifikatkjedevalidatorFunksjoneltTestmiljøTester
        {
            [TestMethod]
            public void ErGyldigSertifikatkjede()
            {
                //Arrange
                var produksjonssertifikat = SertifikatUtility.GetProduksjonsMottakerSertifikatOppslagstjenesten();

                //Act
                var sertifikatValidator = new Sertifikatkjedevalidator(SertifikatkjedeUtility.ProduksjonsSertifikater());
                var erGyldigResponssertifikat = sertifikatValidator.ErGyldigSertifikatkjede(produksjonssertifikat);

                //Assert
                Assert.IsTrue(erGyldigResponssertifikat);
            }

            [TestMethod]
            public void ErGyldigSertifikatkjedeOgKjedestatus()
            {
                //Arrange
                var produksjonssertifikat = SertifikatUtility.GetProduksjonsMottakerSertifikatOppslagstjenesten();
                X509ChainStatus[] kjedestatus;

                //Act
                var sertifikatValidator = new Sertifikatkjedevalidator(SertifikatkjedeUtility.ProduksjonsSertifikater());
                var erGyldigResponssertifikat = sertifikatValidator.ErGyldigSertifikatkjede(produksjonssertifikat, out kjedestatus);

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

                var erGyldigResponssertifikat = sertifikatValidator.ErGyldigSertifikatkjede(selvsignertSertifikat);

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
                var erGyldigResponssertifikat = sertifikatValidator.ErGyldigSertifikatkjede(selvsignertSertifikat, out kjedestatus);

                //Assert
                Assert.IsFalse(erGyldigResponssertifikat);
                Assert.IsTrue(kjedestatus.ElementAt(0).Status == X509ChainStatusFlags.UntrustedRoot);
            }
        }
    }
}