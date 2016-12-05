using System;
using System.Security.Cryptography.X509Certificates;
using Difi.Felles.Utility.Extensions;
using static Difi.Felles.Utility.Resources.Language.LanguageResource;
using static Difi.Felles.Utility.Resources.Language.LanguageResourceEnum;

namespace Difi.Felles.Utility
{
    public class CertificateValidator
    {
        [Obsolete("Use ValidateCertificate(X509Certificate, string) instead and use CertificateValidationResult to get result of validation")]
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
                return NotIssuedToOrganizationResult(certificate, certificateOrganizationNumber);
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
            var nullCertificateResult = GetResource(CertificateIsNull);
            return new CertificateValidationResult(CertificateValidationType.InvalidCertificate, nullCertificateResult);
        }

        private static CertificateValidationResult NotIssuedToOrganizationResult(X509Certificate2 certificate, string certificateOrganizationNumber)
        {
            var notIssuedToOrganizationResult = string.Format(GetResource(CertificateNotIssuedToOrganization), certificateOrganizationNumber);
            return new CertificateValidationResult(
                CertificateValidationType.InvalidCertificate, 
                certificate.ToShortString(notIssuedToOrganizationResult));
        }

        private static CertificateValidationResult NotActivatedResult(X509Certificate2 certificate)
        {
            var notActivatedResult = string.Format(GetResource(CertificateNotActivatedResult), certificate.GetEffectiveDateString());
            return new CertificateValidationResult(
                CertificateValidationType.InvalidCertificate,
                certificate.ToShortString(notActivatedResult));
        }

        private static CertificateValidationResult ExpiredResult(X509Certificate2 certificate)
        {
            var expiredResult = string.Format(GetResource(CertificateExpiredResult), certificate.GetExpirationDateString());
            return new CertificateValidationResult(
                CertificateValidationType.InvalidCertificate,
                certificate.ToShortString(expiredResult));
        }

        private static CertificateValidationResult ValidResult(X509Certificate2 certificate)
        {
            var validResult = GetResource(CertificateValidResult);
            return new CertificateValidationResult(
                CertificateValidationType.Valid,
                certificate.ToShortString(validResult));
        }

        private static bool IsIssuedToOrganizationNumber(X509Certificate certificate, string certificateOrganizationNumber)
        {
            return certificate.Subject.Contains($"SERIALNUMBER={certificateOrganizationNumber}") || certificate.Subject.Contains($"CN={certificateOrganizationNumber}");
        }

        private static bool IsActivatedCertificate(X509Certificate2 certificate)
        {
            return DateTime.Now > certificate.NotBefore;
        }

        private static bool IsExpiredCertificate(X509Certificate2 certificate)
        {
            return DateTime.Now > certificate.NotAfter;
        }
    }
}