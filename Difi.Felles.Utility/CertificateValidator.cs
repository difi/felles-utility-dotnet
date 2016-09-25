using System;
using System.Security.Cryptography.X509Certificates;
using Difi.Felles.Utility.Extensions;

namespace Difi.Felles.Utility
{
    public class CertificateValidator
    {
        public static bool IsValidCertificate(X509Certificate2 certificate, string certificateOrganizationNumber)
        {
            return ValidateCertificate(certificate, certificateOrganizationNumber).Type == SertifikatValideringType.Gyldig;
        }

        public static SertifikatValideringsResultat ValidateCertificate(X509Certificate2 certificate, string certificateOrganizationNumber)
        {
            if (certificate == null)
            {
                return NoCertificateResult();
            }

            if (!IsIssuedToOrganizationNumber(certificate, certificateOrganizationNumber))
            {
                return NotIssuedToOrganizationResult(certificateOrganizationNumber);
            }

            if (!IsActivatedCertificate(certificate))
            {
                return NotActivatedResult(certificate);
            }

            if (IsExpiredCertificate(certificate))
            {
                return ExpiredResult(certificate);
            }

            return ValidResult(certificate);
        }

        private static SertifikatValideringsResultat NoCertificateResult()
        {
            return new SertifikatValideringsResultat(SertifikatValideringType.UgyldigSertifikat, $"Sertifikat var {null}!");
        }

        private static SertifikatValideringsResultat NotIssuedToOrganizationResult(string certificateOrganizationNumber)
        {
            return new SertifikatValideringsResultat(SertifikatValideringType.UgyldigSertifikat,
                $"Sertifikatet er ikke utstedt til organisasjonsnummer '{certificateOrganizationNumber}'. Dette vil skje om sertifikatet er utstedt til en annen virksomhet " +
                "eller hvis det ikke er et virksomhetssertifikat. Virksomhetssertifikat kan skaffes fra Buypass eller Commfides.");
        }

        private static SertifikatValideringsResultat NotActivatedResult(X509Certificate2 certificate)
        {
            return CreateSertifikatValideringsResultat(certificate,
                SertifikatValideringType.UgyldigSertifikat,
                $"aktiveres ikke før {certificate.GetEffectiveDateString()}");
        }

        private static SertifikatValideringsResultat ExpiredResult(X509Certificate2 certificate)
        {
            return CreateSertifikatValideringsResultat(certificate,
                SertifikatValideringType.UgyldigSertifikat,
                $"gikk ut {certificate.GetExpirationDateString()}.");
        }

        private static SertifikatValideringsResultat ValidResult(X509Certificate2 certificate)
        {
            return CreateSertifikatValideringsResultat(certificate, SertifikatValideringType.Gyldig, "er et gyldig sertifikat.");
        }

        private static SertifikatValideringsResultat CreateSertifikatValideringsResultat(X509Certificate2 certificate, SertifikatValideringType sertifikatValideringType, string description)
        {
            return new SertifikatValideringsResultat(
                sertifikatValideringType, 
                $"Sertifikat '{certificate.Info()}' {description}.");
        }

        private static bool IsIssuedToOrganizationNumber(X509Certificate certificate, string certificateOrganizationNumber)
        {
            return certificate.Subject.Contains($"SERIALNUMBER={certificateOrganizationNumber}") || certificate.Subject.Contains($"CN={certificateOrganizationNumber}");
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