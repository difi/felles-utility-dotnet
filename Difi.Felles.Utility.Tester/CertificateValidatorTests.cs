using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Difi.Felles.Utility.Tester
{
    [TestClass]
    public class CertificateValidatorTests
    {
        [TestClass]
        public class IsValidServerSertifikatMethod : CertificateValidatorTests
        {
            [TestMethod]
            public void ReturnsFalseWithNullCertificate()
            {
                //Arrange
                var certificateOrganizationNumber = "123456789";

                //Act
                var isValid = CertificateValidator.IsValidCertificate(null, certificateOrganizationNumber);

                //Assert
                Assert.IsFalse(isValid);
            }

            [TestMethod]
            public void ReturnsFalseIfNotIssuedToServerOrganizationNumber()
            {
                //Arrange
                var sertifikatOrganisasjonsnummer = "123456789";

                //Act
                var isValid = CertificateValidator.IsValidCertificate(SertifikatUtility.TestIntegrasjonssertifikat(), sertifikatOrganisasjonsnummer);

                //Assert
                Assert.IsFalse(isValid);
            }

            [TestMethod]
            public void ReturnsFalseIfNotActivated()
            {
                //Arrange
                var sertifikatOrganisasjonsnummer = "123456789";

                //Act
                var isValid = CertificateValidator.IsValidCertificate(SertifikatUtility.NotActivatedTestCertificate(), sertifikatOrganisasjonsnummer);

                //Assert
                Assert.IsFalse(isValid);
            }

            [TestMethod]
            public void ReturnsFalseIfExpired()
            {
                //Arrange
                var sertifikatOrganisasjonsnummer = "123456789";

                //Act
                var isValid = CertificateValidator.IsValidCertificate(SertifikatUtility.GetExpiredTestCertificate(), sertifikatOrganisasjonsnummer);

                //Assert
                Assert.IsFalse(isValid);
            }

            [TestMethod]
            public void ReturnsTrueForCorrectCertificate()
            {
                //Arrange
                var sertifikatOrganisasjonsnummer = "984661185";

                //Act
                var isValid = CertificateValidator.IsValidCertificate(SertifikatUtility.GetPostenCertificate(), sertifikatOrganisasjonsnummer);

                //Assert
                Assert.IsTrue(isValid);
            }
        }
    }
}