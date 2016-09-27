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
                var sertifikater = CertificateChainUtility.FunksjoneltTestmiljøSertifikater();

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
                var sertifikater = CertificateChainUtility.ProduksjonsSertifikater();

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
                foreach (var certificate in CertificateChainUtility.FunksjoneltTestmiljøSertifikater())
                {
                    Trace.WriteLine($"{i++}: Issuer `{certificate.Issuer}`, thumbprint `{certificate.Thumbprint}`");
                }
            }
        }
    }
}