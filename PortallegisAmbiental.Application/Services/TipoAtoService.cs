using AutoMapper;
using FluentValidation;
using PortalLegisAmbiental.Application.Services.Interfaces;
using PortalLegisAmbiental.Domain.Dtos.Requests;
using PortalLegisAmbiental.Domain.Dtos.Responses;
using PortalLegisAmbiental.Domain.Entities;
using PortalLegisAmbiental.Domain.Exceptions;
using PortalLegisAmbiental.Domain.IRepositories;
using System.Net;

namespace PortalLegisAmbiental.Application.Services
{
    public class TipoAtoService : ITipoAtoService
    {
        private readonly ITipoAtoRepository _tipoAtoRepository;
        private readonly IValidator<AddTipoAtoRequest> _addValidator;
        private readonly IValidator<TipoAtoRequest> _updateValidator;
        private readonly IMapper _mapper;

        public TipoAtoService(ITipoAtoRepository tipoAtoRepository, IMapper mapper, 
            IValidator<AddTipoAtoRequest> addValidator, IValidator<TipoAtoRequest> updateValidator)
        {
            _tipoAtoRepository = tipoAtoRepository;
            _mapper = mapper;
            _addValidator = addValidator;
            _updateValidator = updateValidator;
        }

        public async Task Add(AddTipoAtoRequest tipoAtoRequest)
        {
            _addValidator.ValidateAndThrow(tipoAtoRequest);

            var tipoAto = _mapper.Map<TipoAto>(tipoAtoRequest);
            var exists = await _tipoAtoRepository.Exists(tipoAto);

            if (exists)
                throw new PortalLegisDomainException(
                    "ALREADY_REGISTRED", "Tipo de Ato já cadastrado.",
                    HttpStatusCode.Conflict);

            await _tipoAtoRepository.Add(tipoAto);
            _tipoAtoRepository.UnitOfWork.SaveChanges();
        }

        public async Task<List<TipoAtoResponse>> SearchByName(string? name)
        {
            if (name == null) name = string.Empty;
            var tiposAtos = await _tipoAtoRepository.SearchByName(name, true);
            return _mapper.Map<List<TipoAtoResponse>>(tiposAtos);
        }

        public async Task<TipoAtoResponse> GetById(ulong id)
        {
            var tipoAto = await _tipoAtoRepository.GetById(id, true);
            
            if (tipoAto == null)
                throw new PortalLegisDomainException(
                    "KEY_NOT_FOUND", "Id não encontrado.",
                    HttpStatusCode.NotFound);

            return _mapper.Map<TipoAtoResponse>(tipoAto);
        }

        public async Task Update(TipoAtoRequest tipoAtoRequest)
        {
            _updateValidator.ValidateAndThrow(tipoAtoRequest);

            var tipoAto = await _tipoAtoRepository.GetById(tipoAtoRequest.Id);

            if (tipoAto == null)
                throw new PortalLegisDomainException(
                    "KEY_NOT_FOUND", "Id não encontrado.",
                    HttpStatusCode.NotFound);

            tipoAto.UpdateName(tipoAtoRequest.Nome);
            
            var exists = await _tipoAtoRepository.Exists(tipoAto);
            if (exists)
                throw new PortalLegisDomainException(
                    "ALREADY_REGISTRED", "Tipo de Ato já cadastrado.",
                    HttpStatusCode.Conflict);

            _tipoAtoRepository.UnitOfWork.SaveChanges();
        }

        public async Task Disable(ulong id)
        {
            var tipoAto = await _tipoAtoRepository.GetById(id);

            if (tipoAto == null)
                throw new PortalLegisDomainException(
                    "KEY_NOT_FOUND", "Id não encontrado.",
                    HttpStatusCode.NotFound);

            tipoAto.Disable();
            _tipoAtoRepository.UnitOfWork.SaveChanges();
        }
    }
}
