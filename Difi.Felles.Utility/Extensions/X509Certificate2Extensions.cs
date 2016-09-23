using System.Security.Cryptography.X509Certificates;

namespace Difi.Felles.Utility.Extensions
{
    public static class X509Certificate2Extensions
    {
        public static string Info(this X509Certificate2 certificate)
        {
            return $"Subject: {certificate.Subject}, Thumbprint: {certificate.Thumbprint}"; 
        }
    }
}
