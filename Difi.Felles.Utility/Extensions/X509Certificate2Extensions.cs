using System.Security.Cryptography.X509Certificates;

namespace Difi.Felles.Utility.Extensions
{
    public static class X509Certificate2Extensions
    {
        public static string ToShortString(this X509Certificate2 certificate, string extraInfo = "")
        {
            return $"Sertifikat med Subject '{certificate.Subject}' og Thumbprint '{certificate.Thumbprint}' {extraInfo}";
        }
    }
}