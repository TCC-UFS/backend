using Newtonsoft.Json;

namespace PortalLegisAmbiental.Domain.Dtos
{
    public static class ElasticDto
    {
        public class Data
        {
            public ulong IdAto { get; set; }
            public string Numero { get; set; } = string.Empty;
            public string Ementa { get; set; } = string.Empty;
            public string Conteudo { get; set; } = string.Empty;
            public string Html { get; set; } = string.Empty;
            public string TipoAto { get; set; } = string.Empty;
            public string Ambito { get; set; } = string.Empty;
            public string Jurisdicao { get; set; } = string.Empty;
            public DateTime DataPublicacao { get; set; }
            public DateTime DataAto { get; set; }
            public bool Disponivel { get; set; }
        }

        public class Search
        {
            public string? Jurisdicao { get; set; }
            public object Query { get; set; } = null!;
            public BaseQuery BaseQuery { get; set; } = null!;
        }

        public class BaseQuery
        {
            public List<object> Sort { get; set; } = null!;
            public int From { get; set; } = 0;
            public int Size { get; set; } = 10;
            public Query Query { get; set; } = null!;
        }

        public class Query
        {
            public BoolData Bool { get; set; } = null!;
        }

        public class BoolData
        {
            public List<object> Must { get; set; } = new();
            public List<FilterData> Filter { get; set; } = null!;
        }
        
        public class FilterData
        {
            public object Term { get; set; } = null!;
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
