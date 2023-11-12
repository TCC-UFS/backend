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
    public class GrupoService : IGrupoService
    {
        private readonly IGrupoRepository _grupoRepository;
        private readonly IPermissaoRepository _permissaoRepository;
        private readonly IValidator<AddGrupoRequest> _addValidator;
        private readonly IValidator<UpdateGrupoRequest> _updateValidator;
        private readonly IValidator<AddPermissaoRequest> _addPermValidator;
        private readonly IMapper _mapper;

        public GrupoService(IGrupoRepository grupoRepository, IPermissaoRepository permissaoRepository, IMapper mapper,
            IValidator<AddGrupoRequest> addValidator, IValidator<UpdateGrupoRequest> updateValidator,
            IValidator<AddPermissaoRequest> addPermValidator, IValidator<UpdatePermissaoRequest> updatePermValidator)
        {
            _grupoRepository = grupoRepository;
            _permissaoRepository = permissaoRepository;
            _mapper = mapper;
            _addValidator = addValidator;
            _updateValidator = updateValidator;
            _addPermValidator = addPermValidator;
        }

        public async Task Add(AddGrupoRequest grupoRequest)
        {
            _addValidator.ValidateAndThrow(grupoRequest);

            foreach(var permissao in grupoRequest.Permissoes)
            {
                _addPermValidator.ValidateAndThrow(permissao);
            }

            var grupo = _mapper.Map<Grupo>(grupoRequest);

            var exists = await _grupoRepository.Exists(grupo);
            if (exists)
                throw new PortalLegisDomainException(
                    "ALREADY_REGISTRED", "Grupo já cadastrado.",
                    HttpStatusCode.Conflict);

            await _grupoRepository.Add(grupo);
            _grupoRepository.UnitOfWork.SaveChanges();
        }

        public async Task<List<GrupoResponse>> GetAll()
        {
            var grupos = await _grupoRepository.GetAll(true);
            return _mapper.Map<List<GrupoResponse>>(grupos);
        }

        public async Task<List<GrupoResponse>> SearchByName(string? name)
        {
            if (name == null) name = string.Empty;
            var grupos = await _grupoRepository.SearchByName(name, true, true);
            return _mapper.Map<List<GrupoResponse>>(grupos);
        }

        public async Task<GrupoResponse> GetById(ulong id)
        {
            var grupo = await _grupoRepository.GetById(id, true, true);
            
            if (grupo == null)
                throw new PortalLegisDomainException(
                    "KEY_NOT_FOUND", "Id não encontrado.",
                    HttpStatusCode.NotFound);

            return _mapper.Map<GrupoResponse>(grupo);
        }

        public async Task Update(UpdateGrupoRequest grupoRequest)
        {
            _updateValidator.ValidateAndThrow(grupoRequest);

            var grupo = await _grupoRepository.GetById(grupoRequest.Id);

            if (grupo == null)
                throw new PortalLegisDomainException(
                    "KEY_NOT_FOUND", "Id não encontrado.",
                    HttpStatusCode.NotFound);

            grupo.UpdateName(grupoRequest.Nome);
            
            var exists = await _grupoRepository.Exists(grupo);
            if (exists)
                throw new PortalLegisDomainException(
                    "ALREADY_REGISTRED", "Grupo já cadastrado.",
                    HttpStatusCode.Conflict);

            _grupoRepository.UnitOfWork.SaveChanges();
        }

        public async Task AddPermission(GroupPermissionRequest request)
        {
            var validatePermission = new AddPermissaoRequest() { Recurso = request.Recurso, Scope = request.Scope };
            _addPermValidator.ValidateAndThrow(validatePermission);

            var scope = Enum.Parse<EScopeType>(request.Scope, true);
            var permissao = await _permissaoRepository.GetByResourceAndScope(request.Recurso, scope, true);
            if (permissao == null)
            {
                var newPermission = _mapper.Map<Permissao>(validatePermission);
                permissao = await _permissaoRepository.AddAndReturn(newPermission);
            }
            
            var grupo = await _grupoRepository.GetById(request.GrupoId, includePermissions: true);
            if (grupo == null)
                throw new PortalLegisDomainException(
                    "KEY_NOT_FOUND", "Grupo não encontrado.",
                    HttpStatusCode.NotFound);

            if (grupo.HasPermission(permissao))
                throw new PortalLegisDomainException(
                    "PERMISSION_ALREADY_ADDED", "O grupo já possui esta permissão.");

            grupo.AddPermission(permissao);
            _grupoRepository.UnitOfWork.SaveChanges();
        }

        public async Task RemovePermission(GroupPermissionRequest request)
        {
            var validatePermission = new AddPermissaoRequest() { Recurso = request.Recurso, Scope = request.Scope };
            _addPermValidator.ValidateAndThrow(validatePermission);

            var scope = Enum.Parse<EScopeType>(request.Scope, true);
            var permissao = await _permissaoRepository.GetByResourceAndScope(request.Recurso, scope, true);
            if (permissao == null)
                throw new PortalLegisDomainException(
                    "KEY_NOT_FOUND", "Permissão não encontrada.",
                    HttpStatusCode.NotFound);

            var grupo = await _grupoRepository.GetById(request.GrupoId, includePermissions: true);
            if (grupo == null)
                throw new PortalLegisDomainException(
                    "KEY_NOT_FOUND", "Grupo não encontrado.",
                    HttpStatusCode.NotFound);

            if (!grupo.HasPermission(permissao))
                throw new PortalLegisDomainException(
                    "PERMISSION_GROUP_NOT_FOUND", "O grupo não possui esta permissão.");

            if (grupo.Permissoes.Count == 1)
                throw new PortalLegisDomainException(
                    "INVALID_OPERATION", "O grupo só possui esta permissão e precisa de pelo menos uma permissão.",
                    HttpStatusCode.UnprocessableEntity);

            grupo.RemovePermission(permissao);
            _grupoRepository.UnitOfWork.SaveChanges();
        }

        public async Task Disable(ulong id)
        {
            var grupo = await _grupoRepository.GetById(id);

            if (grupo == null)
                throw new PortalLegisDomainException(
                    "KEY_NOT_FOUND", "Id não encontrado.",
                    HttpStatusCode.NotFound);

            await _grupoRepository.Disable(grupo);
            _grupoRepository.UnitOfWork.SaveChanges();
        }
    }
}
