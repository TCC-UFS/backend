namespace PortalLegisAmbiental.Domain.Dtos.Responses
{
    public class SearchResponse
    {
        public Pagination Pagination { get; set; } = null!;
        public List<SearchResponseData> Data { get; set; } = new();
    }

    public class Pagination
    {
        public int Page { get; set; }
        public int Limit { get; set; }
        public int CurrentCount { get; set; }
        public int TotalCount { get; set; }
    }

    public class SearchResponseData
    {
        public ulong Id { get; set; }
        public string Numero { get; set; } = null!;
        public string Ementa { get; set; } = null!;
        public DateTime DataPublicacao { get; set; }
        public DateTime DataAto { get; set; }
        public string Conteudo { get; set; } = null!;
        public bool Disponivel { get; set; }
        public string TipoAto { get; set; } = null!;
        public string Jurisdicao { get; set; } = null!;
    }
}
