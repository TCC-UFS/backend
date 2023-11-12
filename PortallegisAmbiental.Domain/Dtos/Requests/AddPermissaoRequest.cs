namespace PortalLegisAmbiental.Domain.Dtos.Requests
{
    public class AddPermissaoRequest
    {
        public string Recurso { get; set; } = null!;
        public string Scope { get; set; } = null!;
    }
}
