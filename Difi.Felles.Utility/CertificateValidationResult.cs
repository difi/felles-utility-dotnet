namespace Difi.Felles.Utility
{
    public class CertificateValidationResult
    {
        public CertificateValidationResult(CertificateValidationType type, string message)
        {
            Type = type;
            Message = message;
        }

        public CertificateValidationType Type { get; set; }

        public string Message { get; set; }
    }
}