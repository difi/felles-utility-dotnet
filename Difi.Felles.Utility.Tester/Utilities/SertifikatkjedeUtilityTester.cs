using Difi.Felles.Utility.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Difi.Felles.Utility.Tester.Utilities
{
    [TestClass]
    public class SertifikatkjedeUtilityTester
    {
        [TestClass]
        public class TestsertifikaterMethod : SertifikatkjedeUtilityTester
        {
            [TestMethod]
            public void ReturnererFireSertifikaterMedThumbprint()
            {
                //Arrange
                var sertifikater = SertifikatkjedeUtility.FunksjoneltTestmiljøSertifikater();

                //Act

                //Assert
                foreach (var sertifikat in sertifikater)
                {
                    Assert.IsNotNull(sertifikat.Thumbprint);
                }
            }
        }

        [TestClass]
        public class ProduksjonssertifikaterMethod : SertifikatkjedeUtilityTester
        {
            [TestMethod]
            public void ReturnererFireSertifikaterMedThumbprint()
            {
                //Arrange
                var sertifikater = SertifikatkjedeUtility.ProduksjonsSertifikater();

                //Act

                //Assert
                foreach (var sertifikat in sertifikater)
                {
                    Assert.IsNotNull(sertifikat.Thumbprint);
                }
            }
        }
    }
}