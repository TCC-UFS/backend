using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Hosting;
using PortalLegisAmbiental.Application.Services.Interfaces;
using PortalLegisAmbiental.Domain.Dtos;
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
        private readonly ISearchRepository _elasticRepository;
        private readonly ITipoAtoRepository _tipoAtoRepository;
        private readonly IJurisdicaoRepository _jurisdicaoRepository;
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IValidator<AddAtoRequest> _addValidator;
        private readonly IValidator<UpdateAtoRequest> _updateValidator;
        private readonly IWebHostEnvironment _environment;
        private readonly IMapper _mapper;

        public AtoService(IAtoRepository atoRepository, ITipoAtoRepository tipoAtoRepository,
            IJurisdicaoRepository jurisdicaoRepository, IUsuarioRepository usuarioRepository,
            IValidator<AddAtoRequest> addValidator, IValidator<UpdateAtoRequest> updateValidator,
            IMapper mapper, ISearchRepository elasticRepository, IWebHostEnvironment environment)
        {
            _atoRepository = atoRepository;
            _elasticRepository = elasticRepository;
            _tipoAtoRepository = tipoAtoRepository;
            _jurisdicaoRepository = jurisdicaoRepository;
            _usuarioRepository = usuarioRepository;
            _addValidator = addValidator;
            _updateValidator = updateValidator;
            _environment = environment;
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

            if (atoRequest.File != null)
            {
                string jur = "";
                string tipo = "";
                if (ato.TipoAtoId == 1) // Lei Ordinária
                    tipo = "LO";
                else if (ato.TipoAtoId == 2) // Lei Complementar
                    tipo = "LC";

                if (ato.JurisdicaoId == 1) // Brasil
                    jur = "BR";
                else if (ato.JurisdicaoId == 2) // Sergipe
                    jur = "SE";
                else if (ato.JurisdicaoId == 3) // Amazonas
                    jur = "AM";
                else jur = "DEFAULT";

                var fileName = $"{jur}/{tipo}/{atoRequest.File.FileName}";
                if (!fileName.EndsWith(".pdf"))
                    fileName += ".pdf";

                var filePath = Path.Combine(_environment.WebRootPath, $"{fileName}");
                var dirJurPath = Path.Combine(_environment.WebRootPath, $"{jur}");
                var dirTypePath = Path.Combine(_environment.WebRootPath, $"{jur}/{tipo}");
                
                if (!Directory.Exists(dirJurPath))
                    Directory.CreateDirectory(dirJurPath);

                if (!Directory.Exists(dirTypePath))
                    Directory.CreateDirectory(dirTypePath);

                using (var fileSteam = new FileStream(filePath, FileMode.Create))
                {    
                    await atoRequest.File.CopyToAsync(fileSteam);
                }

                ato.SetCaminhoArquivo(fileName);
            }

            var transaction = _atoRepository.UnitOfWork.BeginTransaction();

            await _atoRepository.Add(ato);
            _atoRepository.UnitOfWork.SaveChanges();

            if (ato.PossuiConteudo || ato.PossuiHtml)
            {
                var jur = await _jurisdicaoRepository.GetById(atoRequest.JurisdicaoId, true);
                var tpAto = await _tipoAtoRepository.GetById(atoRequest.TipoAtoId, true);

                string? tipoAtoStr = string.Empty;
                if (tpAto != null)
                    tipoAtoStr = tpAto.Nome.ToLower().Replace(" ", "_")?.Replace("-", "_");

                string jurStr = string.Empty;
                if (jur != null)
                    jurStr = jur.Ambito.ToString();

                ElasticDto.Data elasticData = new()
                {
                    IdAto = ato.Id,
                    //Numero = ato.Numero,
                    //Ementa = ato.Ementa,
                    Conteudo = atoRequest.Conteudo ?? string.Empty,
                    Html = atoRequest.Html ?? string.Empty,
                    Ambito = jurStr,
                    Jurisdicao = jur?.Sigla ?? string.Empty,
                    TipoAto = tipoAtoStr ?? string.Empty,
                    //DataAto = ato.DataAto,
                    //DataPublicacao = ato.DataPublicacao == default ? ato.DataAto : ato.DataPublicacao,
                    Disponivel = ato.Disponivel
                };

                try
                {
                    await _elasticRepository.AddOrUpdate(elasticData);
                }
                catch (Exception)
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            }

            await transaction.CommitAsync();

            return _mapper.Map<AtoResponse>(ato);
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

            if (atoRequest.Disponivel.HasValue && atoRequest.Disponivel.Value)
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

            if (atoRequest.File != null)
            {
                string jur = "";
                string tipo = "";
                if (ato.TipoAtoId == 1) // Lei Ordinária
                    tipo = "LO";
                else if (ato.TipoAtoId == 2) // Lei Complementar
                    tipo = "LC";

                if (ato.JurisdicaoId == 1) // Brasil
                    jur = "BR";
                else if (ato.JurisdicaoId == 2) // Sergipe
                    jur = "SE";
                else if (ato.JurisdicaoId == 3) // Amazonas
                    jur = "AM";
                else jur = "DEFAULT";

                var fileName = $"{jur}/{tipo}/{atoRequest.File.FileName}";
                if (!fileName.EndsWith(".pdf"))
                    fileName += ".pdf";

                var filePath = Path.Combine(_environment.WebRootPath, $"{fileName}");
                var dirJurPath = Path.Combine(_environment.WebRootPath, $"{jur}");
                var dirTypePath = Path.Combine(_environment.WebRootPath, $"{jur}/{tipo}");
                    
                if (!Directory.Exists(dirJurPath))
                        Directory.CreateDirectory(dirJurPath);

                if (!Directory.Exists(dirTypePath))
                        Directory.CreateDirectory(dirTypePath);

                using (var fileSteam = new FileStream(filePath, FileMode.Create))
                {
                    await atoRequest.File.CopyToAsync(fileSteam);
                }

                ato.SetCaminhoArquivo(fileName);
            }

            if (ato.PossuiConteudo || ato.PossuiHtml)
            {
                var jur = await _jurisdicaoRepository.GetById(ato.JurisdicaoId, true);
                var tpAto = await _tipoAtoRepository.GetById(ato.TipoAtoId, true);

                string? tipoAtoStr = string.Empty;
                if (tpAto != null)
                    tipoAtoStr = tpAto.Nome.ToLower().Replace(" ", "_")?.Replace("-", "_");

                string jurStr = string.Empty;
                if (jur != null)
                    jurStr = jur.Ambito.ToString();

                var searchRequest = new ElasticDto.Search();
                searchRequest.BaseQuery = new();
                searchRequest.BaseQuery.Size = 1;
                searchRequest.BaseQuery.Query = new();
                searchRequest.BaseQuery.Query.Bool = new();
                searchRequest.BaseQuery.Query.Bool.Must.Add(
                    new {
                        match = new
                        {
                            idAto = new
                            {
                                query = ato.Id
                            }
                        }
                    });
                var contents = await _elasticRepository.Search(searchRequest);
                var content = contents?.Hits.Hits.Select(hit => hit.data).FirstOrDefault();

                ElasticDto.Data elasticData = new()
                {
                    IdAto = ato.Id,
                    //Numero = ato.Numero,
                    //Ementa = ato.Ementa,
                    Conteudo = atoRequest.Conteudo ?? content?.Conteudo ?? string.Empty,
                    Html = atoRequest.Html ?? content?.Html ?? string.Empty,
                    Ambito = jurStr,
                    Jurisdicao = jur?.Sigla ?? string.Empty,
                    TipoAto = tipoAtoStr ?? string.Empty,
                    //DataAto = ato.DataAto,
                    //DataPublicacao = ato.DataPublicacao == default ? ato.DataAto : ato.DataPublicacao,
                    Disponivel = ato.Disponivel
                };

                await _elasticRepository.AddOrUpdate(elasticData);
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
