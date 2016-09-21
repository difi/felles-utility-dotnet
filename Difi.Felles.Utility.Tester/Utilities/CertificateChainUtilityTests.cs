using System.Diagnostics;
using Difi.Felles.Utility.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Difi.Felles.Utility.Tester.Utilities
{
    [TestClass]
    public class CertificateChainUtilityTests
    {
        [TestClass]
        public class TestsertifikaterMethod : CertificateChainUtilityTests
        {
            [TestMethod]
            public void ReturnererFireSertifikaterMedThumbprint()
            {
                //Arrange
                var sertifikater = CertificateChainUtility.FunksjoneltTestmiljøSertifikater();

                //Act

                //Assert
                foreach (var sertifikat in sertifikater)
                {
                    Assert.IsNotNull(sertifikat.Thumbprint);
                }
            }
        }

        [TestClass]
        public class ProduksjonssertifikaterMethod : CertificateChainUtilityTests
        {
            [TestMethod]
            public void ReturnererFireSertifikaterMedThumbprint()
            {
                //Arrange
                var sertifikater = CertificateChainUtility.ProduksjonsSertifikater();

                //Act

                //Assert
                foreach (var sertifikat in sertifikater)
                {
                    Assert.IsNotNull(sertifikat.Thumbprint);
                }
            }
        }

        [TestClass]
        public class CertificateChainInfoTests : CertificateChainUtilityTests
        {
            [TestMethod]
            public void DebugMesages()
            {
                int i = 0;
                foreach (var certificate in CertificateChainUtility.FunksjoneltTestmiljøSertifikater())
                {
                    Trace.WriteLine($"{i++}: Issuer `{certificate.Issuer}`, thumbprint `{certificate.Thumbprint}`");
                }
            }

        }
    }
}