using PortalLegisAmbiental.Domain.Enums;

namespace PortalLegisAmbiental.Domain.Entities
{
    public class Jurisdicao
    {
        public ulong Id { get; private set; }
        public EAmbitoType Ambito { get; private set; }
        public string Sigla { get; private set; }
        public string? Estado { get; private set; }
        public bool IsActive { get; private set; }

        public Jurisdicao(
            EAmbitoType ambito,
            string sigla,
            string? estado)
        {
            Ambito = ambito;
            Sigla = sigla;
            Estado = estado;
            IsActive = true;
        }

        public void UpdateAmbito(EAmbitoType ambito)
        {
            Ambito = ambito;
        }

        public void UpdateSigla(string sigla)
        {
            Sigla = sigla;
        }

        public void UpdateState(string estado)
        {
            Estado = estado;
        }

        public void Disable()
        {
            IsActive = false;
        }
    }
}
