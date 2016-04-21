using System;

namespace Difi.Felles.Utility
{
    public abstract class AbstractEnvironment
    {
        public Uri Url { get; set; }

        public CertificateChainValidator CertificateChainValidator { get; set; }
    }
}