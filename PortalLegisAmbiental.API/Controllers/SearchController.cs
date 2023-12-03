using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using PortalLegisAmbiental.API.Controllers.Base;
using PortalLegisAmbiental.Application.Services.Interfaces;
using PortalLegisAmbiental.Domain.Dtos.Requests;

namespace PortalLegisAmbiental.API.Controllers
{
    [ApiVersion("1")]
    [Route("api/v{version:apiVersion}/search")]
    public class SearchController : BaseController
    {
        private readonly IElasticService _elasticService;

        public SearchController(IElasticService elasticService)
        {
            _elasticService = elasticService;
        }

        [HttpPost]
        public async Task<IActionResult> Search([FromBody(EmptyBodyBehavior = EmptyBodyBehavior.Allow)] SearchRequest request,
            int page = 1, int limit = 10)
        {
            var results = await _elasticService.Search(request, page, limit);
            return Ok(results);
        }
    }
}
