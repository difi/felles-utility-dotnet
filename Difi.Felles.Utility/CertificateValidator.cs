using System;
using System.Security.Cryptography.X509Certificates;
using Difi.Felles.Utility.Extensions;

namespace Difi.Felles.Utility
{
    public class CertificateValidator
    {
        public static bool IsValidCertificate(X509Certificate2 certificate, string certificateOrganizationNumber)
        {
            return ValidateCertificate(certificate, certificateOrganizationNumber).Type == CertificateValidationType.Valid;
        }

        public static CertificateValidationResult ValidateCertificateAndChain(X509Certificate2 certificate, string certificateOrganizationNumber, X509Certificate2Collection allowedChainCertificates)
        {
            var sertifikatValideringsResultat = ValidateCertificate(certificate, certificateOrganizationNumber);

            if (sertifikatValideringsResultat.Type != CertificateValidationType.Valid)
            {
                return sertifikatValideringsResultat;
            }

            var certificateChainValidator = new CertificateChainValidator(allowedChainCertificates);
            return certificateChainValidator.Validate(certificate);
        }

        public static CertificateValidationResult ValidateCertificate(X509Certificate2 certificate, string certificateOrganizationNumber)
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

        private static CertificateValidationResult NoCertificateResult()
        {
            return new CertificateValidationResult(CertificateValidationType.InvalidCertificate, "Sertifikat var null! Sjekk at sertifikatet blir lastet korrekt.");
        }

        private static CertificateValidationResult NotIssuedToOrganizationResult(string certificateOrganizationNumber)
        {
            return new CertificateValidationResult(CertificateValidationType.InvalidCertificate,
                $"Sertifikatet er ikke utstedt til organisasjonsnummer '{certificateOrganizationNumber}'. Dette vil skje om sertifikatet er utstedt til en annen virksomhet " +
                "eller hvis det ikke er et virksomhetssertifikat. Virksomhetssertifikat kan skaffes fra Buypass eller Commfides.");
        }

        private static CertificateValidationResult NotActivatedResult(X509Certificate2 certificate)
        {
            return new CertificateValidationResult(
                CertificateValidationType.InvalidCertificate,
                certificate.ToShortString($"aktiveres ikke før {certificate.GetEffectiveDateString()}"));
        }

        private static CertificateValidationResult ExpiredResult(X509Certificate2 certificate)
        {
            return new CertificateValidationResult(
                CertificateValidationType.InvalidCertificate,
                certificate.ToShortString($"gikk ut {certificate.GetExpirationDateString()}."));
        }

        private static CertificateValidationResult ValidResult(X509Certificate2 certificate)
        {
            return new CertificateValidationResult(
                CertificateValidationType.Valid, 
                certificate.ToShortString("er et gyldig sertifikat."));
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