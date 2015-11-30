using System;
using System.Diagnostics;
using System.IO;
using Difi.Felles.Utility;
using Difi.Oppslagstjeneste.Klient;

namespace Difi.SikkerDigitalPost.Klient
{
    /// <summary>
    /// Inneholder konfigurasjon for sending av digital post.
    /// </summary> 
    public abstract class Klientkonfigurasjon
    {
        public AbstraktMiljø Miljø { get; set; }

        /// <summary>
        /// Angir host som skal benyttes i forbindelse med bruk av proxy. Både ProxyHost og ProxyPort må spesifiseres for at en proxy skal benyttes. Denne verdien kan også overstyres i 
        /// applikasjonens konfigurasjonsfil gjennom med appSettings verdi med nøkkelen 'SDP:ProxyHost'.
        /// </summary>
        public string ProxyHost { get; set; }

        /// <summary>
        /// Angir schema ved bruk av proxy. Standardverdien er 'https'. Denne verdien kan også overstyres i 
        /// applikasjonens konfigurasjonsfil gjennom med appSettings verdi med nøkkelen 'SDP:ProxyScheme'.
        /// </summary>
        public string ProxyScheme { get; set; }

        /// <summary>
        /// Angir portnummeret som skal benyttes i forbindelse med bruk av proxy. Både ProxyHost og ProxyPort må spesifiseres for at en proxy skal benyttes. Denne verdien kan også overstyres i 
        /// applikasjonens konfigurasjonsfil gjennom med appSettings verdi med nøkkelen 'SDP:ProxyPort'.
        /// </summary>
        public int ProxyPort { get; set; }

        /// <summary>
        /// Angir timeout for komunikasjonen fra og til meldingsformindleren. Default tid er 30 sekunder. Denne verdien kan også overstyres i 
        /// applikasjonens konfigurasjonsfil gjennom med appSettings verdi med nøkkelen 'SDP:TimeoutIMillisekunder'.
        /// </summary>
        public int TimeoutIMillisekunder { get; set; }

        /// <summary>
        /// Eksponerer et grensesnitt for logging hvor brukere kan integrere sin egen loggefunksjonalitet eller en tredjepartsløsning som f.eks log4net. For bruk, angi en annonym funksjon med 
        /// følgende parametre: severity, konversasjonsid, metode, melding. Som default benyttes trace logging med navn 'SikkerDigitalPost.Klient' som kan aktiveres i applikasjonens konfigurasjonsfil. 
        /// </summary>
        public Action<TraceEventType, Guid?, string, string> Logger { get; set; }

        /// <summary>
        /// Indikerer om proxy skal benyttes for oppkoblingen mot meldingsformidleren.
        /// </summary>
        public bool BrukProxy
        {
            get
            {
                return !string.IsNullOrWhiteSpace(ProxyHost) && ProxyPort > 0;
            }
        }

        /// <summary>
        /// Setter om logging skal gjøres til fil for alle meldinger som går mellom Klientbibliotek og Meldingsformidler
        /// </summary>
        public bool LoggXmlTilFil { get; set; }

        public string StandardLoggSti { get; set; }

        /// <summary>
        /// Klientkonfigurasjon som brukes ved oppsett av <see cref="SikkerDigitalPostKlient"/>.  Brukes for å sette parametere
        /// som proxy, timeout og URI til meldingsformidler.
        /// </summary>
        public Klientkonfigurasjon(AbstraktMiljø miljø)
        {
            Miljø = miljø;
            ProxyHost = null;
            ProxyScheme = "https";
            TimeoutIMillisekunder = (int)TimeSpan.FromSeconds(30).TotalMilliseconds;
            Logger = Oppslagstjeneste.Klient.Logger.TraceLogger();
            LoggXmlTilFil = false;
            StandardLoggSti = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Digipost");
        }
    }
}
