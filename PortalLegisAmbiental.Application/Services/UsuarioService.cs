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
    public class UsuarioService : IUsuarioService
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IGrupoRepository _grupoRepository;
        private readonly IValidator<AddUsuarioRequest> _addValidator;
        private readonly IValidator<UpdateUsuarioRequest> _updateValidator;
        private readonly IMapper _mapper;

        public UsuarioService(IUsuarioRepository usuarioRepository, IGrupoRepository grupoRepository, IMapper mapper,
            IValidator<AddUsuarioRequest> addValidator, IValidator<UpdateUsuarioRequest> updateValidator)
        {
            _usuarioRepository = usuarioRepository;
            _grupoRepository = grupoRepository;
            _mapper = mapper;
            _addValidator = addValidator;
            _updateValidator = updateValidator;
        }

        public async Task Add(AddUsuarioRequest usuarioRequest)
        {
            _addValidator.ValidateAndThrow(usuarioRequest);

            var usuario = _mapper.Map<Usuario>(usuarioRequest);

            var exists = await _usuarioRepository.Exists(usuario);
            if (exists)
                throw new PortalLegisDomainException(
                    "ALREADY_REGISTRED", "Email já cadastrado.",
                    HttpStatusCode.Conflict);

            await _usuarioRepository.Add(usuario);
            _usuarioRepository.UnitOfWork.SaveChanges();
        }

        public async Task<List<UsuarioResponse>> GetAll()
        {
            var usuarios = await _usuarioRepository.GetAll(true);
            return _mapper.Map<List<UsuarioResponse>>(usuarios);
        }

        public async Task<List<UsuarioResponse>> SearchByName(string? name)
        {
            if (name == null) name = string.Empty;
            var usuarios = await _usuarioRepository.SearchByName(name, true, true);
            return _mapper.Map<List<UsuarioResponse>>(usuarios);
        }

        public async Task<List<UsuarioResponse>> SearchByEmail(string? email)
        {
            if (email == null) email = string.Empty;
            var usuarios = await _usuarioRepository.SearchByName(email, true, true);
            return _mapper.Map<List<UsuarioResponse>>(usuarios);
        }

        public async Task<UsuarioResponse> GetById(ulong id)
        {
            var usuario = await _usuarioRepository.GetById(id, true, true);
            
            if (usuario == null)
                throw new PortalLegisDomainException(
                    "KEY_NOT_FOUND", "Id não encontrado.",
                    HttpStatusCode.NotFound);

            return _mapper.Map<UsuarioResponse>(usuario);
        }

        public async Task Update(UpdateUsuarioRequest usuarioRequest)
        {
            _updateValidator.ValidateAndThrow(usuarioRequest);

            var usuario = await _usuarioRepository.GetById(usuarioRequest.Id);

            if (usuario == null)
                throw new PortalLegisDomainException(
                    "KEY_NOT_FOUND", "Id não encontrado.",
                    HttpStatusCode.NotFound);

            usuario.UpdateName(usuarioRequest.Nome);
            usuario.UpdateEmail(usuarioRequest.Email);
            usuario.UpdatePassword(usuarioRequest.Senha);

            if (string.IsNullOrEmpty(usuarioRequest.Email))
            {
                var exists = await _usuarioRepository.Exists(usuario);
                if (exists)
                    throw new PortalLegisDomainException(
                        "ALREADY_REGISTRED", "Email já cadastrado.",
                        HttpStatusCode.Conflict);
            }

            _usuarioRepository.UnitOfWork.SaveChanges();
        }

        public async Task AddGroup(UserGroupRequest request)
        {
            var grupo = await _grupoRepository.GetByName(request.Grupo, true);
            if (grupo == null)
                throw new PortalLegisDomainException(
                    "KEY_NOT_FOUND", "Grupo não encontrado.",
                    HttpStatusCode.NotFound);

            var usuario = await _usuarioRepository.GetById(request.UserId, includeGroups: true);
            if (usuario == null)
                throw new PortalLegisDomainException(
                    "KEY_NOT_FOUND", "Usuario não encontrado.",
                    HttpStatusCode.NotFound);

            if (usuario.HasGroup(grupo))
                throw new PortalLegisDomainException(
                    "PERMISSION_ALREADY_ADDED", "O usuario já está nesse grupo.");

            usuario.AddGroup(grupo);
            _usuarioRepository.UnitOfWork.SaveChanges();
        }

        public async Task RemoveGroup(UserGroupRequest request)
        {
            var grupo = await _grupoRepository.GetByName(request.Grupo, true);
            if (grupo == null)
                throw new PortalLegisDomainException(
                    "KEY_NOT_FOUND", "Grupo não encontrado.",
                    HttpStatusCode.NotFound);

            var usuario = await _usuarioRepository.GetById(request.UserId, includeGroups: true);
            if (usuario == null)
                throw new PortalLegisDomainException(
                    "KEY_NOT_FOUND", "Usuario não encontrado.",
                    HttpStatusCode.NotFound);

            if (!usuario.HasGroup(grupo))
                throw new PortalLegisDomainException(
                    "USER_GROUP_NOT_FOUND", "O usuario não está nesse grupo.");

            if (usuario.Grupos.Count == 1)
                throw new PortalLegisDomainException(
                    "INVALID_OPERATION", "O usuario só possui este grupo e precisa estar em pelo menos um grupo.",
                    HttpStatusCode.UnprocessableEntity);

            usuario.RemoveGroup(grupo);
            _usuarioRepository.UnitOfWork.SaveChanges();
        }

        public async Task Disable(ulong id)
        {
            var usuario = await _usuarioRepository.GetById(id);

            if (usuario == null)
                throw new PortalLegisDomainException(
                    "KEY_NOT_FOUND", "Id não encontrado.",
                    HttpStatusCode.NotFound);

            usuario.Disable();
            _usuarioRepository.UnitOfWork.SaveChanges();
        }
    }
}
