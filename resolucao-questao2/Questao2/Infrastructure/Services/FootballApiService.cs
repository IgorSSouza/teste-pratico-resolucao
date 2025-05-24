using Newtonsoft.Json;
using Questao2.Domain.Interfaces;
using Questao2.Shared.Dtos;

namespace Questao2.Infrastructure.Services
{
    public class FootballApiService : IFootballApiService
    {
        private readonly HttpClient _httpClient;

        public FootballApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<ApiResponse> GetMatchesAsync(string team, int year, string teamParam, int page)
        {
            var url = $"https://jsonmock.hackerrank.com/api/football_matches?year={year}&{teamParam}={Uri.EscapeDataString(team)}&page={page}";
            var response = await _httpClient.GetAsync(url);
            var content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<ApiResponse>(content);
        }
    }
}
