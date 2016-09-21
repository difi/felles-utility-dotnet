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

        public bool ErGyldigSertifikatkjede(X509Certificate2 sertifikat)
        {
            X509ChainStatus[] kjedestatus;
            return ErGyldigSertifikatkjede(sertifikat, out kjedestatus);
        }

        public bool ErGyldigSertifikatkjede(X509Certificate2 sertifikat, out X509ChainStatus[] kjedestatus)
        {
            var chain = new X509Chain
            {
                ChainPolicy = ChainPolicy()

            };

            var erGyldigResponssertifikat = chain.Build(sertifikat);

            ValiderAtKunBrukerValidatorSertifikater(chain,sertifikat);

            erGyldigResponssertifikat = ErGyldigResponssertifikatKjede(chain);
            
            kjedestatus = chain.ChainStatus;
            return erGyldigResponssertifikat;
        }

        private void ValiderAtKunBrukerValidatorSertifikater(X509Chain chain, X509Certificate2 sertifikat)
        {
            foreach (var chainElement in chain.ChainElements)
            {
                var erSertifikatSomValideres = ErSammeSertifikat(chainElement.Certificate, sertifikat);
                if (erSertifikatSomValideres) 
                {
                    continue;
                }

                var erISertifikatlager = SertifikatLager.Cast<X509Certificate2>().Any(lagerSertifikat => ErSammeSertifikat(chainElement.Certificate, lagerSertifikat));

                if (!erISertifikatlager)
                {
                    var chainAsString = chain.ChainElements.Cast<X509ChainElement>().Aggregate("",(result, curr) => GetCertificateInfo(result, curr.Certificate));
                    var lagerAsString = SertifikatLager.Cast<X509Certificate2>().Aggregate("", GetCertificateInfo);

                    throw new SecurityException($"Validering av sertifikat '{sertifikat.Subject}' (thumbprint '{sertifikat.Thumbprint}') feilet. Dette skjer fordi kjeden ble bygd " +
                                                $"med følgende sertifikater {chainAsString}, men kun følgende er godkjent for å bygge kjeden: {lagerAsString}");
                }
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

        private static bool ErGyldigResponssertifikatKjede(X509Chain chain)
        {
            if (!HarForventetLengde(chain, 3)) return false;

            switch (chain.ChainStatus.Length)
            {
                case 0:
                    return true;
                case 1:
                    var erUntrustedStatus = chain.ChainStatus.ElementAt(0).Status == X509ChainStatusFlags.UntrustedRoot;
                    return erUntrustedStatus;
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