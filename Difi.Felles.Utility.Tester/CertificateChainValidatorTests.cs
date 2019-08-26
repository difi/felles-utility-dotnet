using Difi.Felles.Utility.Resources.Certificate;
using Difi.Felles.Utility.Utilities;
using Xunit;

namespace Difi.Felles.Utility.Tester
{
    public class CertificateChainValidatorTests
    {
        public class ValidateCertificateChain : CertificateChainValidatorTests
        {
            [Fact]
            public void Fails_with_self_signed_certificate()
            {
                //Arrange
                var selfSignedCertificate = CertificateResource.UnitTests.GetEnhetstesterSelvsignertSertifikat();
                var certificateChainValidator = new CertificateChainValidator(CertificateChainUtility.ProduksjonsSertifikater());

                //Act
                var result = certificateChainValidator.Validate(selfSignedCertificate);

                //Assert
                Assert.Equal(CertificateValidationType.InvalidChain, result.Type);
                Assert.Contains("sertifikatet er selvsignert", result.Message);
            }

            [Fact(Skip="Skipped because this is a windows specific task. This will be fixed with .NET Core 3+")]
            public void Fails_with_wrong_root_and_intermediate()
            {
                //Arrange
                var productionCertificate = CertificateResource.UnitTests.GetProduksjonsMottakerSertifikatOppslagstjenesten();

                //Act
                var certificateChainValidator = new CertificateChainValidator(CertificateChainUtility.FunksjoneltTestmiljøSertifikater());
                var result = certificateChainValidator.Validate(productionCertificate);

                //Assert
                Assert.Equal(CertificateValidationType.InvalidChain, result.Type);
                Assert.Contains("blir hentet fra Certificate Store på Windows", result.Message);
            }

            
            [Fact(Skip="Skipped because this is a windows specific task. This will be fixed with .NET Core 3+")]
            public void Valid_with_correct_root_and_intermediate()
            {
                //Arrange
                var productionCertificate = CertificateResource.UnitTests.GetProduksjonsMottakerSertifikatOppslagstjenesten();
                var certificateChainValidator = new CertificateChainValidator(CertificateChainUtility.ProduksjonsSertifikater());

                //Act
                var result = certificateChainValidator.Validate(productionCertificate);

                //Assert
                Assert.Equal(CertificateValidationType.Valid, result.Type);
                Assert.Contains("et gyldig sertifikat", result.Message);
            }
        }
    }
}