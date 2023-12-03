using PortalLegisAmbiental.Domain.Dtos.Requests;
using PortalLegisAmbiental.Domain.Dtos.Responses;

namespace PortalLegisAmbiental.Application.Services.Interfaces
{
    public interface IElasticService
    {
        Task<SearchResponse> Search(SearchRequest request, int page, int limit);
    }
}
