namespace PortalLegisAmbiental.Domain.Dtos.Responses
{
    public class GrupoResponse
    {
        public ulong Id { get; set; }
        public string Nome { get; set; } = null!;
        public List<PermissaoResponse> Permissoes { get; set; } = new();
    }
}
