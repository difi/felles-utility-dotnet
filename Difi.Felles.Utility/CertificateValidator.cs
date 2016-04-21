using System;
using System.Security.Cryptography.X509Certificates;

namespace Difi.Felles.Utility
{
    public class CertificateValidator
    {
        public static bool IsValidCertificate(X509Certificate2 certificate, string certificateOrganizationNumber)
        {
            return certificate != null && IsIssuedToOrganizationNumber(certificate, certificateOrganizationNumber) && IsActiveCertificate(certificate);
        }

        private static bool IsIssuedToOrganizationNumber(X509Certificate certificate, string certificateOrganizationNumber)
        {
            return certificate.Subject.Contains($"SERIALNUMBER={certificateOrganizationNumber}") || certificate.Subject.Contains($"CN={certificateOrganizationNumber}");
        }

        private static bool IsActiveCertificate(X509Certificate certificate)
        {
            return DateTime.Now > DateTime.Parse(certificate.GetEffectiveDateString()) && DateTime.Now < DateTime.Parse(certificate.GetExpirationDateString());
        }
    }
}