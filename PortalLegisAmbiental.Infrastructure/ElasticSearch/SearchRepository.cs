using PortalLegisAmbiental.Domain.IRepositories;

namespace PortalLegisAmbiental.Infrastructure.ElasticSearch
{
    public class SearchRepository : ISearchRepository
    {
        private readonly HttpClient _httpClient;

        public SearchRepository(IHttpClientFactory httpClientFactory)
        {
            try
            {
                _httpClient = httpClientFactory.CreateClient("Elastic");
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.InnerException?.Message);
                throw;
            }
        }

        public async Task<string> Teste()
        {
            var response = await _httpClient.GetAsync("basic/_search");
            return await response.Content.ReadAsStringAsync();
        }
    }
}
