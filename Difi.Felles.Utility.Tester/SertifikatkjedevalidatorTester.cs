using System.Linq;
using System.Security.Cryptography.X509Certificates;
using ApiClientShared;
using Difi.Felles.Utility.Tester.Utilities;
using Difi.Felles.Utility.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Difi.Felles.Utility.Tester
{
    [TestClass]
    public class SertifikatkjedevalidatorTester
    {
        private static readonly ResourceUtility ResourceUtility = new ResourceUtility("Difi.Felles.Utility.Tester.Testdata.Sertifikater");

        [TestClass]
        public class ErGyldigSertifikatkjedeMethod : SertifikatkjedevalidatorTester
        {
            [TestMethod]
            public void ErGyldigSertifikatkjedeMedProduksjonssertifikater()
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
            public void ErGyldigSertifikatkjedeMedFunksjoneltTestmiljøsertifikater()
            {
                //Arrange
                var testSertifikat = SertifikatUtility.GetFunksjoneltTestmiljøMottakerSertifikatOppslagstjenesten();

                //Act
                var sertifikatValidator = new Sertifikatkjedevalidator(SertifikatkjedeUtility.FunksjoneltTestmiljøSertifikater());
                var erGyldigResponssertifikat = sertifikatValidator.ErGyldigSertifikatkjede(testSertifikat);

                //Assert
                Assert.IsTrue(erGyldigResponssertifikat);
            }


            [TestMethod]
            public void ErGyldigSertifikatkjedeOgKjedestatusMedProduksjonssertifikater()
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
            public void ErGyldigSertifikatkjedeOgKjedestatusMedFunksjoneltTestmiljøsertifikater()
            {
                //Arrange
                var testSertifikat = SertifikatUtility.GetFunksjoneltTestmiljøMottakerSertifikatOppslagstjenesten();
                X509ChainStatus[] kjedestatus;

                //Act
                var sertifikatValidator = new Sertifikatkjedevalidator(SertifikatkjedeUtility.FunksjoneltTestmiljøSertifikater());
                var erGyldigResponssertifikat = sertifikatValidator.ErGyldigSertifikatkjede(testSertifikat, out kjedestatus);

                //Assert
                Assert.IsTrue(erGyldigResponssertifikat);
                Assert.IsTrue(kjedestatus.Length == 0 || kjedestatus.ElementAt(0).Status == X509ChainStatusFlags.UntrustedRoot);
            }


            [TestMethod]
            public void FeilerMedSertifikatUtenGyldigKjedeMedProduksjonssertifikater()
            {
                //Arrange
                var selvsignertSertifikat = SertifikatUtility.GetEnhetstesterSelvsignertSertifikat();

                //Act
                var sertifikatValidator = new Sertifikatkjedevalidator(SertifikatkjedeUtility.ProduksjonsSertifikater());

                var erGyldigResponssertifikat = sertifikatValidator.ErGyldigSertifikatkjede(selvsignertSertifikat);

                //Assert
                Assert.IsFalse(erGyldigResponssertifikat);
            }

            [TestMethod]
            public void FeilerMedSertifikatUtenGyldigKjedeMedFunksjoneltTestmiljøsertifikater()
            {
                //Arrange
                var selvsignertSertifikat = SertifikatUtility.GetEnhetstesterSelvsignertSertifikat();

                //Act
                var sertifikatValidator = new Sertifikatkjedevalidator(SertifikatkjedeUtility.FunksjoneltTestmiljøSertifikater());

                var erGyldigResponssertifikat = sertifikatValidator.ErGyldigSertifikatkjede(selvsignertSertifikat);

                //Assert
                Assert.IsFalse(erGyldigResponssertifikat);
            }


            [TestMethod]
            public void FeilerMedSertifikatUtenGyldigKjedeReturnererKjedestatusMedProduksjonssertifikater()
            {
                //Arrange
                var selvsignertSertifikat = SertifikatUtility.GetEnhetstesterSelvsignertSertifikat();

                //Act
                var sertifikatValidator = new Sertifikatkjedevalidator(SertifikatkjedeUtility.ProduksjonsSertifikater());

                X509ChainStatus[] kjedestatus;
                var erGyldigResponssertifikat = sertifikatValidator.ErGyldigSertifikatkjede(selvsignertSertifikat, out kjedestatus);

                //Assert
                Assert.IsFalse(erGyldigResponssertifikat);
                Assert.IsTrue(kjedestatus.ElementAt(0).Status == X509ChainStatusFlags.UntrustedRoot);
            }

            [TestMethod]
            public void FeilerMedSertifikatUtenGyldigKjedeReturnererKjedestatusMedFunksjoneltTestmiljøsertifikater()
            {
                var selvsignertSertifikat = SertifikatUtility.GetEnhetstesterSelvsignertSertifikat();
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