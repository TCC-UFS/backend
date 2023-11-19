using Microsoft.AspNetCore.Mvc;

namespace PortalLegisAmbiental.API.Filters
{
    public class PermissionAttribute : TypeFilterAttribute
    {
        public PermissionAttribute() : base(typeof(PermissionFilter)) { }
    }
}
