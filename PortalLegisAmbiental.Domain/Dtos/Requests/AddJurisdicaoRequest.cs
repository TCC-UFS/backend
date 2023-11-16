namespace PortalLegisAmbiental.Domain.Dtos.Requests
{
    public class AddJurisdicaoRequest
    {
        public string Ambito { get; set; } = null!;
        public string? Estado { get; set; } = null!;
        public string Sigla { get; set; } = null!;
    }
}
