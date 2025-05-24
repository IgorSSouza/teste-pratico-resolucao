using Questao2.Application.Services;
using Questao2.Domain.Interfaces;
using Questao2.Infrastructure.Services;

public class Program
{
    public static async Task Main()
    {
        var httpClient = new HttpClient();
        IFootballApiService apiService = new FootballApiService(httpClient);
        var goalService = new GoalService(apiService);

        await getTotalScoredGoals(goalService, "Paris Saint-Germain", 2013);
        await getTotalScoredGoals(goalService, "Chelsea", 2014);
    }
    private static async Task getTotalScoredGoals(GoalService service, string teamName, int year)
    {
        var totalGoals = await service.GetTotalScoredGoalsAsync(teamName, year);
        Console.WriteLine($"Team {teamName} scored {totalGoals} goals in {year}");
    }
}
