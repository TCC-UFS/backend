namespace PortalLegisAmbiental.Domain.Entities
{
    public class ConteudoAto
    {
        public ulong Id { get; private set; }
        public string? CaminhoArquivo { get; private set; }
        public bool PossuiConteudo { get; private set; }
        public bool PossuiHtml { get; private set; }
        public bool IsActive { get; private set; }
        public ulong AtoId { get; private set; }

        public ConteudoAto(
            bool possuiConteudo,
            bool possuiHtml,
            string? caminhoArquivo = null)
        {
            CaminhoArquivo = caminhoArquivo;
            PossuiConteudo = possuiConteudo;
            PossuiHtml = possuiHtml;
            IsActive = true;
        }

        public void UpdateFilePath(string caminhoArquivo)
        {
            CaminhoArquivo = caminhoArquivo;
        }

        public void HasContent()
        {
            PossuiConteudo = true;
        }

        public void HasntContent()
        {
            PossuiConteudo = false;
        }

        public void HasHtml()
        {
            PossuiHtml = true;
        }

        public void HasntHtml()
        {
            PossuiHtml = false;
        }

        public void SetAtoId(ulong id)
        {
            AtoId = id;
        }

        public void Disable()
        {
            IsActive = false;
        }
    }
}
