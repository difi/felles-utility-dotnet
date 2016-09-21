using System.Linq;
using System.Security.Cryptography.X509Certificates;
using Difi.Felles.Utility.Exceptions;
using Difi.Felles.Utility.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Difi.Felles.Utility.Tester
{
    [TestClass]
    public class CertificateChainValidatorTests
    {
        [TestClass]
        public class ErGyldigSertifikatkjedeMethod : CertificateChainValidatorTests
        {
            [TestMethod]
            public void Gyldig_produksjonssertifikat_når_validerer_mot_produksjonskjede()
            {
                //Arrange
                var produksjonssertifikat = SertifikatUtility.GetProduksjonsMottakerSertifikatOppslagstjenesten();

                //Act
                var sertifikatValidator = new CertificateChainValidator(CertificateChainUtility.ProduksjonsSertifikater());
                var erGyldigResponssertifikat = sertifikatValidator.ErGyldigSertifikatkjede(produksjonssertifikat);

                //Assert
                Assert.IsTrue(erGyldigResponssertifikat);
            }

            [TestMethod]
            public void Gyldig_testsertifikat_når_validerer_mot_testkjede()
            {
                //Arrange
                var testSertifikat = SertifikatUtility.GetFunksjoneltTestmiljøMottakerSertifikatOppslagstjenesten();

                //Act
                var sertifikatValidator = new CertificateChainValidator(CertificateChainUtility.FunksjoneltTestmiljøSertifikater());
                var erGyldigResponssertifikat = sertifikatValidator.ErGyldigSertifikatkjede(testSertifikat);

                //Assert
                Assert.IsTrue(erGyldigResponssertifikat);
            }

            [TestMethod]
            public void Gyldig_produksjonssertifikat_og_kjedestatus_når_validerer_mot_produksjonskjede()
            {
                //Arrange
                var produksjonssertifikat = SertifikatUtility.GetProduksjonsMottakerSertifikatOppslagstjenesten();
                X509ChainStatus[] kjedestatus;

                //Act
                var sertifikatValidator = new CertificateChainValidator(CertificateChainUtility.ProduksjonsSertifikater());
                var erGyldigResponssertifikat = sertifikatValidator.ErGyldigSertifikatkjede(produksjonssertifikat, out kjedestatus);

                //Assert
                const int forventetAntallStatusElementer = 0;
                Assert.IsTrue(erGyldigResponssertifikat);
                Assert.AreEqual(forventetAntallStatusElementer, kjedestatus.Length);
            }

            [TestMethod]
            public void Gyldig_testsertifikat_og_kjedestatus_når_validerer_mot_testkjede()
            {
                //Arrange
                var testSertifikat = SertifikatUtility.GetFunksjoneltTestmiljøMottakerSertifikatOppslagstjenesten();
                X509ChainStatus[] kjedestatus;

                //Act
                var sertifikatValidator = new CertificateChainValidator(CertificateChainUtility.FunksjoneltTestmiljøSertifikater());
                var erGyldigResponssertifikat = sertifikatValidator.ErGyldigSertifikatkjede(testSertifikat, out kjedestatus);

                //Assert
                Assert.IsTrue(erGyldigResponssertifikat);
                Assert.IsTrue((kjedestatus.Length == 0) || (kjedestatus.ElementAt(0).Status == X509ChainStatusFlags.UntrustedRoot));
            }

            [TestMethod]
            public void Feiler_med_selvsignert_sertifikat_når_validerer_mot_produksjonskjede()
            {
                //Arrange
                var selvsignertSertifikat = SertifikatUtility.GetEnhetstesterSelvsignertSertifikat();

                //Act
                var sertifikatValidator = new CertificateChainValidator(CertificateChainUtility.ProduksjonsSertifikater());

                var erGyldigResponssertifikat = sertifikatValidator.ErGyldigSertifikatkjede(selvsignertSertifikat);

                //Assert
                Assert.IsFalse(erGyldigResponssertifikat);
            }

            [TestMethod]
            public void Feiler_med_selvsignert_sertifikat_når_validerer_mot_testkjede()
            {
                //Arrange
                var selvsignertSertifikat = SertifikatUtility.GetEnhetstesterSelvsignertSertifikat();

                //Act
                var sertifikatValidator = new CertificateChainValidator(CertificateChainUtility.FunksjoneltTestmiljøSertifikater());

                var erGyldigResponssertifikat = sertifikatValidator.ErGyldigSertifikatkjede(selvsignertSertifikat);

                //Assert
                Assert.IsFalse(erGyldigResponssertifikat);
            }

            [TestMethod]
            public void Feiler_med_selvsignert_sertifikat_og_kjedestatus_når_validerer_mot_produksjonskjede()
            {
                //Arrange
                var selvsignertSertifikat = SertifikatUtility.GetEnhetstesterSelvsignertSertifikat();

                //Act
                var sertifikatValidator = new CertificateChainValidator(CertificateChainUtility.ProduksjonsSertifikater());

                X509ChainStatus[] kjedestatus;
                var erGyldigResponssertifikat = sertifikatValidator.ErGyldigSertifikatkjede(selvsignertSertifikat, out kjedestatus);

                //Assert
                Assert.IsFalse(erGyldigResponssertifikat);
                Assert.IsTrue(kjedestatus.ElementAt(0).Status == X509ChainStatusFlags.UntrustedRoot);
            }

            [TestMethod]
            public void Feiler_med_selvsignert_sertifikat_og_kjedestatus_når_validerer_mot_testkjede()
            {
                var selvsignertSertifikat = SertifikatUtility.GetEnhetstesterSelvsignertSertifikat();

                //Act
                var sertifikatValidator = new CertificateChainValidator(CertificateChainUtility.FunksjoneltTestmiljøSertifikater());

                X509ChainStatus[] kjedestatus;
                var erGyldigResponssertifikat = sertifikatValidator.ErGyldigSertifikatkjede(selvsignertSertifikat, out kjedestatus);

                //Assert
                Assert.IsFalse(erGyldigResponssertifikat);
                Assert.IsTrue(kjedestatus.ElementAt(0).Status == X509ChainStatusFlags.UntrustedRoot);
            }

            [TestMethod]
            [ExpectedException(typeof(SecurityException))]
            public void Feiler_med_produksjonssertifikat_når_validerer_mot_testkjede()
            {
                //Arrange
                var produksjonssertifikat = SertifikatUtility.GetProduksjonsMottakerSertifikatOppslagstjenesten();

                //Act
                var sertifikatValidator = new CertificateChainValidator(CertificateChainUtility.FunksjoneltTestmiljøSertifikater());

                X509ChainStatus[] kjedestatus;
                var erGyldigResponssertifikat = sertifikatValidator.ErGyldigSertifikatkjede(produksjonssertifikat, out kjedestatus);
            }

            [TestMethod]
            [ExpectedException(typeof(SecurityException))]
            public void Feiler_med_testsertifikat_når_validerer_mot_produksjonskjede()
            {
                //Arrange
                var testsertifikat = SertifikatUtility.GetFunksjoneltTestmiljøMottakerSertifikatOppslagstjenesten();

                //Act
                var sertifikatValidator = new CertificateChainValidator(CertificateChainUtility.ProduksjonsSertifikater());

                X509ChainStatus[] kjedestatus;
                var erGyldigResponssertifikat = sertifikatValidator.ErGyldigSertifikatkjede(testsertifikat, out kjedestatus);
            }
        }
    }
}