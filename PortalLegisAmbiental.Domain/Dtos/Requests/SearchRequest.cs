using PortalLegisAmbiental.Domain.Enums;

namespace PortalLegisAmbiental.Domain.Dtos.Requests
{
    public class SearchRequest
    {
        public string? Termo { get; set; }
        public string? GrauAprox { get; set; }
        public string? Numero { get; set; }
        public string? Tipo { get; set; }
        public EAmbitoType? Ambito { get; set; }
        public string? Jurisdicao { get; set; }
        public string? Order { get; set; }
    }
}
