using System;
using System.Security.Cryptography.X509Certificates;
using Difi.Felles.Utility.Extensions;
using static Difi.Felles.Utility.Resources.Language.LanguageResource;
using static Difi.Felles.Utility.Resources.Language.LanguageResourceKey;

namespace Difi.Felles.Utility
{
    public class CertificateValidator
    {
        [Obsolete("Use ValidateCertificate(X509Certificate, string) instead and use CertificateValidationResult to get result of validation")]
        public static bool IsValidCertificate(X509Certificate2 certificate, string certificateOrganizationNumber)
        {
            return ValidateCertificate(certificate, certificateOrganizationNumber).Type == CertificateValidationType.Valid;
        }

        /// <summary>
        ///     Validates the certificate and chain. Validates that certificate is
        ///     <list type="bullet">
        ///         <item> Not null </item>
        ///         <item> Issued to organization number </item>
        ///         <item> Is activated </item>
        ///         <item> Not Expired </item>
        ///         <item> Has a valid chain. A valid chain is one built with the allowed chain certificates.</item>
        ///     </list>
        /// </summary>
        /// <param name="certificate">The certificate to validate</param>
        /// <param name="certificateOrganizationNumber">The organization number which the certificate is issued to</param>
        /// <param name="allowedChainCertificates"></param>
        /// <returns></returns>
        public static CertificateValidationResult ValidateCertificateAndChain(X509Certificate2 certificate, string certificateOrganizationNumber, X509Certificate2Collection allowedChainCertificates)
        {
            return ValidateCertificateAndChainInternal(certificate, certificateOrganizationNumber, allowedChainCertificates);
        }


        /// <summary>
        ///     Validates the certificate and chain. Validates that certificate is
        ///     <list type="bullet">
        ///         <item> Not null </item>
        ///         <item> Is activated </item>
        ///         <item> Not Expired </item>
        ///         <item> Has a valid chain. A valid chain is one built with the allowed chain certificates.</item>
        ///     </list>
        /// </summary>
        /// <param name="certificate">The certificate to validate</param>
        /// <param name="certificateOrganizationNumber">The organization number which the certificate is issued to</param>
        /// <param name="allowedChainCertificates"></param>
        /// <returns></returns>
        public static CertificateValidationResult ValidateCertificateAndChain(X509Certificate2 certificate, X509Certificate2Collection allowedChainCertificates)
        {
            var certificateOrganizationNumber = string.Empty;

            return ValidateCertificateAndChainInternal(certificate, certificateOrganizationNumber, allowedChainCertificates);
        }

        internal static CertificateValidationResult ValidateCertificateAndChainInternal(X509Certificate2 certificate, string certificateOrganizationNumber, X509Certificate2Collection allowedChainCertificates)
        {
            var sertifikatValideringsResultat = ValidateCertificate(certificate, certificateOrganizationNumber);

            if (sertifikatValideringsResultat.Type != CertificateValidationType.Valid)
            {
                return sertifikatValideringsResultat;
            }

            var certificateChainValidator = new CertificateChainValidator(allowedChainCertificates);
            return certificateChainValidator.Validate(certificate);
        }

        /// <summary>
        ///     Validates the certificate itself. Validates that certificate is
        ///     <list type="bullet">
        ///         <item> Not null </item>
        ///         <item> Issued to organization number </item>
        ///         <item> Is activated </item>
        ///         <item> Not Expired </item>
        ///     </list>
        /// </summary>
        /// <remarks>
        ///     Does not validate the certificate chain. Please use <see cref="ValidateCertificateAndChain" /> for including
        ///     chain validation
        /// </remarks>
        /// <param name="certificate">The certificate to validate</param>
        /// <param name="certificateOrganizationNumber">The organization number the certificate is issued to</param>
        /// <returns>The result of the certificate validation</returns>
        public static CertificateValidationResult ValidateCertificate(X509Certificate2 certificate, string certificateOrganizationNumber)
        {
            if (certificate == null)
            {
                return NoCertificateResult();
            }

            if (string.IsNullOrWhiteSpace(certificateOrganizationNumber))
            {
                return ValidateCertificate(certificate);
            }

            if (!IsIssuedToOrganizationNumber(certificate, certificateOrganizationNumber))
            {
                return NotIssuedToOrganizationResult(certificate, certificateOrganizationNumber);
            }

            return ValidateCertificate(certificate);
        }

        /// <summary>
        ///     Validates the certificate itself. Validates that certificate is
        ///     <list type="bullet">
        ///         <item> Not null </item>
        ///         <item> Is activated </item>
        ///         <item> Not Expired </item>
        ///     </list>
        /// </summary>
        /// <remarks>
        ///     Does not validate the certificate chain. Please use <see cref="ValidateCertificateAndChain" /> for including
        ///     chain validation
        /// </remarks>
        /// <param name="certificate"></param>
        /// <returns></returns>
        public static CertificateValidationResult ValidateCertificate(X509Certificate2 certificate)
        {
            if (certificate == null)
            {
                return NoCertificateResult();
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