using Microsoft.AspNetCore.Mvc;
using PortalLegisAmbiental.API.Filters;

namespace PortalLegisAmbiental.API.Controllers.Base
{
    [Exception]
    public class BaseController : ControllerBase
    {
        [NonAction]
        public BadRequestObjectResult Failed(string message)
        {
            return base.BadRequest(new
            {
                errors = new List<string>() { message }
            });
        }

        [NonAction]
        public ActionResult Success(object response)
        {
            if (response is null)
            {
                return NotFound();
            }

            return base.Ok(response);
        }
    }
}
