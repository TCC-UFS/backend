using AutoMapper;
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
        }
    }
}
