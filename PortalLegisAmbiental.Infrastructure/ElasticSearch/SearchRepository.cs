using PortalLegisAmbiental.Domain.Dtos;
using PortalLegisAmbiental.Domain.Exceptions;
using PortalLegisAmbiental.Domain.IRepositories;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace PortalLegisAmbiental.Infrastructure.ElasticSearch
{
    public class SearchRepository : ISearchRepository
    {
        private readonly HttpClient _httpClient;
        private readonly HttpClient _client;

        public SearchRepository(IHttpClientFactory httpClientFactory)
        {
            var uri = Environment.GetEnvironmentVariable("ELASTIC_URL");
            var authString = Environment.GetEnvironmentVariable("ELASTIC_TOKEN");

            var handler = new HttpClientHandler();
            handler.ClientCertificateOptions = ClientCertificateOption.Manual;
            handler.SslProtocols = System.Security.Authentication.SslProtocols.Tls12;
            var path = "/var/www/ssl/projetosufs.cloud/es2/cert.pem";

            if (File.Exists(path))
            {
                handler.ClientCertificates.Add(new X509Certificate2(path));
                handler.ServerCertificateCustomValidationCallback =
                (httpRequestMessage, cert, cetChain, policyErrors) =>
                {
                    return true;
                };
            }

            _client = new HttpClient(handler);
            if (string.IsNullOrEmpty(uri) || string.IsNullOrEmpty(authString))
                throw new NotSupportedException("Um erro interno ocorreu.");

            _client.BaseAddress = new Uri(uri);
            _client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", authString);
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

        public async Task<ElasticDto.ReadResponse?> Search(ElasticDto.Search searchData, bool getContent = true)
        {
            string index;
            if (searchData.Jurisdicao == null) index = "/";
            else index = searchData.Jurisdicao.ToLower() + "/";

            if (searchData.Query != null || searchData.BaseQuery != null)
            {
                string content = string.Empty;
                if (searchData.Query != null)
                {
                    content = JsonSerializer.Serialize(searchData.Query, new JsonSerializerOptions()
                    {
                        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
                    });
                }

                if (searchData.BaseQuery != null)
                {
                    if (!getContent)
                    {
                        searchData.BaseQuery._source = new List<string>
                        {
                            "idAto", "numero", "ementa", "tipoAto", "ambito",
                            "jurisdicao", "dataPublicacao", "dataAto", "disponivel"
                        };
                    }

                    content = JsonSerializer.Serialize(searchData.BaseQuery, new JsonSerializerOptions()
                    {
                        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
                    });
                }

                var strContent = new StringContent(content, Encoding.UTF8, "application/json");

                var uri = _client.BaseAddress + $"{index}_search";
                var httpRequest = new HttpRequestMessage()
                {
                    Method = HttpMethod.Get,
                    RequestUri = new Uri(uri),
                    Content = strContent
                };
                var response = await _client.SendAsync(httpRequest);
                var strResponse = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                    throw new PortalLegisDomainException(
                    "ELASTIC_SEARCH_ERROR",
                    strResponse, System.Net.HttpStatusCode.InternalServerError);

                return Newtonsoft.Json.JsonConvert.DeserializeObject<ElasticDto.ReadResponse>(strResponse);
            }
            else
            {
                var response = await _client.GetAsync($"{index}_search");
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

            var response = await _client.PostAsync($"{index}/_doc/{elasticData.IdAto}", strContent);
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

            var response = await _client.PutAsync($"{index}/_doc/{elasticData.IdAto}", strContent);
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
            var response = await _client.DeleteAsync($"{index}/_doc/{idAto}");

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
