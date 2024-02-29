namespace PortalLegisAmbiental.Domain.Dtos.Responses
{
    public class StatsResponse
    {
        public int LeisOrdinariasFederais {  get; set; }
        public int LeisOrdinariasEstaduais { get; set; }
        public int LeisComplementaresFederais { get; set; }
        public int LeisComplementaresEstaduais { get; set; }
    }
}
