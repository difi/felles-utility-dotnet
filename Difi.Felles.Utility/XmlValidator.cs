using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Schema;
using Difi.Felles.Utility.Validation;

namespace Difi.Felles.Utility
{
    public abstract class XmlValidator
    {
        private readonly XmlSchemaSet _schemaSet = new XmlSchemaSet();

        public bool Validate(string document)
        {
            var validationRunner = new ValidationRunner(_schemaSet);
            return validationRunner.Validate(document);
        }

        public bool Validate(string document, out string validationMessage)
        {
            var validationRunner = new ValidationRunner(_schemaSet);
            var status = validationRunner.Validate(document);
            validationMessage = validationRunner.ValidationMessages.ToString();
            return status;
        }

        public bool Validate(string document, out List<string> validationMessages)
        {
            var validationRunner = new ValidationRunner(_schemaSet);
            var result = validationRunner.Validate(document);
            validationMessages = validationRunner.ValidationMessages;
            return result;
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