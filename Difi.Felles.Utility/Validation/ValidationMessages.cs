using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Schema;

namespace Difi.Felles.Utility.Validation
{
    public class ValidationMessages : List<string>
    {
        public bool HasErrors { get; private set; }

        public bool HasWarnings { get; private set; }

        internal void Add(XmlSeverityType severity, string message)
        {
            Add(message);

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

        public override string ToString()
        {
            return Count <= 0 ? "" : this.Aggregate((current, variable) => current + Environment.NewLine + variable);
        }

        internal void Reset()
        {
            Clear();
            HasWarnings = HasErrors = false;
        }
    }
}