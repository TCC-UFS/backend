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
    public class JurisdicaoService : IJurisdicaoService
    {
        private readonly IJurisdicaoRepository _jurisdicaoRepository;
        private readonly IValidator<AddJurisdicaoRequest> _addValidator;
        private readonly IValidator<UpdateJurisdicaoRequest> _updateValidator;
        private readonly IMapper _mapper;

        public JurisdicaoService(IJurisdicaoRepository jurisdicaoRepository, IMapper mapper, 
            IValidator<AddJurisdicaoRequest> addValidator, IValidator<UpdateJurisdicaoRequest> updateValidator)
        {
            _jurisdicaoRepository = jurisdicaoRepository;
            _mapper = mapper;
            _addValidator = addValidator;
            _updateValidator = updateValidator;
        }

        public async Task Add(AddJurisdicaoRequest jurisdicaoRequest)
        {
            _addValidator.ValidateAndThrow(jurisdicaoRequest);

            var jurisdicao = _mapper.Map<Jurisdicao>(jurisdicaoRequest);
            if (jurisdicao.Ambito.Equals(EAmbitoType.Federal) && jurisdicao.Estado != null)
                throw new PortalLegisDomainException(
                    "INVALID_OPERATION", "Ambito Federal não possui estado.",
                    HttpStatusCode.UnprocessableEntity);

            var exists = await _jurisdicaoRepository.Exists(jurisdicao);
            if (exists)
                throw new PortalLegisDomainException(
                    "ALREADY_REGISTRED", "Jurisdicao já cadastrada.",
                    HttpStatusCode.Conflict);

            await _jurisdicaoRepository.Add(jurisdicao);
            _jurisdicaoRepository.UnitOfWork.SaveChanges();
        }

        public async Task<List<JurisdicaoResponse>> GetAll()
        {
            var jurisdicoes = await _jurisdicaoRepository.GetAll();
            return _mapper.Map<List<JurisdicaoResponse>>(jurisdicoes);
        }

        public async Task<List<JurisdicaoResponse>> Search(string? state, string? sigla, string? ambito, string order)
        {
            var isEnum = Enum.TryParse<EAmbitoType>(ambito, true, out var ambitoEnum);
            if (!isEnum && !string.IsNullOrEmpty(ambito))
                throw new PortalLegisDomainException(
                    "BAD_REQUEST", $"{ambito} não é um ambito válido.");

            List<Jurisdicao> jurisdicoes;
            if (isEnum)
                jurisdicoes = await _jurisdicaoRepository.Search(state, sigla, ambitoEnum, order, true);
            else
                jurisdicoes = await _jurisdicaoRepository.Search(state, sigla, null, order, true);
            
            return _mapper.Map<List<JurisdicaoResponse>>(jurisdicoes);
        }

        public async Task<List<JurisdicaoDropdownResponse>> GetDropdown()
        {
            var jurisdicoes = await _jurisdicaoRepository.Search();
            return _mapper.Map<List<JurisdicaoDropdownResponse>>(jurisdicoes);
        }

        public async Task<JurisdicaoResponse> GetById(ulong id)
        {
            var jurisdicao = await _jurisdicaoRepository.GetById(id, true);
            
            if (jurisdicao == null)
                throw new PortalLegisDomainException(
                    "KEY_NOT_FOUND", "Id não encontrado.",
                    HttpStatusCode.NotFound);

            return _mapper.Map<JurisdicaoResponse>(jurisdicao);
        }

        public async Task Update(UpdateJurisdicaoRequest jurisdicaoRequest)
        {
            _updateValidator.ValidateAndThrow(jurisdicaoRequest);

            var jurisdicao = await _jurisdicaoRepository.GetById(jurisdicaoRequest.Id);

            if (jurisdicao == null)
                throw new PortalLegisDomainException(
                    "KEY_NOT_FOUND", "Id não encontrado.",
                    HttpStatusCode.NotFound);

            if (!string.IsNullOrEmpty(jurisdicaoRequest.Ambito))
            {
                var ambito = Enum.Parse<EAmbitoType>(jurisdicaoRequest.Ambito);
                if (ambito.Equals(EAmbitoType.Federal) && jurisdicao.Estado != null)
                    throw new PortalLegisDomainException(
                        "INVALID_OPERATION", "Ambito Federal não possui estado.",
                        HttpStatusCode.UnprocessableEntity);

                jurisdicao.UpdateAmbito(ambito);
            }

            if (jurisdicaoRequest.Estado != null && jurisdicao.Ambito.Equals(EAmbitoType.Federal))
                throw new PortalLegisDomainException(
                    "INVALID_OPERATION", "Ambito Federal não possui estado.",
                    HttpStatusCode.UnprocessableEntity);

            jurisdicao.UpdateState(jurisdicaoRequest.Estado);
            jurisdicao.UpdateSigla(jurisdicaoRequest.Sigla);
            
            var exists = await _jurisdicaoRepository.Exists(jurisdicao);
            if (exists)
                throw new PortalLegisDomainException(
                    "ALREADY_REGISTRED", "Jurisdicao já cadastrada.",
                    HttpStatusCode.Conflict);

            _jurisdicaoRepository.UnitOfWork.SaveChanges();
        }

        public async Task Disable(ulong id)
        {
            var jurisdicao = await _jurisdicaoRepository.GetById(id);

            if (jurisdicao == null)
                throw new PortalLegisDomainException(
                    "KEY_NOT_FOUND", "Id não encontrado.",
                    HttpStatusCode.NotFound);

            await _jurisdicaoRepository.Disable(jurisdicao);
            _jurisdicaoRepository.UnitOfWork.SaveChanges();
        }
    }
}
