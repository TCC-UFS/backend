using System.Net;
using System.Runtime.Serialization;

namespace PortalLegisAmbiental.Domain.Exceptions
{
    [Serializable]
    public class PortalLegisDomainException : Exception
    {
        public string? Code { get; private set; }
        public HttpStatusCode StatusCode { get; private set; }

        public PortalLegisDomainException(
            string code,
            string message,
            HttpStatusCode statusCode = HttpStatusCode.BadRequest)
            : base(message)
        {
            Code = code;
            StatusCode = statusCode;
        }

        protected PortalLegisDomainException(SerializationInfo info, StreamingContext context)
        : base(info, context) { }
    }
}
