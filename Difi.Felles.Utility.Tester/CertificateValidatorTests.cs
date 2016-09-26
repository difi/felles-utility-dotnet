using Difi.Felles.Utility.Utilities;
using Xunit;

namespace Difi.Felles.Utility.Tester
{
    
    public class CertificateValidatorTests
    {
        public class ValidateCertificateAndChainMethod : CertificateValidatorTests
        {
            [Fact]
            public void Returns_fail_if_certificate_error()
            {
                //Arrange
                var funksjoneltTestmiljøSertifikater = CertificateChainUtility.TestCertificates();

                //Act
                var result = CertificateValidator.ValidateCertificateAndChain(SertifikatUtility.GetExpiredSelfSignedTestCertificate(), "988015814", funksjoneltTestmiljøSertifikater);

                //Assert
                Assert.Equal(CertificateValidationType.InvalidCertificate, result.Type);
                Assert.NotNull(result.Message);
            }

            [Fact]
            public void Returns_fail_if_invalid_certificate_chain()
            {
                //Arrange
                var funksjoneltTestmiljøSertifikater = CertificateChainUtility.TestCertificates();

                //Act
                var result = CertificateValidator.ValidateCertificateAndChain(SertifikatUtility.GetValidSelfSignedTestCertificate(), "988015814", funksjoneltTestmiljøSertifikater);

                //Assert
                Assert.Equal(CertificateValidationType.InvalidChain, result.Type);
            }

            [Fact]
            public void Returns_ok_if_valid_certificate_and_chain()
            {
                //Arrange
                var funksjoneltTestmiljøSertifikater = CertificateChainUtility.TestCertificates();

                //Act
                var result = CertificateValidator.ValidateCertificateAndChain(SertifikatUtility.GetPostenCertificate(), "984661185", funksjoneltTestmiljøSertifikater);

                //Assert
                Assert.Equal(CertificateValidationType.Valid, result.Type);
            }
        }

        public class ValidateCertificateMethod : CertificateValidatorTests
        {
            [Fact]
            public void Returns_fail_with_null_certificate()
            {
                //Arrange
                const string organizationNumber = "123456789";

                //Act
                var result = CertificateValidator.ValidateCertificate(null, organizationNumber);

                //Assert
                Assert.Equal(CertificateValidationType.InvalidCertificate, result.Type);
                Assert.Contains("var null", result.Message);
            }

            [Fact]
            public void Returns_fail_if_not_issued_to_organization_number()
            {
                //Arrange
                const string certificateOrganizationNumber = "123456789";

                //Act
                var result = CertificateValidator.ValidateCertificate(SertifikatUtility.TestIntegrasjonssertifikat(), certificateOrganizationNumber);

                //Assert
                Assert.Equal(CertificateValidationType.InvalidCertificate, result.Type);
                Assert.Contains("ikke utstedt til organisasjonsnummer", result.Message);
            }

            [Fact]
            public void Returns_fail_if_not_activated()
            {
                //Arrange
                const string certificateOrganizationNumber = "988015814";

                //Act
                var result = CertificateValidator.ValidateCertificate(SertifikatUtility.NotActivatedSelfSignedTestCertificate(), certificateOrganizationNumber);

                //Assert
                Assert.Equal(CertificateValidationType.InvalidCertificate, result.Type);
                Assert.Contains("aktiveres ikke før", result.Message);
            }

            [Fact]
            public void Returns_fail_if_expired()
            {
                //Arrange
                const string certificateOrganizationNumber = "988015814";

                //Act
                var result = CertificateValidator.ValidateCertificate(SertifikatUtility.GetExpiredSelfSignedTestCertificate(), certificateOrganizationNumber);

                //Assert
                Assert.Equal(CertificateValidationType.InvalidCertificate, result.Type);
                Assert.Contains("gikk ut",result.Message);
            }

            [Fact]
            public void Returns_ok_if_valid()
            {
                //Arrange
                const string certificateOrganizationNumber = "984661185";

                //Act
                var result = CertificateValidator.ValidateCertificate(SertifikatUtility.GetPostenCertificate(), certificateOrganizationNumber);

                //Assert
                Assert.Equal(CertificateValidationType.Valid, result.Type);
                Assert.Contains("er et gyldig sertifikat", result.Message);
            }


        }

        public class IsValidCertificateMethod : CertificateValidatorTests
        {
            [Fact]
            public void Returns_false_with_null_certificate()
            {
                //Arrange
                const string certificateOrganizationNumber = "123456789";

                //Act
                var isValid = CertificateValidator.IsValidCertificate(null, certificateOrganizationNumber);

                //Assert
                Assert.False(isValid);
            }

            [Fact]
            public void Returns_false_if_not_issued_to_organization_number()
            {
                //Arrange
                var certificateOrganizationNumber = "123456789";

                //Act
                var isValid = CertificateValidator.IsValidCertificate(SertifikatUtility.TestIntegrasjonssertifikat(), certificateOrganizationNumber);

                //Assert
                Assert.False(isValid);
            }

            [Fact]
            public void Returns_false_if_not_activated()
            {
                //Arrange
                var certificateOrganizationNumber = "123456789";

                //Act
                var isValid = CertificateValidator.IsValidCertificate(SertifikatUtility.NotActivatedSelfSignedTestCertificate(), certificateOrganizationNumber);

                //Assert
                Assert.False(isValid);
            }

            [Fact]
            public void Returns_false_if_expired()
            {
                //Arrange
                var certificateOrganizationNumber = "123456789";

                //Act
                var isValid = CertificateValidator.IsValidCertificate(SertifikatUtility.GetExpiredSelfSignedTestCertificate(), certificateOrganizationNumber);

                //Assert
                Assert.False(isValid);
            }

            [Fact]
            public void Returns_true_for_correct_certificate()
            {
                //Arrange
                var certificateOrganizationNumber = "984661185";

                //Act
                var isValid = CertificateValidator.IsValidCertificate(SertifikatUtility.GetPostenCertificate(), certificateOrganizationNumber);

                //Assert
                Assert.True(isValid);
            }
        }
    }
}