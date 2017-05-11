using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Schema;
using Difi.Felles.Utility.Resources.Language;
using static Difi.Felles.Utility.Resources.Language.LanguageResource;
using static Difi.Felles.Utility.Resources.Language.LanguageResourceKey;

namespace Difi.Felles.Utility.Validation
{
    internal class XmlValidationRunner
    {
        private static Dictionary<Guid, XmlSchemaSet> _cachedSchemaSets = new Dictionary<Guid, XmlSchemaSet>();

        internal static readonly List<string> ToleratedErrors = new List<string>
        {
            GetResource(ToleratedXsdIdError, Language.Norwegian),
            GetResource(ToleratedXsdIdError, Language.English),
            GetResource(ToleratedPrefixListError, Language.Norwegian),
            GetResource(ToleratedPrefixListError, Language.English)
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
            Guid guid = this.GetType().GUID;
            lock (_cachedSchemaSets)
                if (!_cachedSchemaSets.ContainsKey(guid))
                    _cachedSchemaSets.Add(guid, XmlSchemaSet);
            settings.Schemas = _cachedSchemaSets[guid];
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