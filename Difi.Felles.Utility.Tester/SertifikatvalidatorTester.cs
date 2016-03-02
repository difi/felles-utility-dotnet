using Difi.Felles.Utility.Tester.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Difi.Felles.Utility.Tester
{
    [TestClass]
    public class SertifikatvalidatorTester
    {
        [TestClass]
        public class IsValidServerSertifikatMethod : SertifikatvalidatorTester
        {
            [TestMethod]
            public void ReturnsFalseWithNullCertificate()
            {
                //Arrange
                var certificateOrganizationNumber = "123456789";

                //Act
                var isValid = Sertifikatvalidator.ErGyldigSertifikat(null, certificateOrganizationNumber);

                //Assert
                Assert.IsFalse(isValid);
            }

            [TestMethod]
            public void ReturnsFalseIfNotIssuedToServerOrganizationNumber()
            {
                //Arrange
                var sertifikatOrganisasjonsnummer = "123456789";

                //Act
                var isValid = Sertifikatvalidator.ErGyldigSertifikat(SertifikatUtility.TestIntegrasjonssertifikat(), sertifikatOrganisasjonsnummer);

                //Assert
                Assert.IsFalse(isValid);
            }

            [TestMethod]
            public void ReturnsFalseIfNotActivated()
            {
                //Arrange
                var sertifikatOrganisasjonsnummer = "123456789";

                //Act
                var isValid = Sertifikatvalidator.ErGyldigSertifikat(SertifikatUtility.NotActivatedTestCertificate(), sertifikatOrganisasjonsnummer);

                //Assert
                Assert.IsFalse(isValid);
            }

            [TestMethod]
            public void ReturnsFalseIfExpired()
            {
                //Arrange
                var sertifikatOrganisasjonsnummer = "123456789";

                //Act
                var isValid = Sertifikatvalidator.ErGyldigSertifikat(SertifikatUtility.GetExpiredTestCertificate(), sertifikatOrganisasjonsnummer);

                //Assert
                Assert.IsFalse(isValid);
            }

            [TestMethod]
            public void ReturnsTrueForCorrectCertificate()
            {
                //Arrange
                var sertifikatOrganisasjonsnummer = "984661185";

                //Act
                var isValid = Sertifikatvalidator.ErGyldigSertifikat(SertifikatUtility.GetPostenCertificate(), sertifikatOrganisasjonsnummer);

                //Assert
                Assert.IsTrue(isValid);
            }
        }
    }
}