

using Xunit;

namespace Difi.Felles.Utility.Tester
{
    
    public class CertificateValidatorTests
    {
        public class ValidateCertificate : CertificateValidatorTests
        {
            [Fact]
            public void Returns_fail_with_null_certificate()
            {
                //Arrange

                //Act
                var result = CertificateValidator.ValidateCertificate(null, "123456789");

                //Assert
                Assert.Equal(SertifikatValideringType.UgyldigSertifikat, result.Type);
                Assert.NotNull(result.Melding);
            }

            [Fact]
            public void Returns_fail_if_not_issued_to_organization_number()
            {
                //Arrange
                var organizationNumber = "123456789";

                //Act
                var result = CertificateValidator.ValidateCertificate(SertifikatUtility.TestIntegrasjonssertifikat(), organizationNumber);

                //Assert
                Assert.Equal(SertifikatValideringType.UgyldigSertifikat, result.Type);
                Assert.NotNull(result.Melding);
            }

            [Fact]
            public void Returns_fail_if_not_activated()
            {
                //Arrange
                var sertifikatOrganisasjonsnummer = "988015814";

                //Act
                var result = CertificateValidator.ValidateCertificate(SertifikatUtility.NotActivatedTestCertificate(), sertifikatOrganisasjonsnummer);

                //Assert
                Assert.Equal(SertifikatValideringType.UgyldigSertifikat, result.Type);
                Assert.NotNull(result.Melding);
            }

            [Fact]
            public void Returns_fail_if_expired()
            {
                //Arrange
                var sertifikatOrganisasjonsnummer = "988015814";

                //Act
                var result = CertificateValidator.ValidateCertificate(SertifikatUtility.GetExpiredTestCertificate(), sertifikatOrganisasjonsnummer);

                //Assert
                Assert.Equal(SertifikatValideringType.UgyldigSertifikat, result.Type);
                Assert.NotNull(result.Melding);
            }

            [Fact]
            public void Returns_ok_if_valid()
            {
                //Arrange
                var sertifikatOrganisasjonsnummer = "984661185";

                //Act
                var result = CertificateValidator.ValidateCertificate(SertifikatUtility.GetPostenCertificate(), sertifikatOrganisasjonsnummer);

                //Assert
                Assert.Equal(SertifikatValideringType.Gyldig, result.Type);
                Assert.NotNull(result.Melding);
            }


        }

        public class IsValidCertificateMethod : CertificateValidatorTests
        {
            [Fact]
            public void Returns_false_with_null_certificate()
            {
                //Arrange
                var certificateOrganizationNumber = "123456789";

                //Act
                var isValid = CertificateValidator.IsValidCertificate(null, certificateOrganizationNumber);

                //Assert
                Assert.False(isValid);
            }

            [Fact]
            public void Returns_false_if_not_issued_to_organization_number()
            {
                //Arrange
                var sertifikatOrganisasjonsnummer = "123456789";

                //Act
                var isValid = CertificateValidator.IsValidCertificate(SertifikatUtility.TestIntegrasjonssertifikat(), sertifikatOrganisasjonsnummer);

                //Assert
                Assert.False(isValid);
            }

            [Fact]
            public void Returns_false_if_not_activated()
            {
                //Arrange
                var sertifikatOrganisasjonsnummer = "123456789";

                //Act
                var isValid = CertificateValidator.IsValidCertificate(SertifikatUtility.NotActivatedTestCertificate(), sertifikatOrganisasjonsnummer);

                //Assert
                Assert.False(isValid);
            }

            [Fact]
            public void Returns_false_if_expired()
            {
                //Arrange
                var sertifikatOrganisasjonsnummer = "123456789";

                //Act
                var isValid = CertificateValidator.IsValidCertificate(SertifikatUtility.GetExpiredTestCertificate(), sertifikatOrganisasjonsnummer);

                //Assert
                Assert.False(isValid);
            }

            [Fact]
            public void Returns_true_for_correct_certificate()
            {
                //Arrange
                var sertifikatOrganisasjonsnummer = "984661185";

                //Act
                var isValid = CertificateValidator.IsValidCertificate(SertifikatUtility.GetPostenCertificate(), sertifikatOrganisasjonsnummer);

                //Assert
                Assert.True(isValid);
            }
        }
    }
}