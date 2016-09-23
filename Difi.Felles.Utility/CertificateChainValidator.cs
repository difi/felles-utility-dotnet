using System;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using Difi.Felles.Utility.Exceptions;

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
        /// <exception cref="CertificateChainValidationException">Kastes hvis det prøves å gjøre validering mot andre sertifikater enn de i <see cref="SertifikatLager"/>.</exception>
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
        /// <exception cref="CertificateChainValidationException">Kastes hvis det prøves å gjøre validering mot andre sertifikater enn de i <see cref="SertifikatLager"/>.</exception>
        /// <returns></returns>
        public bool ErGyldigSertifikatkjede(X509Certificate2 sertifikat, out string detaljertFeilinformasjon)
        {
            X509ChainStatus[] chainStatuses;
            var erGyldigSertifikatkjede = ErGyldigSertifikatkjede(sertifikat, out chainStatuses);
            detaljertFeilinformasjon = chainStatuses.Aggregate("", (result, curr) => $"{curr.Status}: {curr.StatusInformation}");

            return erGyldigSertifikatkjede;
        }
        
        /// <summary>
        /// Validerer sertifikatkjeden til sertifikatet. Gjør dette ved å validere mot <see cref="SertifikatLager"/> 
        /// </summary>
        /// <param name="sertifikat"></param>
        /// <param name="detaljertFeilinformasjon">Status på kjeden etter validering hvis validering feilet.</param>
        /// <exception cref="CertificateChainValidationException">Kastes hvis det prøves å gjøre validering mot andre sertifikater enn de i <see cref="SertifikatLager"/>.</exception>
        /// <returns></returns>
        public bool ErGyldigSertifikatkjede(X509Certificate2 sertifikat, out X509ChainStatus[] detaljertFeilinformasjon)
        {
            var chain = BuildCertificateChain(sertifikat);
            detaljertFeilinformasjon = chain.ChainStatus;

            ValidateThatUsingOnlyValidatorCertificatesOrThrow(chain,sertifikat);

            return IsValidCertificateChain(chain);
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

        private void ValidateThatUsingOnlyValidatorCertificatesOrThrow(X509Chain chain, X509Certificate2 sertifikat)
        {
            foreach (var chainElement in chain.ChainElements)
            {
                var isCertificateToValidate = IsSameCertificate(chainElement.Certificate, sertifikat);
                if (isCertificateToValidate) { continue; }

                var isValidatorCertificate = SertifikatLager.Cast<X509Certificate2>().Any(lagerSertifikat => IsSameCertificate(chainElement.Certificate, lagerSertifikat));
                if (isValidatorCertificate) { continue; }

                var chainAsString = chain.ChainElements.Cast<X509ChainElement>().Aggregate("",(result, curr) => GetCertificateInfo(result, curr.Certificate));
                var validatorCertificatesAsString = SertifikatLager.Cast<X509Certificate2>().Aggregate("", GetCertificateInfo);

                throw new CertificateChainValidationException($"Validering av sertifikat '{sertifikat.Subject}' (thumbprint '{sertifikat.Thumbprint}') feilet. Dette skjer fordi kjeden ble bygd " +
                                                              $"med følgende sertifikater {chainAsString}, men kun følgende er godkjent for å bygge kjeden: {validatorCertificatesAsString}. Dette skjer som oftest " +
                                                              "om sertifikater blir hentet fra Certificate Store på Windows, og det tillates ikke under validering. Det er kun gyldig å bygge en " +
                                                              "kjede med de sertifikatene sendt inn til validatoren.");
            }
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

        private static bool IsValidCertificateChain(X509Chain chain)
        {
            if (!HasExpectedLength(chain, 3)) return false;

            var detailedErrorInformation = chain.ChainStatus;
            switch (detailedErrorInformation.Length)
            {
                case 0:
                    return true;
                case 1:
                    var isUntrustedRootStatus = detailedErrorInformation.ElementAt(0).Status == X509ChainStatusFlags.UntrustedRoot;
                    return isUntrustedRootStatus;
                default:
                    return false;
            }
        }

        private static bool HasExpectedLength(X509Chain chain, int chainLength)
        {
            return chain.ChainElements.Count == chainLength;
        }
    }
}