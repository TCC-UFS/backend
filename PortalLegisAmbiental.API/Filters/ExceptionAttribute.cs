using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;

namespace PortalLegisAmbiental.API.Filters
{
    public class ExceptionAttribute : TypeFilterAttribute
    {
        public ExceptionAttribute() : base(typeof(ExceptionFilter)) { }
    }
}
