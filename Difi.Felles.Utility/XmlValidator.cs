using System;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Schema;

namespace Difi.Felles.Utility
{
    internal abstract class XmlValidator
    {
        private bool _harWarnings;
        private bool _harErrors;

        private const string ToleratedError = "It is an error if there is a member of the attribute uses of a type definition with type xs:ID or derived from xs:ID and another attribute with type xs:ID matches an attribute wildcard.";
        private const string ErrorToleratedPrefix = "The 'PrefixList' attribute is invalid - The value '' is invalid according to its datatype 'http://www.w3.org/2001/XMLSchema:NMTOKENS' - The attribute value cannot be empty.";
        private const string WarningMessage = "\tWarning: Matching schema not found. No validation occurred.";
        private const string ErrorMessage = "\tValidation error:";

        readonly XmlSchemaSet _schemaSet = new XmlSchemaSet();

        public string ValideringsVarsler { get; private set; }

        public bool ValiderDokumentMotXsd(string document)
        {
            var settings = new XmlReaderSettings();
            settings.Schemas.Add(_schemaSet);
            settings.ValidationType = ValidationType.Schema;
            settings.ValidationFlags = XmlSchemaValidationFlags.ReportValidationWarnings;
            settings.ValidationEventHandler += ValidationEventHandler;

            var xmlReader = System.Xml.XmlReader.Create(new MemoryStream(Encoding.UTF8.GetBytes(document)), settings);

            while (xmlReader.Read()) { }

            return _harErrors == false && _harWarnings == false;
        }

        private void ValidationEventHandler(object sender, ValidationEventArgs e)
        {
            switch (e.Severity)
            {
                case XmlSeverityType.Warning:
                    ValideringsVarsler += String.Format("{0} {1}\n", WarningMessage, e.Message);
                    _harWarnings = true;
                    break;
                case XmlSeverityType.Error:
                    ValideringsVarsler += String.Format("{0} {1}\n", ErrorMessage, e.Message);
                    if (!e.Message.Equals(ToleratedError) && !e.Message.Equals(ErrorToleratedPrefix))
                        _harErrors = true;
                    else
                        ValideringsVarsler +=
                            "Feilen over er ikke noe vi håndterer, og er heller ikke årsaken til at validering feilet\n";
                    break;
            }
        }

        /// <summary>
        /// Legg til en referanse til en XSD-fil.
        /// </summary>
        /// <param name="navnerom">Navnerom på XSD-fil som lastes inn.</param>
        /// <param name="fil">Sti til ressurs </param>
        protected void LeggTilXsdRessurs(string navnerom, string fil)
        {
            _schemaSet.Add(navnerom, XmlReader(fil));
        }

        /// <summary>
        /// Legg til en referanse til en XSD-fil.
        /// </summary>
        /// <param name="navnerom">Navnerom på XSD-fil som lastes inn.</param>
        /// <param name="reader">Reader for XSD-fil</param>
        protected void LeggTilXsdRessurs(string navnerom, XmlReader reader)
        {
            _schemaSet.Add(navnerom, reader);
        }

        private XmlReader XmlReader(string fil)
        {
            Stream s = new MemoryStream(File.ReadAllBytes(fil));
            return System.Xml.XmlReader.Create(s);
        }
    }
}
