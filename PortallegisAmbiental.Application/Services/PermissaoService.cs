using AutoMapper;
using FluentValidation;
using PortalLegisAmbiental.Application.Services.Interfaces;
using PortalLegisAmbiental.Domain.Dtos.Requests;
using PortalLegisAmbiental.Domain.Dtos.Responses;
using PortalLegisAmbiental.Domain.Entities;
using PortalLegisAmbiental.Domain.Enums;
using PortalLegisAmbiental.Domain.Exceptions;
using PortalLegisAmbiental.Domain.IRepositories;
using System.Net;

namespace PortalLegisAmbiental.Application.Services
{
    public class PermissaoService : IPermissaoService
    {
        private readonly IPermissaoRepository _permissaoRepository;
        private readonly IValidator<AddPermissaoRequest> _addValidator;
        private readonly IValidator<UpdatePermissaoRequest> _updateValidator;
        private readonly IMapper _mapper;

        public PermissaoService(IPermissaoRepository permissaoRepository, IMapper mapper, 
            IValidator<AddPermissaoRequest> addValidator, IValidator<UpdatePermissaoRequest> updateValidator)
        {
            _permissaoRepository = permissaoRepository;
            _mapper = mapper;
            _addValidator = addValidator;
            _updateValidator = updateValidator;
        }

        public async Task Add(AddPermissaoRequest permissaoRequest)
        {
            _addValidator.ValidateAndThrow(permissaoRequest);

            var permissao = _mapper.Map<Permissao>(permissaoRequest);
            var exists = await _permissaoRepository.Exists(permissao);

            if (exists)
                throw new PortalLegisDomainException(
                    "ALREADY_REGISTRED", "Permissao já cadastrada.",
                    HttpStatusCode.Conflict);

            await _permissaoRepository.Add(permissao);
            _permissaoRepository.UnitOfWork.SaveChanges();
        }

        public async Task<List<PermissaoResponse>> SearchByResource(string? recurso)
        {
            if (recurso == null) recurso = string.Empty;
            var permissoes = await _permissaoRepository.SearchByResource(recurso, true);
            return _mapper.Map<List<PermissaoResponse>>(permissoes);
        }

        public async Task<List<PermissaoResponse>> SearchByScope(string scope)
        {
            if (!Enum.TryParse<EScopeType>(scope, true, out var eScope))
                throw new PortalLegisDomainException(
                    "INVALID_SCOPE", "Escopo inválido.");

            var permissoes = await _permissaoRepository.SearchByScope(eScope, true);
            return _mapper.Map<List<PermissaoResponse>>(permissoes);
        }

        public async Task<PermissaoResponse> GetById(ulong id)
        {
            var permissao = await _permissaoRepository.GetById(id, true);
            
            if (permissao == null)
                throw new PortalLegisDomainException(
                    "KEY_NOT_FOUND", "Id não encontrado.",
                    HttpStatusCode.NotFound);

            return _mapper.Map<PermissaoResponse>(permissao);
        }

        public async Task Update(UpdatePermissaoRequest permissaoRequest)
        {
            _updateValidator.ValidateAndThrow(permissaoRequest);

            var permissao = await _permissaoRepository.GetById(permissaoRequest.Id);

            if (permissao == null)
                throw new PortalLegisDomainException(
                    "KEY_NOT_FOUND", "Id não encontrado.",
                    HttpStatusCode.NotFound);

            permissao.UpdateResource(permissaoRequest.Recurso);
            
            var exists = await _permissaoRepository.Exists(permissao);
            if (exists)
                throw new PortalLegisDomainException(
                    "ALREADY_REGISTRED", "Permissao já cadastrada.",
                    HttpStatusCode.Conflict);

            _permissaoRepository.UnitOfWork.SaveChanges();
        }

        public async Task Disable(ulong id)
        {
            var permissao = await _permissaoRepository.GetById(id);

            if (permissao == null)
                throw new PortalLegisDomainException(
                    "KEY_NOT_FOUND", "Id não encontrado.",
                    HttpStatusCode.NotFound);

            await _permissaoRepository.Disable(permissao);
            _permissaoRepository.UnitOfWork.SaveChanges();
        }
    }
}
