namespace PortalLegisAmbiental.Domain.Entities
{
    public class ConteudoAto
    {
        public ulong Id { get; set; }
        public string? CaminhoArquivo { get; set; }
        public bool PossuiConteudo { get; set; }
        public bool PossuiHtml { get; set; }
        public bool IsActive { get; set; }
        public ulong AtoId { get; set; }
    }
}
