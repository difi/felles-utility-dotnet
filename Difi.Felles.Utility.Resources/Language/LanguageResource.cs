using System.Reflection;
using System.Resources;

namespace Difi.Felles.Utility.Resources.Language
{
    public class LanguageResource
    {
        private const string ResourceBasePath = "Difi.Felles.Utility.Resources.Language.Data";
        private static readonly string EnUs = $"{ResourceBasePath}.en-us";
        private static readonly string NbNo = $"{ResourceBasePath}.nb-no";
        private static readonly ResourceManager NorwegianResourceManager = new ResourceManager(NbNo, Assembly.GetExecutingAssembly());
        private static readonly ResourceManager EnglishResourceManager = new ResourceManager(EnUs, Assembly.GetExecutingAssembly());

        private static Language _currentLanguage = Language.Norwegian;
        public static Language CurrentLanguage
        {
            get
            {
                return _currentLanguage;
            }
            set
            {
                _currentLanguage = value;
                ResourceManager = CurrentLanguage == Language.Norwegian 
                    ? NorwegianResourceManager
                    : EnglishResourceManager;
            }
        }

        public static ResourceManager ResourceManager { get; private set; } = NorwegianResourceManager;
        
        public static string GetResource(LanguageResourceEnum languageResourceEnum)
        {
            var name = languageResourceEnum.ToString();
            return ResourceManager.GetString(name);
        }

        public static string GetResource(LanguageResourceEnum languageResourceEnum, Language temporaryLanguage)
        {
            var preLanguage = CurrentLanguage;

            CurrentLanguage = temporaryLanguage;
            var resource = GetResource(languageResourceEnum);
            CurrentLanguage = preLanguage;

            return resource;

        }


    }
}
