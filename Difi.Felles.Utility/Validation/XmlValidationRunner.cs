using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Schema;
using Difi.Felles.Utility.Resources.Language;

namespace Difi.Felles.Utility.Validation
{
    internal class XmlValidationRunner
    {
        internal static readonly List<string> ToleratedErrors = new List<string>
        {
            LanguageResource.GetResource(LanguageResourceEnum.ToleratedXsdIdError, Language.Norwegian),
            LanguageResource.GetResource(LanguageResourceEnum.ToleratedXsdIdError, Language.English),
            LanguageResource.GetResource(LanguageResourceEnum.ToleratedPrefixListError, Language.Norwegian),
            LanguageResource.GetResource(LanguageResourceEnum.ToleratedPrefixListError, Language.English),
        };

        internal XmlValidationRunner(XmlSchemaSet xmlSchemaSet)
        {
            XmlSchemaSet = xmlSchemaSet;
        }

        internal XmlSchemaSet XmlSchemaSet { get; }

        internal ValidationMessages ValidationMessages { get; } = new ValidationMessages();

        internal bool Validate(string document)
        {
            var settings = new XmlReaderSettings();
            settings.Schemas.Add(XmlSchemaSet);
            settings.ValidationType = ValidationType.Schema;
            settings.ValidationFlags = XmlSchemaValidationFlags.ReportValidationWarnings;
            settings.ValidationEventHandler += ValidationEventHandler;

            var xmlReader = XmlReader.Create(new MemoryStream(Encoding.UTF8.GetBytes(document)), settings);

            while (xmlReader.Read())
            {
            }

            return !ValidationMessages.HasErrors && !ValidationMessages.HasWarnings;
        }

        private void ValidationEventHandler(object sender, ValidationEventArgs e)
        {
            if (IsToleratedError(e))
            {
            }
            else
            {
                ValidationMessages.Add(e.Severity, e.Message);
            }
        }

        private static bool IsToleratedError(ValidationEventArgs e)
        {
            return ToleratedErrors.Contains(e.Message);
        }
    }
}