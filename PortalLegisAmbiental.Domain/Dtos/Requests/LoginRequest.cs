namespace PortalLegisAmbiental.Domain.Dtos.Requests
{
    public class LoginRequest
    {
        public string Email { get; set; } = null!;
        public string Senha { get; set; } = null!;
    }
}
