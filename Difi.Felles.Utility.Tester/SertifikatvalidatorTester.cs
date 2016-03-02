using Difi.Felles.Utility.Tester.Utilities;
using Difi.Felles.Utility.Utilities;
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
                var sertifikatkjedevalidator = new Sertifikatkjedevalidator(SertifikatkjedeUtility.FunksjoneltTestmiljøSertifikater());
                var certificateOrganizationNumber = "123456789";

                //Act
                var isValid = Sertifikatvalidator.IsValidServerCertificate(sertifikatkjedevalidator, null, certificateOrganizationNumber);

                //Assert
                Assert.IsFalse(isValid);
            }

            [TestMethod]
            public void ReturnsFalseIfNotIssuedToServerOrganizationNumber()
            {
                //Arrange
                var sertifikatkjedevalidator = new Sertifikatkjedevalidator(SertifikatkjedeUtility.FunksjoneltTestmiljøSertifikater());
                var sertifikatOrganisasjonsnummer = "123456789";

                //Act
                var isValid = Sertifikatvalidator.IsValidServerCertificate(sertifikatkjedevalidator, SertifikatUtility.TestIntegrasjonssertifikat(), sertifikatOrganisasjonsnummer);

                //Assert
                Assert.IsFalse(isValid);
            }

            [TestMethod]
            public void ReturnsFalseIfNotActivated()
            {
                //Arrange
                var sertifikatkjedevalidator = new Sertifikatkjedevalidator(SertifikatkjedeUtility.FunksjoneltTestmiljøSertifikater());
                var sertifikatOrganisasjonsnummer = "123456789";

                //Act
                var isValid = Sertifikatvalidator.IsValidServerCertificate(sertifikatkjedevalidator, SertifikatUtility.NotActivatedTestCertificate(), sertifikatOrganisasjonsnummer);

                //Assert
                Assert.IsFalse(isValid);
            }

            [TestMethod]
            public void ReturnsFalseIfExpired()
            {
                //Arrange
                var sertifikatkjedevalidator = new Sertifikatkjedevalidator(SertifikatkjedeUtility.FunksjoneltTestmiljøSertifikater());
                var sertifikatOrganisasjonsnummer = "123456789";

                //Act
                var isValid = Sertifikatvalidator.IsValidServerCertificate(sertifikatkjedevalidator, SertifikatUtility.GetExpiredTestCertificate(), sertifikatOrganisasjonsnummer);

                //Assert
                Assert.IsFalse(isValid);
            }

            [TestMethod]
            public void ReturnsTrueForCorrectCertificate()
            {
                //Arrange
                var sertifikatkjedevalidator = new Sertifikatkjedevalidator(SertifikatkjedeUtility.FunksjoneltTestmiljøSertifikater());
                var sertifikatOrganisasjonsnummer = "984661185";

                //Act
                var isValid = Sertifikatvalidator.IsValidServerCertificate(sertifikatkjedevalidator, SertifikatUtility.GetPostenCertificate(), sertifikatOrganisasjonsnummer);

                //Assert
                Assert.IsTrue(isValid);
            }
        }
    }
}