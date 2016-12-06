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

            [Fact]
            public void Get_resource_with_temporary_language()
            {
                LanguageResource.CurrentLanguage = Language.Norwegian;
                var resource = LanguageResource.GetResource(LanguageResourceKey.ToleratedPrefixListError, Language.English);

                Assert.True(resource.Contains("The 'PrefixList' attribute is invalid"));
                Assert.Equal(Language.Norwegian, LanguageResource.CurrentLanguage);
            }
        }
    }
}