using System.Security.Cryptography.X509Certificates;
using static Difi.Felles.Utility.Resources.Language.LanguageResource;
using static Difi.Felles.Utility.Resources.Language.LanguageResourceKey;

namespace Difi.Felles.Utility.Extensions
{
    public static class X509Certificate2Extensions
    {
        public static string ToShortString(this X509Certificate2 certificate, string extraInfo = "")
        {
            var shortStringWithPlaceholders = GetResource(CertificateShortDescription);
            return string.Format(shortStringWithPlaceholders, certificate.Subject, certificate.Thumbprint, extraInfo);
        }
    }
}