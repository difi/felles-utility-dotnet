namespace Difi.Felles.Utility.Exceptions
{
    public class CertificateChainValidationException : SecurityException
    {
        public CertificateChainValidationException(string message)
            : base(message)
        {
        }
    }
}
