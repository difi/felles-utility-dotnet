using Difi.Felles.Utility.Resources.Certificate;
using Difi.Felles.Utility.Utilities;
using Xunit;

namespace Difi.Felles.Utility.Tester
{
    public class CertificateValidatorTests
    {
        public class ValidateCertificateAndChainInternalMethod : CertificateValidatorTests
        {
            [Fact]
            public void Returns_fail_if_certificate_error()
            {
                //Arrange
                var funksjoneltTestmiljøSertifikater = CertificateChainUtility.FunksjoneltTestmiljøSertifikater();

                //Act
                var result = CertificateValidator.ValidateCertificateAndChainInternal(CertificateResource.UnitTests.GetExpiredSelfSignedTestCertificate(), "988015814", funksjoneltTestmiljøSertifikater);

                //Assert
                Assert.Equal(CertificateValidationType.InvalidCertificate, result.Type);
            }

            [Fact]
            public void Returns_fail_if_self_signed_certificate()
            {
                //Arrange
                var funksjoneltTestmiljøSertifikater = CertificateChainUtility.FunksjoneltTestmiljøSertifikater();

                //Act
                var result = CertificateValidator.ValidateCertificateAndChainInternal(CertificateResource.UnitTests.GetValidSelfSignedTestCertificate(), "988015814", funksjoneltTestmiljøSertifikater);

                //Assert
                Assert.Equal(CertificateValidationType.InvalidChain, result.Type);
            }

            [Fact(Skip="Skipped because the certificate in question is expired")]
            public void Returns_ok_if_valid_certificate_and_chain()
            {
                //Arrange
                var funksjoneltTestmiljøSertifikater = CertificateChainUtility.FunksjoneltTestmiljøSertifikater();

                //Act
                var result = CertificateValidator.ValidateCertificateAndChainInternal(CertificateResource.UnitTests.GetPostenCertificate(), "984661185", funksjoneltTestmiljøSertifikater);

                //Assert
                Assert.Equal(CertificateValidationType.Valid, result.Type);
            }
        }

        public class ValidateCertificateMethodWithOrganizationNumber : CertificateValidatorTests
        {
            /// <summary>
            ///     To ensure we are calling the overload doing checking for expiration, activation and not null.
            /// </summary>
            [Fact]
            public void Calls_validate_certificate_overload_with_no_organization_number()
            {
                //Arrange
                const string organizationNumber = "988015814";

                //Act
                var result = CertificateValidator.ValidateCertificate(CertificateResource.UnitTests.GetExpiredSelfSignedTestCertificate(), organizationNumber);

                //Assert
                Assert.Equal(CertificateValidationType.InvalidCertificate, result.Type);
            }

            [Fact(Skip="Skipped because the certificate in question is expired")]
            public void Ignores_issued_to_organization_if_no_organization_number()
            {
                //Act
                var result = CertificateValidator.ValidateCertificate(CertificateResource.UnitTests.GetPostenCertificate(), string.Empty);

                //Assert
                Assert.Equal(CertificateValidationType.Valid, result.Type);
            }

            [Fact]
            public void Returns_fail_if_not_issued_to_organization_number()
            {
                //Arrange
                const string certificateOrganizationNumber = "123456789";

                //Act
                var result = CertificateValidator.ValidateCertificate(CertificateResource.UnitTests.TestIntegrasjonssertifikat(), certificateOrganizationNumber);

                //Assert
                Assert.Equal(CertificateValidationType.InvalidCertificate, result.Type);
            }

            [Fact(Skip="Skipped because the certificate in question is expired")]
            public void Returns_ok_if_valid()
            {
                //Arrange
                const string certificateOrganizationNumber = "984661185";

                //Act
                var result = CertificateValidator.ValidateCertificate(CertificateResource.UnitTests.GetPostenCertificate(), certificateOrganizationNumber);

                //Assert
                Assert.Equal(CertificateValidationType.Valid, result.Type);
            }
        }

        public class ValidateCertificateMethodWithNoOrganizationNumber : CertificateValidatorTests
        {
            [Fact]
            public void Returns_fail_if_expired()
            {
                //Arrange

                //Act
                var result = CertificateValidator.ValidateCertificate(CertificateResource.UnitTests.GetExpiredSelfSignedTestCertificate());

                //Assert
                Assert.Equal(CertificateValidationType.InvalidCertificate, result.Type);
            }

            [Fact]
            public void Returns_fail_if_not_activated()
            {
                //Arrange

                //Act
                var result = CertificateValidator.ValidateCertificate(CertificateResource.UnitTests.NotActivatedSelfSignedTestCertificate());

                //Assert
                Assert.Equal(CertificateValidationType.InvalidCertificate, result.Type);
            }

            [Fact]
            public void Returns_fail_with_null_certificate()
            {
                //Arrange

                //Act
                var result = CertificateValidator.ValidateCertificate(null);

                //Assert
                Assert.Equal(CertificateValidationType.InvalidCertificate, result.Type);
            }

            [Fact(Skip="Skipped because the certificate in question is expired")]
            public void Returns_ok_if_valid()
            {
                //Arrange

                //Act
                var result = CertificateValidator.ValidateCertificate(CertificateResource.UnitTests.GetPostenCertificate());

                //Assert
                Assert.Equal(CertificateValidationType.Valid, result.Type);
            }

        }

        public class IsValidCertificateMethod : CertificateValidatorTests
        {
            [Fact]
            public void Returns_false_if_expired()
            {
                //Arrange

                //Act
                var result = CertificateValidator.ValidateCertificate(CertificateResource.UnitTests.GetExpiredSelfSignedTestCertificate());

                //Assert
                Assert.Equal(CertificateValidationType.InvalidCertificate, result.Type);
            }

            [Fact]
            public void Returns_false_if_not_activated()
            {
                //Arrange

                //Act
                var result = CertificateValidator.ValidateCertificate(CertificateResource.UnitTests.NotActivatedSelfSignedTestCertificate());

                //Assert
                Assert.Equal(CertificateValidationType.InvalidCertificate, result.Type);
            }

            [Fact]
            public void Returns_false_if_not_issued_to_organization_number()
            {
                //Arrange
                const string certificateOrganizationNumber = "123456789";
                
                //Act
                var result = CertificateValidator.ValidateCertificate(CertificateResource.UnitTests.TestIntegrasjonssertifikat(), certificateOrganizationNumber);

                //Assert
                Assert.Equal(CertificateValidationType.InvalidCertificate, result.Type);
            }

            [Fact]
            public void Returns_false_with_null_certificate()
            {
                //Arrange

                //Act
                var result = CertificateValidator.ValidateCertificate(null);

                //Assert
                Assert.Equal(CertificateValidationType.InvalidCertificate, result.Type);
            }

            [Fact(Skip="Skipped because the certificate in question is expired")]
            public void Returns_true_for_correct_certificate()
            {
                //Arrange

                //Act
                var result = CertificateValidator.ValidateCertificate(CertificateResource.UnitTests.GetPostenCertificate());

                //Assert
                Assert.Equal(CertificateValidationType.Valid, result.Type);
            }
        }
    }
}
