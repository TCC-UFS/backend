using AutoMapper;
using PortalLegisAmbiental.Domain.Dtos;
using PortalLegisAmbiental.Domain.Dtos.Requests;
using PortalLegisAmbiental.Domain.Dtos.Responses;
using PortalLegisAmbiental.Domain.Entities;

namespace PortalLegisAmbiental.Application.Mappers
{
    public class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<AddTipoAtoRequest, TipoAto>();
            CreateMap<TipoAto, TipoAtoResponse>();

            CreateMap<AddPermissaoRequest, Permissao>();
            CreateMap<Permissao, PermissaoResponse>();

            CreateMap<AddJurisdicaoRequest, Jurisdicao>();
            CreateMap<Jurisdicao, JurisdicaoResponse>();

            CreateMap<AddGrupoRequest, Grupo>();
            CreateMap<Grupo, GrupoResponse>();

            CreateMap<AddUsuarioRequest, Usuario>();
            CreateMap<Usuario, UsuarioResponse>();

            CreateMap<Ato, AtoResponse>();
            CreateMap<AddAtoRequest, Ato>()
                .ConstructUsing(atoRequest => new Ato(atoRequest));

            CreateMap<ElasticDto.ReadResponse, SearchResponse>()
                .ConstructUsing(read => toSearchResponse(read));
        }

        private SearchResponse toSearchResponse(ElasticDto.ReadResponse read)
        {
            var data = new List<SearchResponseData>();

            foreach (var hit in read.Hits.Hits.Select(hit => hit.data))
            {
                data.Add(new()
                {
                    Id = hit.IdAto,
                    Conteudo = hit.Html ?? hit.Conteudo,
                    Numero = hit.Numero,
                    Ementa = hit.Ementa,
                    DataPublicacao = hit.DataPublicacao,
                    DataAto = hit.DataAto,
                    Disponivel = hit.Disponivel,
                    Jurisdicao = hit.Jurisdicao,
                    TipoAto = hit.TipoAto
                });
            }

            return new SearchResponse
            {
                Pagination = new()
                {
                    CurrentCount = read.Hits.Hits.Count,
                    TotalCount = read.Hits.Total.Value
                },
                Data = data
            };
        }
    }
}
