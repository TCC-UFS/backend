
using AutoMapper;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PortalLegisAmbiental.Application.Services.Interfaces;
using PortalLegisAmbiental.Domain.Dtos;
using PortalLegisAmbiental.Domain.Dtos.Requests;
using PortalLegisAmbiental.Domain.Dtos.Responses;
using PortalLegisAmbiental.Domain.Exceptions;
using PortalLegisAmbiental.Domain.IRepositories;
using System.Collections.Generic;
using System.Globalization;

namespace PortalLegisAmbiental.Application.Services
{
    public class SearchService : IElasticService
    {
        private readonly ISearchRepository _searchRepository;
        private readonly IJurisdicaoRepository _jurisdicaoRepository;
        private readonly ITipoAtoRepository _tipoAtoRepository;
        private readonly IMapper _mapper;

        public SearchService(
            ISearchRepository searchRepository,
            IJurisdicaoRepository jurisdicaoRepository,
            ITipoAtoRepository tipoAtoRepository,
            IMapper mapper)
        {
            _searchRepository = searchRepository;
            _jurisdicaoRepository = jurisdicaoRepository;
            _tipoAtoRepository = tipoAtoRepository;
            _mapper = mapper;
        }

        public async Task<SearchResponse> Search(SearchRequest request, int page, int limit)
        {
            if (page <= 0 || limit <= 0 || limit > 500)
            {
                throw new PortalLegisDomainException("INVALID_PAGINATION",
                    "Dados de paginação inválidos. O tamanho máximo de resultados por página é de 500 resultados.");
            }

            var baseSearch = new ElasticDto.Search
            {
                Jurisdicao = null,
                BaseQuery = new()
                {
                    From = (page-1)*limit,
                    Size = limit,
                    Sort = new List<object>
                    {
                        "_score"
                    },
                    Query = new()
                    {
                        Bool = new()
                        {
                            Must = new List<object> { },
                            Filter = new()
                            {
                                new()
                                {
                                    Term = new
                                    {
                                        disponivel = true
                                    }
                                }
                            }
                        }
                    }
                }
            };

            if (!string.IsNullOrEmpty(request.Jurisdicao) || request.Ambito.HasValue)
            {
                var jurisdicao = await _jurisdicaoRepository.Search(sigla: request.Jurisdicao, ambito: request.Ambito, noTracking: true);
                if (jurisdicao.Count < 1)
                {
                    return new SearchResponse();
                }

                if (!string.IsNullOrEmpty(request.Jurisdicao))
                    baseSearch.Jurisdicao = request.Jurisdicao.Trim();

                if (request.Ambito.HasValue)
                    baseSearch.BaseQuery.Query.Bool.Filter.Add(new()
                    {
                        Term = new
                        {
                            ambito = request.Ambito.Value.ToString().ToLower()
                        }
                    });
            }

            if (!string.IsNullOrEmpty(request.Order) && request.Order != "rel")
            {
                baseSearch.BaseQuery.Sort = new List<object>
                {
                    new
                    {
                        dataPublicacao = new
                        {
                            order = request.Order
                        }
                    },
                    "_score"
                };
            }

            if (!string.IsNullOrEmpty(request.Termo))
            {
                if (request.Termo.Trim().Split(" ").Length == 1)
                {
                    var isNumber = int.TryParse(request.GrauAprox, out int grauAprox);
                    if (!string.IsNullOrEmpty(request.GrauAprox)
                        && !request.GrauAprox.ToLower().Equals("auto")
                        && (!isNumber || (grauAprox < 0 || grauAprox > 2)))
                    {
                        throw new PortalLegisDomainException("INVALID_FUZZINESS",
                            "O valor do grau de aproximação é inválido. Valores válidos: 0, 1, 2 ou 'auto'.");
                    }

                    baseSearch.BaseQuery.Query.Bool.Must.Add(new
                    {
                        match = new
                        {
                            conteudo = new
                            {
                                query = request.Termo.Trim(),
                                fuzziness = request.GrauAprox ?? "auto"
                            }
                        }
                    });
                }
                else
                {
                    baseSearch.BaseQuery.Query.Bool.Must.Add(new
                    {
                        match_phrase = new
                        {
                            conteudo = new
                            {
                                query = request.Termo.Trim()
                            }
                        }
                    });
                }
            }

            if (!string.IsNullOrEmpty(request.Numero))
            {
                var number = request.Numero.Trim();
                var isNumber = int.TryParse(number, out int numero);
                if (isNumber)
                {
                    number = numero.ToString("N", new CultureInfo("pt-BR"));
                }

                baseSearch.BaseQuery.Query.Bool.Must.Add(new
                {
                    match = new
                    {
                        numero = new
                        {
                            query = number
                        }
                    }
                });
            }

            if (!string.IsNullOrEmpty(request.Tipo))
            {
                var tipoAtoList = await _tipoAtoRepository.Search(request.Tipo, "desc", noTracking: true);
                var tipoAto = tipoAtoList.FirstOrDefault();
                if (tipoAto == null)
                {
                    return new SearchResponse();
                }

                baseSearch.BaseQuery.Query.Bool.Filter.Add(new()
                {
                    Term = new
                    {
                        tipoAto = tipoAto.Nome.ToLower().Replace(" ", "_")?.Replace("-", "_")
                    }
                });
            }

            var contents = await _searchRepository.Search(baseSearch, false);

            var response = _mapper.Map<SearchResponse>(contents);
            response.Pagination.Page = page;
            response.Pagination.Limit = limit;
            
            return response;
        }

        public async Task<SearchResponse> SearchById(ulong id)
        {
            var query = new ElasticDto.Search
            {
                BaseQuery = new()
                {
                    Query = new()
                    {
                        Bool = new()
                        {
                            Must = new List<object> {
                                new {
                                    match = new
                                    {
                                        idAto = new
                                        {
                                            query = id
                                        }
                                    }
                                }
                            },
                            Filter = new()
                            {
                                new()
                                {
                                    Term = new
                                    {
                                        disponivel = true
                                    }
                                }
                            }
                        }
                    }
                }
            };

            var contents = await _searchRepository.Search(query);

            var response = _mapper.Map<SearchResponse>(contents);

            return response;
        }
    }
}
