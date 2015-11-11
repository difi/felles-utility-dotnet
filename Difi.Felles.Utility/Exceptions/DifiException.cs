using System;

namespace Difi.Felles.Utility.Exceptions
{
    [Serializable]
    public class DifiException : Exception
    {
        public DifiException()
        {

        }

        public DifiException(string message) : base(message)
        {

        }

        public DifiException(string message, Exception inner) : base(message, inner)
        {

        }
    }


}
