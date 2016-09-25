using System.Linq;
using System.Security.Cryptography.X509Certificates;
using Difi.Felles.Utility.Exceptions;
using Difi.Felles.Utility.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Xunit;
using Assert = Xunit.Assert;

namespace Difi.Felles.Utility.Tester
{
    
    public class CertificateChainValidatorTests
    {
        
        public class ErGyldigSertifikatkjedeMethod : CertificateChainValidatorTests
        {
            [Fact]
            public void Gyldig_produksjonssertifikat_når_validerer_mot_produksjonskjede()
            {
                //Arrange
                var produksjonssertifikat = SertifikatUtility.GetProduksjonsMottakerSertifikatOppslagstjenesten();

                //Act
                var sertifikatValidator = new CertificateChainValidator(CertificateChainUtility.ProduksjonsSertifikater());
                var erGyldigResponssertifikat = sertifikatValidator.ErGyldigSertifikatkjede(produksjonssertifikat);

                //Assert
                Assert.True(erGyldigResponssertifikat);
            }

            [Fact]
            public void Gyldig_testsertifikat_når_validerer_mot_testkjede()
            {
                //Arrange
                var testSertifikat = SertifikatUtility.GetFunksjoneltTestmiljøMottakerSertifikatOppslagstjenesten();

                //Act
                var sertifikatValidator = new CertificateChainValidator(CertificateChainUtility.FunksjoneltTestmiljøSertifikater());
                var erGyldigResponssertifikat = sertifikatValidator.ErGyldigSertifikatkjede(testSertifikat);

                //Assert
                Assert.True(erGyldigResponssertifikat);
            }

            [Fact]
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
                Assert.True(erGyldigResponssertifikat);
                Assert.Equal(forventetAntallStatusElementer, kjedestatus.Length);
            }

            [Fact]
            public void Gyldig_testsertifikat_og_kjedestatus_når_validerer_mot_testkjede()
            {
                //Arrange
                var testSertifikat = SertifikatUtility.GetFunksjoneltTestmiljøMottakerSertifikatOppslagstjenesten();
                X509ChainStatus[] kjedestatus;

                //Act
                var sertifikatValidator = new CertificateChainValidator(CertificateChainUtility.FunksjoneltTestmiljøSertifikater());
                var erGyldigResponssertifikat = sertifikatValidator.ErGyldigSertifikatkjede(testSertifikat, out kjedestatus);

                //Assert
                Assert.True(erGyldigResponssertifikat);
                Assert.True((kjedestatus.Length == 0) || (kjedestatus.ElementAt(0).Status == X509ChainStatusFlags.UntrustedRoot));
            }

            [Fact]
            public void Feiler_med_selvsignert_sertifikat_når_validerer_mot_produksjonskjede()
            {
                //Arrange
                var selvsignertSertifikat = SertifikatUtility.GetEnhetstesterSelvsignertSertifikat();

                //Act
                var sertifikatValidator = new CertificateChainValidator(CertificateChainUtility.ProduksjonsSertifikater());

                var erGyldigResponssertifikat = sertifikatValidator.ErGyldigSertifikatkjede(selvsignertSertifikat);

                //Assert
                Assert.False(erGyldigResponssertifikat);
            }

            [Fact]
            public void Feiler_med_selvsignert_sertifikat_når_validerer_mot_testkjede()
            {
                //Arrange
                var selvsignertSertifikat = SertifikatUtility.GetEnhetstesterSelvsignertSertifikat();

                //Act
                var sertifikatValidator = new CertificateChainValidator(CertificateChainUtility.FunksjoneltTestmiljøSertifikater());

                var erGyldigResponssertifikat = sertifikatValidator.ErGyldigSertifikatkjede(selvsignertSertifikat);

                //Assert
                Assert.False(erGyldigResponssertifikat);
            }

            [Fact]
            public void Feiler_med_selvsignert_sertifikat_og_kjedestatus_når_validerer_mot_produksjonskjede()
            {
                //Arrange
                var selvsignertSertifikat = SertifikatUtility.GetEnhetstesterSelvsignertSertifikat();

                //Act
                var sertifikatValidator = new CertificateChainValidator(CertificateChainUtility.ProduksjonsSertifikater());

                X509ChainStatus[] kjedestatus;
                var erGyldigResponssertifikat = sertifikatValidator.ErGyldigSertifikatkjede(selvsignertSertifikat, out kjedestatus);

                //Assert
                Assert.False(erGyldigResponssertifikat);
                Assert.True(kjedestatus.ElementAt(0).Status == X509ChainStatusFlags.UntrustedRoot);
            }

            [Fact]
            public void Feiler_med_selvsignert_sertifikat_og_kjedestatus_når_validerer_mot_testkjede()
            {
                var selvsignertSertifikat = SertifikatUtility.GetEnhetstesterSelvsignertSertifikat();

                //Act
                var sertifikatValidator = new CertificateChainValidator(CertificateChainUtility.FunksjoneltTestmiljøSertifikater());

                X509ChainStatus[] kjedestatus;
                var erGyldigResponssertifikat = sertifikatValidator.ErGyldigSertifikatkjede(selvsignertSertifikat, out kjedestatus);

                //Assert
                Assert.False(erGyldigResponssertifikat);
                Assert.True(kjedestatus.ElementAt(0).Status == X509ChainStatusFlags.UntrustedRoot);
            }

            [Fact]
            public void Feiler_med_produksjonssertifikat_når_validerer_mot_testkjede()
            {
                //Arrange
                var produksjonssertifikat = SertifikatUtility.GetProduksjonsMottakerSertifikatOppslagstjenesten();

                //Act
                var sertifikatValidator = new CertificateChainValidator(CertificateChainUtility.FunksjoneltTestmiljøSertifikater());
                var erGyldigSertifikatkjede = sertifikatValidator.ErGyldigSertifikatkjede(produksjonssertifikat);

                Assert.False(erGyldigSertifikatkjede);
            }

            [Fact]
            public void Feiler_med_testsertifikat_når_validerer_mot_produksjonskjede()
            {
                //Arrange
                var testsertifikat = SertifikatUtility.GetFunksjoneltTestmiljøMottakerSertifikatOppslagstjenesten();

                //Act
                var sertifikatValidator = new CertificateChainValidator(CertificateChainUtility.ProduksjonsSertifikater());
                var erGyldigSertifikatkjede = sertifikatValidator.ErGyldigSertifikatkjede(testsertifikat);

                Assert.False(erGyldigSertifikatkjede);
            }
        }
    }
}