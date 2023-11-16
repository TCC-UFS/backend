namespace PortalLegisAmbiental.Domain.Dtos.Requests
{
    public class AddUsuarioRequest
    {
        public string Nome { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Senha { get; set; } = null!;
    }
}