

using Xunit;

namespace Difi.Felles.Utility.Tester
{
    
    public class CertificateValidatorTests
    {
        
        public class IsValidServerSertifikatMethod : CertificateValidatorTests
        {
            [Fact]
            public void ReturnsFalseWithNullCertificate()
            {
                //Arrange
                var certificateOrganizationNumber = "123456789";

                //Act
                var isValid = CertificateValidator.IsValidCertificate(null, certificateOrganizationNumber);

                //Assert
                Assert.False(isValid);
            }

            [Fact]
            public void ReturnsFalseIfNotIssuedToServerOrganizationNumber()
            {
                //Arrange
                var sertifikatOrganisasjonsnummer = "123456789";

                //Act
                var isValid = CertificateValidator.IsValidCertificate(SertifikatUtility.TestIntegrasjonssertifikat(), sertifikatOrganisasjonsnummer);

                //Assert
                Assert.False(isValid);
            }

            [Fact]
            public void ReturnsFalseIfNotActivated()
            {
                //Arrange
                var sertifikatOrganisasjonsnummer = "123456789";

                //Act
                var isValid = CertificateValidator.IsValidCertificate(SertifikatUtility.NotActivatedTestCertificate(), sertifikatOrganisasjonsnummer);

                //Assert
                Assert.False(isValid);
            }

            [Fact]
            public void ReturnsFalseIfExpired()
            {
                //Arrange
                var sertifikatOrganisasjonsnummer = "123456789";

                //Act
                var isValid = CertificateValidator.IsValidCertificate(SertifikatUtility.GetExpiredTestCertificate(), sertifikatOrganisasjonsnummer);

                //Assert
                Assert.False(isValid);
            }

            [Fact]
            public void ReturnsTrueForCorrectCertificate()
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