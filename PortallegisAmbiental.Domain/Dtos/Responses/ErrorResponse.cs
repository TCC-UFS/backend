namespace PortalLegisAmbiental.Domain.Dtos.Responses
{
    public class ErrorResponse
    {
        public string Code { get; private set; }
        public string Instance { get; private set; }
        public List<string> Errors { get; private set; }
        public string? Exception { get; private set; }

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
        }
    }
}
