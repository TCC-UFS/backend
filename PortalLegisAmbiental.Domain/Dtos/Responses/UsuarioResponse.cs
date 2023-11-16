namespace PortalLegisAmbiental.Domain.Dtos.Responses
{
    public class UsuarioResponse
    {
        public ulong Id { get; set; }
        public string Nome { get; set; } = null!;
        public string Email { get; set; } = null!;
        public List<GrupoResponse> Grupos { get; set; } = new();
    }
}
