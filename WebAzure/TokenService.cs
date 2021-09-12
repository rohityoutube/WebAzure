using Microsoft.Extensions.Configuration;
using Microsoft.Identity.Web;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace WebAzure
{
    public interface ITokenServce
    {
        public Task<string> Get();
    }
    public class TokenService : ITokenServce
    {
        private readonly HttpClient _httpClinet;
        private readonly string _APIScope = string.Empty;
        private readonly string _APIBaseAddress = string.Empty;
        private readonly ITokenAcquisition _tokenAcquistion;

        public TokenService(ITokenAcquisition tokenAcquisition , HttpClient httpClient , IConfiguration configuration)
        {
            _httpClinet = httpClient;
            _tokenAcquistion = tokenAcquisition;
            _APIScope = configuration["APIConfig:APIScope"];
            _APIBaseAddress = configuration["APIConfig:APIBaseaddress"];
        }

        public async Task<string> Get()
        {
            await FindToke();

            var response = await _httpClinet.GetAsync($"{_APIBaseAddress}/weatherforecast");
            if(response.StatusCode==System.Net.HttpStatusCode.OK)
            {
                var content = await response.Content.ReadAsStringAsync();
                var output = JsonConvert.DeserializeObject<string>(content);

                return output;
            }

            throw new HttpRequestException("Invalid Respinse");
        }

        private  async Task FindToke()
        {
            var accessToken = await _tokenAcquistion.GetAccessTokenForUserAsync(new[] { _APIScope });

            _httpClinet.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            _httpClinet.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
        }
    }
}
