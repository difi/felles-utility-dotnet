using System.Reflection;
using System.Resources;

namespace Difi.Felles.Utility.Resources.Language
{
    public class LanguageResource
    {
        private static readonly string ResourceBasePath = "Difi.Felles.Utility.Resources.Language.Data";
        private static readonly string EnUs = $"{ResourceBasePath}.en-us";
        private static readonly string NbNo = $"{ResourceBasePath}.nb-no";
        private static readonly ResourceManager NorwegianResourceManager = new ResourceManager(NbNo, Assembly.GetExecutingAssembly());
        private static readonly ResourceManager EnglishResourceManager = new ResourceManager(EnUs, Assembly.GetExecutingAssembly());

        private static Resources.Language.Language _currentLanguage = Resources.Language.Language.Norwegian;
        public static Resources.Language.Language CurrentLanguage
        {
            get
            {
                return _currentLanguage;
            }
            set
            {
                _currentLanguage = value;
                ResourceManager = CurrentLanguage == Resources.Language.Language.Norwegian 
                    ? NorwegianResourceManager
                    : EnglishResourceManager;
            }
        }

        public static ResourceManager ResourceManager { get; private set; } = NorwegianResourceManager;
        
        public static string GetLanguageResource(LanguageResourceEnum languageResourceEnum)
        {
            var name = languageResourceEnum.ToString();
            return ResourceManager.GetString(name);
        }

        
    }
}
