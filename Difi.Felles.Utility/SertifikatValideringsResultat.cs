using System;

namespace Difi.Felles.Utility
{
    public class SertifikatValideringsResultat
    {
        public SertifikatValideringsResultat(SertifikatValideringType type, string melding)
        {
            Type = type;
            Melding = melding;
        }

        public SertifikatValideringType Type { get; set; }

        public string Melding { get; set; }
    }
}
