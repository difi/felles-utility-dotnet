using System;
using System.Security.Cryptography.X509Certificates;

namespace Difi.Felles.Utility
{
    public class Sertifikatvalidator
    {
        public static bool IsValidServerCertificate(Sertifikatkjedevalidator sertifikatkjedevalidator, X509Certificate2 certificate, string certificateOrganizationNumber)
        {
            return certificate != null && IsIssuedToServerOrganizationNumber(certificate, certificateOrganizationNumber) && IsActiveCertificate(certificate);
        }

        private static bool IsIssuedToServerOrganizationNumber(X509Certificate certificate, string certificateOrganizationNumber)
        {
            return certificate.Subject.Contains($"SERIALNUMBER={certificateOrganizationNumber}") || certificate.Subject.Contains($"CN={certificateOrganizationNumber}");
        }

        private static bool IsActiveCertificate(X509Certificate certificate)
        {
            return DateTime.Now > DateTime.Parse(certificate.GetEffectiveDateString()) && DateTime.Now < DateTime.Parse(certificate.GetExpirationDateString());
        }
    }
}
