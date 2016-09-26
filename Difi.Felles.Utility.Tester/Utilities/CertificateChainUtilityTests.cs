using System.Diagnostics;
using Difi.Felles.Utility.Utilities;
using Xunit;

namespace Difi.Felles.Utility.Tester.Utilities
{
    public class CertificateChainUtilityTests
    {
        public class TestsertifikaterMethod : CertificateChainUtilityTests
        {
            [Fact]
            public void ReturnererFireSertifikaterMedThumbprint()
            {
                //Arrange
                var sertifikater = CertificateChainUtility.TestCertificates();

                //Act

                //Assert
                foreach (var sertifikat in sertifikater)
                {
                    Assert.NotNull(sertifikat.Thumbprint);
                }
            }
        }

        public class ProduksjonssertifikaterMethod : CertificateChainUtilityTests
        {
            [Fact]
            public void ReturnererFireSertifikaterMedThumbprint()
            {
                //Arrange
                var sertifikater = CertificateChainUtility.ProductionCertificates();

                //Act

                //Assert
                foreach (var sertifikat in sertifikater)
                {
                    Assert.NotNull(sertifikat.Thumbprint);
                }
            }
        }

        public class CertificateChainInfoTests : CertificateChainUtilityTests
        {
            [Fact]
            public void DebugMesages()
            {
                var i = 0;
                foreach (var certificate in CertificateChainUtility.TestCertificates())
                {
                    Trace.WriteLine($"{i++}: Issuer `{certificate.Issuer}`, thumbprint `{certificate.Thumbprint}`");
                }
            }
        }
    }
}