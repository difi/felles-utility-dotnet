using System;

namespace Difi.Felles.Utility.Exceptions
{
    [Obsolete("No reason to have a common SecurityException as it creates implicit library dependencies.")]
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