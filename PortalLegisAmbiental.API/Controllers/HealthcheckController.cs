using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PortalLegisAmbiental.API.Controllers.Base;
using PortalLegisAmbiental.Domain.Dtos;
using PortalLegisAmbiental.Domain.IRepositories;

namespace PortalLegisAmbiental.API.Controllers
{
    [ApiVersion("1")]
    [Route("api/v{version:apiVersion}/healthcheck")]
    public class HealthcheckController : BaseController
    {
        private readonly ISearchRepository _searchRepository;
        public HealthcheckController(ISearchRepository searchRepository)
        {
            _searchRepository = searchRepository;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Get(string frase)
        {
            var search = new ElasticDto.Search()
            {
                Jurisdicao = "br",
                Query = new
                {
                    query = new
                    {
                        match_phrase = new
                        {
                            Conteudo = new
                            {
                                query = frase
                            }
                        }
                    }
                }
            };
            var response = await _searchRepository.Search(search);
            return Ok(response?.Hits?.Hits);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Post()
        {
            return Ok("I'm alive and postting.");
        }

        [HttpDelete]
        [Authorize]
        public async Task<IActionResult> Delete()
        {
            return Ok("I'm alive and deleting.");
        }
    }
}