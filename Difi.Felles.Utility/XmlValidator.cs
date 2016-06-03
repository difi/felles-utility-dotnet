using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Schema;
using Difi.Felles.Utility.Validation;

namespace Difi.Felles.Utility
{
    public abstract class XmlValidator
    {
        private const string ToleratedXsdIdErrorEnUs = "It is an error if there is a member of the attribute uses of a type definition with type xs:ID or derived from xs:ID and another attribute with type xs:ID matches an attribute wildcard.";
        private const string ToleratedXsdIdErrorNbNo = "Det er en feil hvis det finnes et medlem av attributtet som bruker en typedefinisjon med typen xs:ID eller avledet fra xs:ID og et annet attributt med typen xs:ID tilsvarer et attributtjokertegn.";
        private const string ToleratedPrefixListErrorEnUs = "The 'PrefixList' attribute is invalid - The value '' is invalid according to its datatype 'http://www.w3.org/2001/XMLSchema:NMTOKENS' - The attribute value cannot be empty.";
        private const string ToleratedPrefixListErrorNbNo = "Attributtet PrefixList er ugyldig - Verdien  er ugyldig i henhold til datatypen http://www.w3.org/2001/XMLSchema:NMTOKENS - Attributtverdien kan ikke være tom.";

        private static readonly List<string> ToleratedErrors = new List<string> {ToleratedXsdIdErrorEnUs, ToleratedXsdIdErrorNbNo, ToleratedPrefixListErrorEnUs, ToleratedPrefixListErrorNbNo};

        private readonly XmlSchemaSet _schemaSet = new XmlSchemaSet();

        protected XmlValidator()
        {
            ValidationMessages = new ValidationMessages();
        }

        public ValidationMessages ValidationMessages { get; }

        public bool Validate(string document)
        {
            ResetState();

            var settings = new XmlReaderSettings();
            settings.Schemas.Add(_schemaSet);
            settings.ValidationType = ValidationType.Schema;
            settings.ValidationFlags = XmlSchemaValidationFlags.ReportValidationWarnings;
            settings.ValidationEventHandler += ValidationEventHandler;

            var xmlReader = System.Xml.XmlReader.Create(new MemoryStream(Encoding.UTF8.GetBytes(document)), settings);

            while (xmlReader.Read())
            {
            }

            return !ValidationMessages.HasErrors && !ValidationMessages.HasWarnings;
        }

        private void ResetState()
        {
            ValidationMessages.Reset();
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

        protected void AddXsd(string @namespace, string fileName)
        {
            _schemaSet.Add(@namespace, XmlReader(fileName));
        }

        protected void AddXsd(string @namespace, XmlReader reader)
        {
            _schemaSet.Add(@namespace, reader);
        }

        private static XmlReader XmlReader(string fil)
        {
            Stream s = new MemoryStream(File.ReadAllBytes(fil));
            return System.Xml.XmlReader.Create(s);
        }
    }
}