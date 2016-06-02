using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Schema;

namespace Difi.Felles.Utility.Validation
{
    public class ValidationMessages
    {
        public ValidationMessages()
        {
            Messages = new List<string>();
        }

        public bool HasErrors { get; private set; }

        public bool HasWarnings { get; private set; }

        private List<string> Messages { get; }

        internal void Add(XmlSeverityType severity, string message)
        {
            Messages.Add(message);

            switch (severity)
            {
                case XmlSeverityType.Warning:
                    HasWarnings = true;
                    break;
                case XmlSeverityType.Error:
                    HasErrors = true;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public int Count()
        {
            return Messages.Count;
        }

        public override string ToString()
        {
            return Messages.Count <= 0 ? "" : Messages.Aggregate((current, variable) => current + Environment.NewLine + variable);
        }

        internal void Reset()
        {
            Messages.Clear();
            HasWarnings = HasErrors = false;
        }

        public List<string> ToList()
        {
            return Messages.AsReadOnly().ToList();
        }
    }
}