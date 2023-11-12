﻿using Microsoft.AspNetCore.Mvc;
using PortalLegisAmbiental.API.Controllers.Base;
using PortalLegisAmbiental.Application.Services.Interfaces;
using PortalLegisAmbiental.Domain.Dtos.Requests;

namespace PortalLegisAmbiental.API.Controllers
{
    [ApiVersion("1")]
    [ApiController]
    [Route("api/v{version:apiVersion}/permissoes")]
    public class PermissaoController : BaseController
    {
        private readonly IPermissaoService _permissaoService;

        public PermissaoController(IPermissaoService permissaoService)
        {
            _permissaoService = permissaoService;
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddPermissaoRequest request)
        {
            await _permissaoService.Add(request);
            return NoContent();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(ulong id)
        {
            var response = await _permissaoService.GetById(id);
            return Ok(response);
        }

        [HttpGet("search")]
        public async Task<IActionResult> Search(string? resource)
        {
            var response = await _permissaoService.SearchByResource(resource);
            return Ok(response);
        }

        [HttpGet("search/{scope}")]
        public async Task<IActionResult> SearchScope(string scope)
        {
            var response = await _permissaoService.SearchByScope(scope);
            return Ok(response);
        }

        [HttpPatch("update/{id}")]
        public async Task<IActionResult> Update(ulong id, UpdatePermissaoRequest request)
        {
            request.Id = id;
            await _permissaoService.Update(request);
            return NoContent();
        }

        [HttpPatch("disable")]
        public async Task<IActionResult> Disable(ulong id)
        {
            await _permissaoService.Disable(id);
            return NoContent();
        }
    }
}