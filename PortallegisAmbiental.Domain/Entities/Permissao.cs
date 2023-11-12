using PortalLegisAmbiental.Domain.Enums;
using System.Text.Json.Serialization;

namespace PortalLegisAmbiental.Domain.Entities
{
    public class Permissao
    {
        public ulong Id { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter))]
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

        public void UpdateScope(EScopeType? scope)
        {
            if (scope.HasValue)
                Scope = scope.Value;
        }

        public void UpdateResource(string? recurso)
        {
            if (!string.IsNullOrEmpty(recurso))
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
