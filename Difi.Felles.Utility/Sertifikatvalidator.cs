using System;
using System.Security.Cryptography.X509Certificates;

namespace Difi.Felles.Utility
{
    public class Sertifikatvalidator
    {
        public static bool ErGyldigSertifikat(X509Certificate2 certificate, string certificateOrganizationNumber)
        {
            return certificate != null && ErUtstedtTilOrganisasjonsnummer(certificate, certificateOrganizationNumber) && ErAktivtSertifikat(certificate);
        }

        private static bool ErUtstedtTilOrganisasjonsnummer(X509Certificate certificate, string certificateOrganizationNumber)
        {
            return certificate.Subject.Contains($"SERIALNUMBER={certificateOrganizationNumber}") || certificate.Subject.Contains($"CN={certificateOrganizationNumber}");
        }

        private static bool ErAktivtSertifikat(X509Certificate certificate)
        {
            return DateTime.Now > DateTime.Parse(certificate.GetEffectiveDateString()) && DateTime.Now < DateTime.Parse(certificate.GetExpirationDateString());
        }
    }
}