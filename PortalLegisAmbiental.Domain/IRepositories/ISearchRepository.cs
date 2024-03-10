using PortalLegisAmbiental.Domain.Dtos;

namespace PortalLegisAmbiental.Domain.IRepositories
{
    public interface ISearchRepository
    {
        Task<ElasticDto.ReadResponse?> Search(ElasticDto.Search searchData, bool getContent = true);
        Task Add(ElasticDto.Data elasticData);
        Task AddOrUpdate(ElasticDto.Data elasticData);
        Task Delete(ulong idAto, string jurisdicao);
    }
}
