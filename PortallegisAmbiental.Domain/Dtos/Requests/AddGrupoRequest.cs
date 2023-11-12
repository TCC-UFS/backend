namespace PortalLegisAmbiental.Domain.Dtos.Requests
{
    public class AddGrupoRequest
    {
        public string Nome { get; set; } = null!;
        public List<AddPermissaoRequest> Permissoes { get; set; } = new();
    }
}
