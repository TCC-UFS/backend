using System.Text.Json.Serialization;

namespace PortalLegisAmbiental.Domain.Dtos.Requests
{
    public class UpdateAtoRequest
    {
        [JsonIgnore]
        public ulong Id { get; set; }
        public string? Numero { get; set; }
        public string? Ementa { get; set; }
        public DateTime? DataPublicacao { get; set; }
        public DateTime? DataAto { get; set; }
        public string? CaminhoArquivo { get; set; }
        public string? Conteudo { get; set; }
        public string? Html { get; set; }
        public bool Disponivel { get; set; }
        public ulong TipoAtoId { get; set; }
        public ulong JurisdicaoId { get; set; }
    }
}