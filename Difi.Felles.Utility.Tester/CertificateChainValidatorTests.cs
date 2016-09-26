using Difi.Felles.Utility.Utilities;
using Xunit;
using Assert = Xunit.Assert;

namespace Difi.Felles.Utility.Tester
{

    public class CertificateChainValidatorTests
    {
        
        public class ValidateCertificateChain : CertificateChainValidatorTests
        {
            [Fact]
            public void  Valid_with_correct_root_and_intermediate()
            {
                //Arrange
                var productionCertificate = SertifikatUtility.GetProduksjonsMottakerSertifikatOppslagstjenesten();
                var certificateChainValidator = new CertificateChainValidator(CertificateChainUtility.ProduksjonsSertifikater());

                //Act
                var result = certificateChainValidator.Validate(productionCertificate);

                //Assert
                Assert.Equal(CertificateValidationType.Valid, result.Type);
                Assert.Contains("et gyldig sertifikat", result.Message);
            }

            [Fact]
            public void Fails_with_wrong_root_and_intermediate()
            {
                //Arrange
                var productionCertificate = SertifikatUtility.GetProduksjonsMottakerSertifikatOppslagstjenesten();

                //Act
                var certificateChainValidator = new CertificateChainValidator(CertificateChainUtility.FunksjoneltTestmiljøSertifikater());
                var result = certificateChainValidator.Validate(productionCertificate);

                //Assert
                Assert.Equal(CertificateValidationType.InvalidChain, result.Type);
                Assert.Contains("blir hentet fra Certificate Store på Windows", result.Message);

            }

            [Fact]
            public void Fails_with_self_signed_certificate()
            {
                //Arrange
                var selfSignedCertificate = SertifikatUtility.GetEnhetstesterSelvsignertSertifikat();
                var certificateChainValidator = new CertificateChainValidator(CertificateChainUtility.ProduksjonsSertifikater());

                //Act
                var result = certificateChainValidator.Validate(selfSignedCertificate);

                //Assert
                Assert.Equal(CertificateValidationType.InvalidChain, result.Type);
                Assert.Contains("sertifikatet er selvsignert", result.Message);

            }
        }
    }
}