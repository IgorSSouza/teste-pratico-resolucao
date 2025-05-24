using Questao5.Infrastructure.Database.QueryStore.Dtos;

namespace Questao5.Domain.Repositories
{
    public interface ICurrentAccountQueryRepository
    {
        Task<CurrentAccountDto?> ObterPorNumeroAsync(int numero);
        Task<decimal> ObterTotalCreditosAsync(string idContaCorrente);
        Task<decimal> ObterTotalDebitosAsync(string idContaCorrente);
    }
}
