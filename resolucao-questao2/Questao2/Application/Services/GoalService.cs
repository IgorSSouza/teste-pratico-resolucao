using Questao2.Domain.Interfaces;

namespace Questao2.Application.Services
{
    public class GoalService 
    {
        private readonly IFootballApiService _apiService;

        public GoalService(IFootballApiService apiService)
        {
            _apiService = apiService;
        }

        public async Task<int> GetTotalScoredGoalsAsync(string team, int year)
        {
            int totalGoals = 0;

            totalGoals += await GetGoalsByTeamRoleAsync(team, year, "team1", "team1goals");
            totalGoals += await GetGoalsByTeamRoleAsync(team, year, "team2", "team2goals");

            return totalGoals;
        }

        private async Task<int> GetGoalsByTeamRoleAsync(string team, int year, string teamParam, string goalsKey)
        {
            int totalGoals = 0;
            int page = 1;
            int totalPages;

            do
            {
                var response = await _apiService.GetMatchesAsync(team, year, teamParam, page);
                totalPages = response.Total_Pages;

                foreach (var match in response.Data)
                {
                    totalGoals += goalsKey switch
                    {
                        "team1goals" => int.Parse(match.Team1Goals),
                        "team2goals" => int.Parse(match.Team2Goals),
                        _ => throw new ArgumentException($"Invalid goalsKey: {goalsKey}")
                    };
                }

                page++;
            } while (page <= totalPages);

            return totalGoals;

        }
    }
}
