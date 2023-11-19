using PortalLegisAmbiental.Domain.Enums;
using System.Text.Json.Serialization;

namespace PortalLegisAmbiental.Domain.Entities
{
    public class Permissao
    {
        public ulong Id { get; private set; }
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public EScopeType Scope { get; private set; }
        public string Recurso { get; private set; }
        public bool IsActive { get; private set; }

        public virtual List<Grupo> Grupos { get; set; } = null!;

        public Permissao(EScopeType scope, string recurso)
        {
            Scope = scope;
            Recurso = recurso.ToLower();
            IsActive = true;
        }

        public void SetId(ulong id)
        {
            Id = id;
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
