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

        public static SertifikatValideringsResultat ValidateCertificate(X509Certificate2 certificate, string certificateOrganizationNumber)
        {
            if (certificate == null)
            {
                return new SertifikatValideringsResultat(SertifikatValideringType.UgyldigSertifikat, $"Sertifikat var {null}!");
            }

            if (!IsIssuedToOrganizationNumber(certificate, certificateOrganizationNumber))
            {
                return new SertifikatValideringsResultat(SertifikatValideringType.UgyldigSertifikat, 
                    $"Sertifikatet er ikke utstedt til organisasjonsnummer '{certificateOrganizationNumber}'. Dette vil skje om sertifikatet er utstedt til en annen virksomhet " +
                    $"eller hvis det ikke er et virksomhetssertifikat. Virksomhetssertifikat kan skaffes fra Buypass eller Commfides.");
            }

            if (!IsActivatedCertificate(certificate))
            {
                return new SertifikatValideringsResultat(SertifikatValideringType.UgyldigSertifikat, $"Sertifikat '{GetCertificateInfo(certificate)}' aktiveres ikke før {certificate.GetEffectiveDateString()}.");
            }

            if (IsExpiredCertificate(certificate))
            {
                return new SertifikatValideringsResultat(SertifikatValideringType.UgyldigSertifikat, $"Sertifikat '{GetCertificateInfo(certificate)}' gikk ut {certificate.GetExpirationDateString()}.");
            }

            return new SertifikatValideringsResultat(SertifikatValideringType.Gyldig, $"Sertifikat '{GetCertificateInfo(certificate)}' er et gyldig sertifikat.");
        }

        private static string GetCertificateInfo(X509Certificate2 certificate)
        {
            return $"Subject: {certificate.Subject}, Thumbprint: {certificate.Thumbprint}"; //TODO: Lag som extensionmetode
        }

        private static bool IsIssuedToOrganizationNumber(X509Certificate certificate, string certificateOrganizationNumber)
        {
            return certificate.Subject.Contains($"SERIALNUMBER={certificateOrganizationNumber}") || certificate.Subject.Contains($"CN={certificateOrganizationNumber}");
        }

        private static bool IsActiveCertificate(X509Certificate certificate)
        {
            return IsActivatedCertificate(certificate) && !IsExpiredCertificate(certificate);
        }

        private static bool IsActivatedCertificate(X509Certificate certificate)
        {
            return DateTime.Now > DateTime.Parse(certificate.GetEffectiveDateString());
        }

        private static bool IsExpiredCertificate(X509Certificate certificate)
        {
            return DateTime.Now > DateTime.Parse(certificate.GetExpirationDateString());
        }
    }
}