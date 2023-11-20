using PortalLegisAmbiental.Domain.IRepositories;

namespace PortalLegisAmbiental.Infrastructure.ElasticSearch
{
    public class SearchRepository : ISearchRepository
    {
        private readonly HttpClient _httpClient;

        public SearchRepository(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("Elastic");
        }

        public async Task<string> Teste()
        {
            var response = await _httpClient.GetAsync("basic/_search");
            return await response.Content.ReadAsStringAsync();
        }
    }
}
