using System;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Schema;

namespace Difi.Felles.Utility
{
    public abstract class XmlValidator
    {
        private const string ToleratedError = "It is an error if there is a member of the attribute uses of a type definition with type xs:ID or derived from xs:ID and another attribute with type xs:ID matches an attribute wildcard.";
        private const string ErrorToleratedPrefix = "The 'PrefixList' attribute is invalid - The value '' is invalid according to its datatype 'http://www.w3.org/2001/XMLSchema:NMTOKENS' - The attribute value cannot be empty.";
        private const string WarningMessage = "\tWarning: Matching schema not found. No validation occurred.";

        private readonly XmlSchemaSet _schemaSet = new XmlSchemaSet();
        private bool _hasErrors;
        private bool _hasWarnings;

        public string ValidationWarnings { get; private set; }

        public bool Validate(string document)
        {
            var settings = new XmlReaderSettings();
            settings.Schemas.Add(_schemaSet);
            settings.ValidationType = ValidationType.Schema;
            settings.ValidationFlags = XmlSchemaValidationFlags.ReportValidationWarnings;
            settings.ValidationEventHandler += ValidationEventHandler;

            var xmlReader = System.Xml.XmlReader.Create(new MemoryStream(Encoding.UTF8.GetBytes(document)), settings);

            while (xmlReader.Read())
            {
            }

            return _hasErrors == false && _hasWarnings == false;
        }

        private void ValidationEventHandler(object sender, ValidationEventArgs e)
        {
            switch (e.Severity)
            {
                case XmlSeverityType.Warning:
                    ValidationWarnings += $"{WarningMessage} {e.Message}\n";
                    _hasWarnings = true;
                    break;
                case XmlSeverityType.Error:

                    if (e.Message.Equals(ToleratedError) || e.Message.Equals(ErrorToleratedPrefix))
                    {
                        //supress standard error in DIFI contract
                    }
                    else
                    {
                        ValidationWarnings += $"{e.Message}\n";
                        _hasErrors = true;
                    }
                     
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
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