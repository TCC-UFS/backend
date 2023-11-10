using PortalLegisAmbiental.Domain.Enums;

namespace PortalLegisAmbiental.Domain.Entities
{
    public class Permissao
    {
        public ulong Id { get; set; }
        public EScopeType Scope { get; set; }
        public string Recurso { get; set; }
        public bool IsActive { get; set; }

        public virtual List<Grupo> Grupos { get; set; } = null!;

        public Permissao(EScopeType scope, string recurso)
        {
            Scope = scope;
            Recurso = recurso;
            IsActive = true;
        }

        public void UpdateScope(EScopeType scope)
        {
            Scope = scope;
        }

        public void UpdateResource(string recurso)
        {
            Recurso = recurso;
        }

        public void AddGroup(Grupo grupo)
        {
            Grupos.Add(grupo);
        }

        public bool RemoveGroup(Grupo grupo)
        {
            return Grupos.Remove(grupo);
        }

        public void Disable()
        {
            IsActive = false;
        }
    }
}
