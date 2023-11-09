using System.ComponentModel.DataAnnotations.Schema;

namespace PortalLegisAmbiental.Domain.Entities
{
    public class Ato
    {
        public ulong Id { get; set; }
        public string Numero { get; set; }
        public string Ementa { get; set; }
        public DateTime DataPublicacao { get; set; }
        public DateTime DataAto { get; set; }
        public bool Disponivel { get; set; }
        public bool IsActive { get; set; }

        public virtual ConteudoAto ConteudoAto { get; set; } = null!;
        public ulong? CreatedById { get; set; }
        public virtual Usuario? CreatedBy { get; set; } = null!;
        public ulong JurisdicaoId { get; set; }
        public virtual Jurisdicao Jurisdicao { get; set; } = null!;
        public ulong TipoAtoId { get; set; }
        public virtual TipoAto TipoAto { get; set; } = null!;
    }
}
