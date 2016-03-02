using System;

namespace Difi.Felles.Utility.Exceptions
{
    public class SecurityException : DifiException
    {
        public SecurityException()
        {
        }

        public SecurityException(string message)
            : base(message)
        {
        }

        public SecurityException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}