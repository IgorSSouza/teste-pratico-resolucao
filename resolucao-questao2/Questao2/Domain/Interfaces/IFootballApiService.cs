using Questao2.Shared.Dtos;

namespace Questao2.Domain.Interfaces
{
    public interface IFootballApiService
    {
        Task<ApiResponse> GetMatchesAsync(string team, int year, string teamParam, int page);
    }
}
