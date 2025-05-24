using Moq;
using Questao2.Application.Services;
using Questao2.Domain.Interfaces;
using Questao2.Shared.Dtos;
using Xunit;
using DomainMatch = Questao2.Domain.Entities.Match;

namespace Questao2.Tests
{
    public class GoalServiceTests
    {
        [Fact]
        public async Task GetTotalScoredGoalsAsync_ReturnsCorrectTotalGoals()
        {
            var mockApiService = new Mock<IFootballApiService>();
            var team = "Chelsea";
            var year = 2014;

            mockApiService.Setup(x => x.GetMatchesAsync(team, year, "team1", 1))
                .ReturnsAsync(new ApiResponse
                {
                    Total_Pages = 2,
                    Data = new DomainMatch[]
                    {
                        new DomainMatch { Team1Goals = "2" },
                        new DomainMatch { Team1Goals = "3" }
                    }
                });

            mockApiService.Setup(x => x.GetMatchesAsync(team, year, "team1", 2))
                .ReturnsAsync(new ApiResponse
                {
                    Total_Pages = 2,
                    Data = new DomainMatch[]
                    {
                        new DomainMatch { Team1Goals = "1" }
                    }
                });

            mockApiService.Setup(x => x.GetMatchesAsync(team, year, "team2", 1))
                .ReturnsAsync(new ApiResponse
                {
                    Total_Pages = 1,
                    Data = new DomainMatch[]
                    {
                        new DomainMatch { Team2Goals = "2" },
                        new DomainMatch { Team2Goals = "4" }
                    }
                });

            var goalService = new GoalService(mockApiService.Object);

            var totalGoals = await goalService.GetTotalScoredGoalsAsync(team, year);

            Assert.Equal(12, totalGoals);
        }
    }
}