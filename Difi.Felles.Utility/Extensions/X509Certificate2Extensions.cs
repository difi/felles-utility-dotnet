using System.Security.Cryptography.X509Certificates;
using Difi.Felles.Utility.Resources.Language;

namespace Difi.Felles.Utility.Extensions
{
    public static class X509Certificate2Extensions
    {
        public static string ToShortString(this X509Certificate2 certificate, string extraInfo = "")
        {
            var shortStringWithPlaceholders = LanguageResource.GetLanguageResource(LanguageResourceEnum.CertificateShortDescription);
            return string.Format(shortStringWithPlaceholders, certificate.Subject, certificate.Thumbprint, extraInfo);
        }
    }
}