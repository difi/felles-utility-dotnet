using System;
using System.Linq;
using System.Net.Mime;
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
            X509ChainStatus[] kjedestatus;
            return ErGyldigSertifikatkjede(sertifikat, out kjedestatus);
        }

        /// <summary>
        /// Validerer sertifikatkjeden til sertifikatet. Gjør dette ved å validere mot <see cref="SertifikatLager"/> 
        /// </summary>
        /// <param name="sertifikat"></param>
        /// <param name="kjedestatus">Status på kjeden etter validering. </param>
        /// <exception cref="CertificateChainValidationException">Kastes hvis det prøves å gjøre validering mot andre sertifikater enn de i <see cref="SertifikatLager"/>.</exception>
        /// <returns></returns>
        public bool ErGyldigSertifikatkjede(X509Certificate2 sertifikat, out X509ChainStatus[] kjedestatus)
        {
            var chain = ByggSertifikatKjede(sertifikat);
            kjedestatus = chain.ChainStatus;

            ValiderAtBrukerKunSertifikatLagerEllerKastException(chain,sertifikat);

            return ErGyldigSertifikatKjede(chain);
        }

        private X509Chain ByggSertifikatKjede(X509Certificate2 sertifikat)
        {
            var chain = new X509Chain
            {
                ChainPolicy = ChainPolicy()
            };
            chain.Build(sertifikat);
            return chain;
        }

        private void ValiderAtBrukerKunSertifikatLagerEllerKastException(X509Chain chain, X509Certificate2 sertifikat)
        {
            foreach (var chainElement in chain.ChainElements)
            {
                var erSertifikatSomValideres = ErSammeSertifikat(chainElement.Certificate, sertifikat);
                if (erSertifikatSomValideres) { continue; }

                var erISertifikatlager = SertifikatLager.Cast<X509Certificate2>().Any(lagerSertifikat => ErSammeSertifikat(chainElement.Certificate, lagerSertifikat));
                if (erISertifikatlager) { continue; }

                var chainAsString = chain.ChainElements.Cast<X509ChainElement>().Aggregate("",(result, curr) => GetCertificateInfo(result, curr.Certificate));
                var lagerAsString = SertifikatLager.Cast<X509Certificate2>().Aggregate("", GetCertificateInfo);

                throw new CertificateChainValidationException($"Validering av sertifikat '{sertifikat.Subject}' (thumbprint '{sertifikat.Thumbprint}') feilet. Dette skjer fordi kjeden ble bygd " +
                                                              $"med følgende sertifikater {chainAsString}, men kun følgende er godkjent for å bygge kjeden: {lagerAsString}. Dette skjer som oftest " +
                                                              "om sertifikater blir hentet fra Certificate Store på Windows, og det tillates ikke under validering. Det er kun gyldig å bygge en " +
                                                              "kjede med de sertifikatene sendt inn til validatoren.");
            }
        }

        private static bool ErSammeSertifikat(X509Certificate2 sertifikat1, X509Certificate2 sertifikat2)
        {
            return sertifikat2.Thumbprint == sertifikat1.Thumbprint;
        }

        private static string GetCertificateInfo(string current, X509Certificate2 chainchain)
        {
            return current + $"'{chainchain.Subject}' {Environment.NewLine}";
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

        private static bool ErGyldigSertifikatKjede(X509Chain chain)
        {
            if (!HarForventetLengde(chain, 3)) return false;

            var detailedErrorInformation = chain.ChainStatus;
            switch (detailedErrorInformation.Length)
            {
                case 0:
                    return true;
                case 1:
                    var erUntrustedRootStatus = detailedErrorInformation.ElementAt(0).Status == X509ChainStatusFlags.UntrustedRoot;
                    return erUntrustedRootStatus;
                default:
                    return false;
            }
        }

        private static bool HarForventetLengde(X509Chain chain, int kjedelengde)
        {
            return chain.ChainElements.Count == kjedelengde;
        }
    }
}