namespace PortalLegisAmbiental.Domain.Dtos.Responses
{
    public class ErrorResponse
    {
        public string Code { get; set; } = null!;
        public string Instance { get; set; }
        public List<string> Errors { get; set; }
        public string? Exception { get; set; }
        public string? InnerException { get; set; } = null;

        public ErrorResponse(string instance, List<string> errors, string? exception)
        {
            Instance = instance;
            Errors = errors;
            Exception = exception;
        }

        public void SetCode(string code)
        {
            Code = code;
        }

        public void NoException()
        {
            Exception = null;
            InnerException = null;
        }
    }
}
