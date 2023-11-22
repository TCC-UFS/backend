using PortalLegisAmbiental.Domain.Dtos;
using PortalLegisAmbiental.Domain.Exceptions;
using PortalLegisAmbiental.Domain.IRepositories;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

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

        public async Task<ElasticDto.ReadResponse?> Search(ElasticDto.Search searchData)
        {
            string index;
            if (searchData.Jurisdicao == null) index = "/";
            else index = searchData.Jurisdicao.ToLower() + "/";

            if (searchData.Query != null)
            {
                var content = JsonSerializer.Serialize(searchData.Query, new JsonSerializerOptions()
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
                });
                var strContent = new StringContent(content, Encoding.UTF8, "application/json");

                var uri = _httpClient.BaseAddress + $"{index}_search";
                var httpRequest = new HttpRequestMessage()
                {
                    Method = HttpMethod.Get,
                    RequestUri = new Uri(uri),
                    Content = strContent
                };
                var response = await _httpClient.SendAsync(httpRequest);
                var strResponse = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                    throw new PortalLegisDomainException(
                    "ELASTIC_SEARCH_ERROR",
                    strResponse, System.Net.HttpStatusCode.InternalServerError);

                return Newtonsoft.Json.JsonConvert.DeserializeObject<ElasticDto.ReadResponse>(strResponse);
            }
            else
            {
                var response = await _httpClient.GetAsync($"{index}_search");
                var strResponse = await response.Content.ReadAsStringAsync();
                
                if (!response.IsSuccessStatusCode)
                    throw new PortalLegisDomainException(
                    "ELASTIC_SEARCH_ERROR",
                    strResponse, System.Net.HttpStatusCode.InternalServerError);

                return JsonSerializer.Deserialize<ElasticDto.ReadResponse>(strResponse);
            }
        }

        public async Task Add(ElasticDto.Data elasticData)
        {
            var index = elasticData.Jurisdicao.ToLower();
            var content = JsonSerializer.Serialize(elasticData, new JsonSerializerOptions()
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
            });
            var strContent = new StringContent(content, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync($"{index}/_doc/{elasticData.IdAto}", strContent);
            if (!response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                throw new PortalLegisDomainException(
                    "FAILED_TO_INSERT_ELASTIC",
                    result, System.Net.HttpStatusCode.InternalServerError);
            }
        }

        public async Task AddOrUpdate(ElasticDto.Data elasticData)
        {
            var index = elasticData.Jurisdicao.ToLower();
            elasticData.TipoAto = elasticData.TipoAto.ToLower().Replace(" ", "_");
            var content = JsonSerializer.Serialize(elasticData, new JsonSerializerOptions()
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
            });
            var strContent = new StringContent(content, Encoding.UTF8, "application/json");

            var response = await _httpClient.PutAsync($"{index}/_doc/{elasticData.IdAto}", strContent);
            if (!response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                throw new PortalLegisDomainException(
                    "FAILED_TO_INSERT_ELASTIC",
                    result, System.Net.HttpStatusCode.InternalServerError);
            }
        }

        public async Task Delete(ulong idAto, string jurisdicao)
        {
            var index = jurisdicao.ToLower();
            var response = await _httpClient.DeleteAsync($"{index}/_doc/{idAto}");

            if (!response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                throw new PortalLegisDomainException(
                    "FAILED_TO_INSERT_ELASTIC",
                    result, System.Net.HttpStatusCode.InternalServerError);
            }
        }
    }
}
