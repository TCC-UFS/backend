using Microsoft.AspNetCore.Http;

namespace PortalLegisAmbiental.Domain.Dtos.Responses
{
    public class AtoResponse
    {
        public ulong Id { get; set; }
        public string Numero { get; set; } = null!;
        public string Ementa { get; set; } = null!;
        public DateTime DataPublicacao { get; set; }
        public DateTime DataAto { get; set; }
        public string? CaminhoArquivo { get; set; }
        public bool PossuiConteudo { get; set; }
        public bool PossuiHtml { get; set; }
        public bool Disponivel { get; set; }
        public TipoAtoResponse TipoAto { get; set; } = null!;
        public JurisdicaoResponse Jurisdicao { get; set; } = null!;
        public UsuarioResponse CreatedBy { get; set; } = null!;
        public string ConteudoHtml { get; set; } = null!;
        public string Arquivo { get; set; } = null!;
    }
}
