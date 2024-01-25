using Microsoft.AspNetCore.Http;
using System.Text.Json.Serialization;

namespace PortalLegisAmbiental.Domain.Dtos.Requests
{
    public class AddAtoRequest
    {
        public string Numero { get; set; } = null!;
        public string Ementa { get; set; } = null!;
        public DateTime DataPublicacao { get; set; }
        public DateTime DataAto { get; set; }
        [JsonIgnore]
        public string? CaminhoArquivo { get; set; }
        [JsonIgnore]
        public IFormFile? File { get; set; }
        public string? Conteudo { get; set; }
        public string? Html { get; set; }
        public bool Disponivel { get; set; }
        public ulong TipoAtoId { get; set; }
        public ulong JurisdicaoId { get; set; }
        [JsonIgnore]
        public ulong CreatedById { get; set; } = 1;
    }
}