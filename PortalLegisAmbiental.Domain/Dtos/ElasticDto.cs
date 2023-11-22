using Newtonsoft.Json;

namespace PortalLegisAmbiental.Domain.Dtos
{
    public static class ElasticDto
    {
        public class Data
        {
            public ulong IdAto { get; set; }
            public string Conteudo { get; set; } = string.Empty;
            public string Html { get; set; } = string.Empty;
            public string TipoAto { get; set; } = string.Empty;
            public string Ambito { get; set; } = string.Empty;
            public string Jurisdicao { get; set; } = string.Empty;
        }

        public class Search
        {
            public string? Jurisdicao { get; set; }
            public object Query { get; set; } = null!;
        }

        public class ReadResponse
        {
            public HitsData Hits { get; set; } = null!;
        }

        public class HitsData
        {
            public TotalData Total { get; set; } = null!;
            public List<Hit> Hits { get; set; } = new();
        }

        public class TotalData
        {
            public int Value { get; set; }
            public string Relation { get; set; } = null!;
        }

        public class Hit
        {
            [JsonProperty("_id")]
            public string Id { get; set; } = null!;
            [JsonProperty("_score")]
            public decimal Score { get; set; }
            [JsonProperty("_source")]
            public Data data { get; set; } = null!;
        }
    }
}
