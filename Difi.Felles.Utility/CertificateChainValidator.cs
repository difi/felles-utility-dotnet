using System;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using Difi.Felles.Utility.Exceptions;
using Difi.Felles.Utility.Extensions;

namespace Difi.Felles.Utility
{
    public class CertificateChainValidator
    {
        public CertificateChainValidator(X509Certificate2Collection sertifikatLager)
        {
            SertifikatLager = sertifikatLager;
        }

        public X509Certificate2Collection SertifikatLager { get; set; }

        /// <summary>
        /// Validerer sertifikatkjeden til sertifikatet. Gjør dette ved å validere mot <see cref="SertifikatLager"/> 
        /// </summary>
        /// <param name="sertifikat"></param>
        /// <returns></returns>
        public bool ErGyldigSertifikatkjede(X509Certificate2 sertifikat)
        {
            X509ChainStatus[] chainStatuses;
            return ErGyldigSertifikatkjede(sertifikat, out chainStatuses);
        }

        /// <summary>
        /// Validerer sertifikatkjeden til sertifikatet. Gjør dette ved å validere mot <see cref="SertifikatLager"/> 
        /// </summary>
        /// <param name="sertifikat"></param>
        /// <param name="detaljertFeilinformasjon">Status på kjeden etter validering hvis validering feilet.</param>
        /// <returns></returns>
        public bool ErGyldigSertifikatkjede(X509Certificate2 sertifikat, out string detaljertFeilinformasjon)
        {
            var result = ValidateCertificateChain(sertifikat);
            detaljertFeilinformasjon = result.Melding;

            return result.Type == SertifikatValideringType.Gyldig;
        }

        public SertifikatValideringsResultat ValidateCertificateChain(X509Certificate2 certificate)
        {
            var chain = BuildCertificateChain(certificate);

            var onlyUsingValidatorCertificatesResult = ValidateThatUsingOnlyValidatorCertificates(chain, certificate);

            return onlyUsingValidatorCertificatesResult.Type != SertifikatValideringType.Gyldig 
                ? onlyUsingValidatorCertificatesResult 
                : ValidateCertificateChain(certificate, chain);
        }

        /// <summary>
        /// Validerer sertifikatkjeden til sertifikatet. Gjør dette ved å validere mot <see cref="SertifikatLager"/> 
        /// </summary>
        /// <param name="sertifikat"></param>
        /// <param name="detaljertFeilinformasjon">Status på kjeden etter validering hvis validering feilet.</param>
        /// <returns></returns>
        public bool ErGyldigSertifikatkjede(X509Certificate2 sertifikat, out X509ChainStatus[] detaljertFeilinformasjon)
        {
            var chain = BuildCertificateChain(sertifikat);
            detaljertFeilinformasjon = chain.ChainStatus;

            var onlyUsingValidatorCertificatesResult = ValidateThatUsingOnlyValidatorCertificates(chain,sertifikat);
            if (onlyUsingValidatorCertificatesResult.Type != SertifikatValideringType.Gyldig)
            {
                return false;
            }

            return ValidateCertificateChain(sertifikat, chain).Type == SertifikatValideringType.Gyldig;
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

        private SertifikatValideringsResultat ValidateThatUsingOnlyValidatorCertificates(X509Chain chain, X509Certificate2 sertifikat)
        {
            foreach (var chainElement in chain.ChainElements)
            {
                var isCertificateToValidate = IsSameCertificate(chainElement.Certificate, sertifikat);
                if (isCertificateToValidate) { continue; }

                var isValidatorCertificate = SertifikatLager.Cast<X509Certificate2>().Any(lagerSertifikat => IsSameCertificate(chainElement.Certificate, lagerSertifikat));
                if (isValidatorCertificate) { continue; }

                var chainAsString = chain.ChainElements
                    .Cast<X509ChainElement>()
                    .Where(c => c.Certificate.Thumbprint != sertifikat.Thumbprint)
                    .Aggregate("",(result, curr) => GetCertificateInfo(result, curr.Certificate));

                var validatorCertificatesAsString = SertifikatLager
                    .Cast<X509Certificate2>()
                    .Aggregate("", GetCertificateInfo);

                return UsedExternalCertificatesResult(sertifikat, chainAsString, validatorCertificatesAsString);
            }

            return new SertifikatValideringsResultat(SertifikatValideringType.Gyldig, "");
        }

        private static SertifikatValideringsResultat UsedExternalCertificatesResult(X509Certificate2 sertifikat, string chainAsString, string validatorCertificatesAsString)
        {
            return new SertifikatValideringsResultat(SertifikatValideringType.UgyldigKjede, 
                $"Validering av sertifikat '{sertifikat.Info()}' feilet. {Environment.NewLine}" +
                $"Dette skjer fordi kjeden ble bygd med følgende sertifikater: {Environment.NewLine}{chainAsString}, " + 
                $"men kun følgende er godkjent for å bygge kjeden: {Environment.NewLine}{validatorCertificatesAsString}. Dette skjer som oftest om sertifikater blir hentet fra Certificate Store på Windows, " +
                "og det tillates ikke under validering. Det er kun gyldig å bygge en kjede med de sertifikatene sendt inn til validatoren.");
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

            policy.ExtraStore.AddRange(SertifikatLager);

            return policy;
        }

        private static SertifikatValideringsResultat ValidateCertificateChain(X509Certificate2 certificate, X509Chain chain)
        {
            const int requiredChainLength = 3;
            if (!HasExpectedLength(chain, requiredChainLength))
            {
                return IncorrectChainLengthResult(certificate, requiredChainLength, chain.ChainElements.Count);
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
                        : InvalidChainResult(certificate, detailedErrorInformation);
                default:
                    return InvalidChainResult(certificate, detailedErrorInformation);
            }
        }

        private static SertifikatValideringsResultat InvalidChainResult(X509Certificate2 certificate, params X509ChainStatus[] x509ChainStatuses)
        {
            return CreateSertifikatValideringsResultat(certificate, SertifikatValideringType.UgyldigKjede, $"har følgende feil i sertifikatkjeden: {GetPrettyChainErrorStatuses(x509ChainStatuses)}");
        }

        private static SertifikatValideringsResultat ValidResult(X509Certificate2 theCertificate)
        {
            return CreateSertifikatValideringsResultat(theCertificate, SertifikatValideringType.Gyldig, "er et gyldig sertifikat.");
        }

        private static SertifikatValideringsResultat IncorrectChainLengthResult(X509Certificate2 certificate2, int requiredChainLength, int actualChainLength)
        {
            return CreateSertifikatValideringsResultat(certificate2, SertifikatValideringType.UgyldigKjede, $"er ugyldig, fordi lengden på kjeden er {actualChainLength}, men skal være {requiredChainLength}. Dette skjer hvis sertifikatet er utstedt av en ukjent sertifikattilbyder eller er selvsignert.");
        }

        private static SertifikatValideringsResultat CreateSertifikatValideringsResultat(X509Certificate2 certificate, SertifikatValideringType sertifikatValideringType, string description)
        {
            return new SertifikatValideringsResultat(sertifikatValideringType, $"Sertifikat '{certificate.Info()}' {description}.");
        }

        private static string GetPrettyChainErrorStatuses(X509ChainStatus[] chainStatuses)
        {
            return chainStatuses.Aggregate("", (result, curr) => $"{curr.Status}: {curr.StatusInformation}");
        }

        private static bool HasExpectedLength(X509Chain chain, int chainLength)
        {
            return chain.ChainElements.Count == chainLength;
        }
    }
}