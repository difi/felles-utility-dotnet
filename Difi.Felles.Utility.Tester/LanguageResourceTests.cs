using Difi.Felles.Utility.Extensions;
using Difi.Felles.Utility.Resources.Certificate;
using Difi.Felles.Utility.Resources.Language;
using Xunit;

namespace Difi.Felles.Utility.Tester
{
    public class LanguageResourceTests
    {
        public class GetResourceMethod
        {
            [Fact]
            public void Get_resource_with_placeholders()
            {
                LanguageResource.CurrentLanguage = Language.Norwegian;
                var certificate = CertificateResource.UnitTests.GetPostenCertificate();

                var certDescr = certificate.ToShortString("Extrainfo");

                Assert.True(certDescr.Contains(certificate.Subject));
            }
        }
    }
}
