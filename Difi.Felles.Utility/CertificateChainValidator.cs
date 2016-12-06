using System;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using Difi.Felles.Utility.Extensions;
using static Difi.Felles.Utility.Resources.Language.LanguageResource;
using static Difi.Felles.Utility.Resources.Language.LanguageResourceKey;

namespace Difi.Felles.Utility
{
    public class CertificateChainValidator
    {
        public CertificateChainValidator(X509Certificate2Collection certificateStore)
        {
            CertificateStore = certificateStore;
        }

        public X509Certificate2Collection CertificateStore { get; set; }

        [Obsolete("Use CertificateStore instead.")]
        public X509Certificate2Collection SertifikatLager => CertificateStore;

        /// <summary>
        ///     Validerer sertifikatkjeden til sertifikatet. Gjør dette ved å validere mot <see cref="SertifikatLager" />
        /// </summary>
        /// <param name="certificate"></param>
        /// <returns></returns>
        [Obsolete("Use IsValidChain instead.")]
        public bool ErGyldigSertifikatkjede(X509Certificate2 certificate)
        {
            return IsValidChain(certificate);
        }

        /// <summary>
        ///     Validerer sertifikatkjeden til sertifikatet. Gjør dette ved å validere mot <see cref="SertifikatLager" />
        /// </summary>
        /// <param name="certificate"></param>
        /// <returns></returns>
        public bool IsValidChain(X509Certificate2 certificate)
        {
            return Validate(certificate).Type == CertificateValidationType.Valid;
        }

        /// <summary>
        ///     Validerer sertifikatkjeden til sertifikatet. Gjør dette ved å validere mot <see cref="SertifikatLager" />
        /// </summary>
        /// <param name="certificate"></param>
        /// <param name="detailedErrorInformation">Status på kjeden etter validering hvis validering feilet.</param>
        /// <returns></returns>
        [Obsolete("Use IsValidChain instead.")]
        public bool ErGyldigSertifikatkjede(X509Certificate2 certificate, out string detailedErrorInformation)
        {
            return IsValidChain(certificate, out detailedErrorInformation);
        }

        /// <summary>
        ///     Validerer sertifikatkjeden til sertifikatet. Gjør dette ved å validere mot <see cref="SertifikatLager" />
        /// </summary>
        /// <param name="certificate"></param>
        /// <param name="detailedErrorInformation">Status på kjeden etter validering hvis validering feilet.</param>
        /// <returns></returns>
        public bool IsValidChain(X509Certificate2 certificate, out string detailedErrorInformation)
        {
            var result = Validate(certificate);
            detailedErrorInformation = result.Message;

            return result.Type == CertificateValidationType.Valid;
        }

        public CertificateValidationResult Validate(X509Certificate2 certificate)
        {
            var chain = BuildCertificateChain(certificate);

            var onlyUsingValidatorCertificatesResult = ValidateThatUsingOnlyValidatorCertificates(chain, certificate);

            return onlyUsingValidatorCertificatesResult.Type != CertificateValidationType.Valid
                ? onlyUsingValidatorCertificatesResult
                : Validate(certificate, chain);
        }

        /// <summary>
        ///     Validerer sertifikatkjeden til sertifikatet. Gjør dette ved å validere mot <see cref="SertifikatLager" />
        /// </summary>
        /// <param name="certificate"></param>
        /// <param name="detailedErrorInformation">Status på kjeden etter validering hvis validering feilet.</param>
        /// <returns></returns>
        [Obsolete("Use other overloads for validation, as this overload exposes the error of untrusted root certificate. We tolerate this error because it occurs when loading a root certificate from file, which is always done here. We trust the certificates as they are preloaded in library.")]
        public bool ErGyldigSertifikatkjede(X509Certificate2 certificate, out X509ChainStatus[] detailedErrorInformation)
        {
            var chain = BuildCertificateChain(certificate);
            detailedErrorInformation = chain.ChainStatus;

            var onlyUsingValidatorCertificatesResult = ValidateThatUsingOnlyValidatorCertificates(chain, certificate);
            if (onlyUsingValidatorCertificatesResult.Type != CertificateValidationType.Valid)
            {
                return false;
            }

            return Validate(certificate, chain).Type == CertificateValidationType.Valid;
        }

        private X509Chain BuildCertificateChain(X509Certificate2 sertifikat)
        {
            var chain = new X509Chain
            {
                ChainPolicy = ChainPolicy()
            };
            chain.Build(sertifikat);
            return chain;
        }

        private CertificateValidationResult ValidateThatUsingOnlyValidatorCertificates(X509Chain chain, X509Certificate2 certificate)
        {
            foreach (var chainElement in chain.ChainElements)
            {
                var isCertificateToValidate = IsSameCertificate(chainElement.Certificate, certificate);
                if (isCertificateToValidate)
                {
                    continue;
                }

                var isValidatorCertificate = CertificateStore.Cast<X509Certificate2>().Any(lagerSertifikat => IsSameCertificate(chainElement.Certificate, lagerSertifikat));
                if (isValidatorCertificate)
                {
                    continue;
                }

                var chainAsString = chain.ChainElements
                    .Cast<X509ChainElement>()
                    .Where(c => c.Certificate.Thumbprint != certificate.Thumbprint)
                    .Aggregate("", (result, curr) => GetCertificateInfo(result, curr.Certificate));

                var validatorCertificatesAsString = CertificateStore
                    .Cast<X509Certificate2>()
                    .Aggregate("", GetCertificateInfo);

                return UsedExternalCertificatesResult(certificate, chainAsString, validatorCertificatesAsString);
            }

            return ValidResult(certificate);
        }

        private static CertificateValidationResult UsedExternalCertificatesResult(X509Certificate2 certificate, string chainAsString, string validatorCertificatesAsString)
        {
            var externalCertificatesUsedMessage =
                string.Format(
                    GetResource(CertificateUsedExternalResult),
                    certificate.ToShortString(), chainAsString, validatorCertificatesAsString);

            return new CertificateValidationResult(CertificateValidationType.InvalidChain, externalCertificatesUsedMessage);
        }

        private static bool IsSameCertificate(X509Certificate2 certificate1, X509Certificate2 certificate2)
        {
            return certificate2.Thumbprint == certificate1.Thumbprint;
        }

        private static string GetCertificateInfo(string current, X509Certificate2 certificate)
        {
            return current + $"'{certificate.Subject}' {Environment.NewLine}";
        }

        public X509ChainPolicy ChainPolicy()
        {
            var policy = new X509ChainPolicy
            {
                RevocationMode = X509RevocationMode.NoCheck
            };

            policy.ExtraStore.AddRange(CertificateStore);

            return policy;
        }

        private static CertificateValidationResult Validate(X509Certificate2 certificate, X509Chain chain)
        {
            if (IsSelfSignedCertificate(chain))
            {
                return SelfSignedErrorResult(certificate);
            }

            var detailedErrorInformation = chain.ChainStatus;
            switch (detailedErrorInformation.Length)
            {
                case 0:
                    return ValidResult(certificate);
                case 1:
                    var chainError = detailedErrorInformation.ElementAt(0).Status;
                    return chainError == X509ChainStatusFlags.UntrustedRoot
                        ? ValidResult(certificate)
                        : InvalidChainResult(certificate, detailedErrorInformation); //We tolerate this 'UntrustedRoot' because it occurs when loading a root certificate from file, which is always done here. We trust the certificates as they are preloaded in library.
                default:
                    return InvalidChainResult(certificate, detailedErrorInformation);
            }
        }

        private static CertificateValidationResult InvalidChainResult(X509Certificate2 certificate, params X509ChainStatus[] x509ChainStatuses)
        {
            var invalidChainResult = string.Format(GetResource(CertificateInvalidChainResult), GetPrettyChainErrorStatuses(x509ChainStatuses));
            return new CertificateValidationResult(CertificateValidationType.InvalidChain, certificate.ToShortString(invalidChainResult));
        }

        private static CertificateValidationResult ValidResult(X509Certificate2 certificate)
        {
            var validChainResult = GetResource(CertificateValidResult);
            return new CertificateValidationResult(CertificateValidationType.Valid, certificate.ToShortString(validChainResult));
        }

        private static CertificateValidationResult SelfSignedErrorResult(X509Certificate2 certificate)
        {
            var selfSignedErrorResult = GetResource(CertificateSelfSignedErrorResult);
            return new CertificateValidationResult(CertificateValidationType.InvalidChain, certificate.ToShortString(selfSignedErrorResult));
        }

        private static string GetPrettyChainErrorStatuses(X509ChainStatus[] chainStatuses)
        {
            return chainStatuses.Aggregate("", (result, curr) => $"{curr.Status}: {curr.StatusInformation}");
        }

        private static bool IsSelfSignedCertificate(X509Chain chain)
        {
            const int selfSignedChainLength = 1;
            return chain.ChainElements.Count == selfSignedChainLength;
        }
    }
}