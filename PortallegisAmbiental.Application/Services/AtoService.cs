
using AutoMapper;
using AutoMapper.Execution;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
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
    public class AtoService : IAtoService
    {
        private readonly IAtoRepository _atoRepository;
        private readonly ITipoAtoRepository _tipoAtoRepository;
        private readonly IJurisdicaoRepository _jurisdicaoRepository;
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IValidator<AddAtoRequest> _addValidator;
        private readonly IValidator<UpdateAtoRequest> _updateValidator;
        private readonly IMapper _mapper;

        public AtoService(IAtoRepository atoRepository, ITipoAtoRepository tipoAtoRepository,
            IJurisdicaoRepository jurisdicaoRepository, IUsuarioRepository usuarioRepository,
            IValidator<AddAtoRequest> addValidator, IValidator<UpdateAtoRequest> updateValidator,
            IMapper mapper)
        {
            _atoRepository = atoRepository;
            _tipoAtoRepository = tipoAtoRepository;
            _jurisdicaoRepository = jurisdicaoRepository;
            _usuarioRepository = usuarioRepository;
            _addValidator = addValidator;
            _updateValidator = updateValidator;
            _mapper = mapper;
        }

        public async Task<AtoResponse> Add(AddAtoRequest atoRequest)
        {
            _addValidator.ValidateAndThrow(atoRequest);

            var tipoAto = await _tipoAtoRepository.Exists(atoRequest.TipoAtoId);
            if (!tipoAto)
                throw new PortalLegisDomainException(
                    "TYPE_NOT_FOUND", "Tipo do ato não encontrado.",
                    HttpStatusCode.NotFound);

            var jurisdicao = await _jurisdicaoRepository.Exists(atoRequest.JurisdicaoId);
            if (!jurisdicao)
                throw new PortalLegisDomainException(
                    "JURISDICTION_NOT_FOUND", "Jurisdição não encontrada.",
                    HttpStatusCode.NotFound);

            var createdBy = await _usuarioRepository.Exists(atoRequest.CreatedById);
            if (!createdBy)
                throw new PortalLegisDomainException(
                    "USER_NOT_FOUND", "Usuário não encontrado.",
                    HttpStatusCode.NotFound);


            var ato = _mapper.Map<Ato>(atoRequest);

            var exists = await _atoRepository.Exists(ato);
            if (exists)
                throw new PortalLegisDomainException(
                    "ALREADY_REGISTRED", "Ato já cadastrado.",
                    HttpStatusCode.NotFound);

            var response = await _atoRepository.Add(ato);
            //if (ato.PossuiConteudo || ato.PossuiHtml)
            //{
            // TODO: ADD TO ELASTICSEARCH
            //}

            _atoRepository.UnitOfWork.SaveChanges();
            return _mapper.Map<AtoResponse>(response);
        }

        public async Task<List<AtoResponse>> GetAll()
        {
            var atos = await _atoRepository.GetAll(includeTipo: true, includeJurisdicao: true);
            return _mapper.Map<List<AtoResponse>>(atos);
        }

        public async Task<AtoResponse?> GetById(ulong id)
        {
            var ato = await _atoRepository.GetById(id, includeTipo: true, includeJurisdicao: true, includeCreated: true);
            return _mapper.Map<AtoResponse?>(ato);
        }

        public async Task<AtoResponse?> GetByNumber(string number)
        {
            var ato = await _atoRepository.GetByNumber(number, includeTipo: true, includeJurisdicao: true, includeCreated: true);
            return _mapper.Map<AtoResponse?>(ato);
        }

        public async Task<List<AtoResponse>> Search(string? numero = null, string? tipo = null, string? jurisdicao = null, string order = "desc")
        {
            List<Ato> atos;
            var isEnum = Enum.TryParse<EAmbitoType>(jurisdicao, true, out var ambito);
            if (isEnum)
                atos = await _atoRepository.Search(numero: numero, tipo: tipo, ambito: ambito, includeTipo: true, includeJurisdicao: true, order: order);
            else
                atos = await _atoRepository.Search(numero: numero, tipo: tipo, jurisdicao: jurisdicao, includeTipo: true, includeJurisdicao: true, order: order);

            return _mapper.Map<List<AtoResponse>>(atos);
        }

        public async Task<List<AtoResponse>> SearchByNumber(string number)
        {
            var atos = await _atoRepository.SearchByNumber(number, includeTipo: true, includeJurisdicao: true);
            return _mapper.Map<List<AtoResponse>>(atos);
        }

        public async Task<List<AtoResponse>> SearchByJurisdicao(string jurisdicao)
        {
            List<Ato> atos;
            var isEnum = Enum.TryParse<EAmbitoType>(jurisdicao, true, out var ambito);
            if (isEnum)
                atos = await _atoRepository.SearchByJurisdicao(ambito, includeTipo: true);
            else
                atos = await _atoRepository.SearchByJurisdicao(jurisdicao, includeTipo: true);

            return _mapper.Map<List<AtoResponse>>(atos);
        }

        public async Task<List<AtoResponse>> SearchByTipo(string tipoAto)
        {
            var atos = await _atoRepository.SearchByTipo(tipoAto, includeJurisdicao: true);
            return _mapper.Map<List<AtoResponse>>(atos);
        }

        public async Task<AtoResponse> Update(UpdateAtoRequest atoRequest)
        {
            _updateValidator.ValidateAndThrow(atoRequest);

            var ato = await _atoRepository.GetById(atoRequest.Id, tracking: true);
            if (ato == null)
                throw new PortalLegisDomainException(
                    "KEY_NOT_FOUND", "Id não encontrado.",
                    HttpStatusCode.NotFound);

            ato.UpdateNumber(atoRequest.Numero);
            ato.UpdateFilePath(atoRequest.CaminhoArquivo);
            ato.UpdateDate(atoRequest.DataAto);
            ato.UpdatePublishDate(atoRequest.DataPublicacao);
            ato.UpdateEmenta(atoRequest.Ementa);

            if (atoRequest.Disponivel)
                ato.Publish();
            else
                ato.Unpublish();

            if (!string.IsNullOrEmpty(atoRequest.Conteudo))
            {
                ato.HasContent();
            }

            if (!string.IsNullOrEmpty(atoRequest.Html))
            {
                ato.HasHtml();
            }

            if (atoRequest.TipoAtoId != 0)
            {
                var tipoAto = await _tipoAtoRepository.Exists(atoRequest.TipoAtoId);
                if (!tipoAto)
                    throw new PortalLegisDomainException(
                        "TYPE_NOT_FOUND", "Tipo do ato não encontrado.",
                        HttpStatusCode.NotFound);

                ato.SetTipoAtoId(atoRequest.TipoAtoId);
            }

            if (atoRequest.JurisdicaoId != 0)
            {
                var jurisdicao = await _jurisdicaoRepository.Exists(atoRequest.JurisdicaoId);
                if (!jurisdicao)
                    throw new PortalLegisDomainException(
                        "JURISDICTION_NOT_FOUND", "Jurisdição não encontrada.",
                        HttpStatusCode.NotFound);

                ato.SetJurisdicaoId(atoRequest.JurisdicaoId);
            }

            if (atoRequest.TipoAtoId != 0 || !string.IsNullOrEmpty(atoRequest.Numero))
            {
                var exists = await _atoRepository.Exists(ato);
                if (exists)
                    throw new PortalLegisDomainException(
                        "ALREADY_REGISTRED", "Ato já cadastrado.",
                        HttpStatusCode.NotFound);
            }

            _atoRepository.UnitOfWork.SaveChanges();
            return _mapper.Map<AtoResponse>(ato);
        }

        public async Task Disable(ulong id)
        {
            var ato = await _atoRepository.GetById(id);
            if (ato == null)
                throw new PortalLegisDomainException(
                    "KEY_NOT_FOUND", "Id não encontrado.",
                    HttpStatusCode.NotFound);

            ato.Disable();
            _atoRepository.UnitOfWork.SaveChanges();
        }
    }
}
