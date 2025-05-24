using Dapper;
using Questao5.Domain.Repositories;
using Questao5.Infrastructure.Database.QueryStore.Dtos;
using System.Data;

namespace Questao5.Infrastructure.Database.QueryStore.Repository
{
    public class CurrentAccountQueryRepository : ICurrentAccountQueryRepository
    {
        private readonly IDbConnection _connection;

        public CurrentAccountQueryRepository(IDbConnection connection)
        {
            _connection = connection;
        }

        public async Task<CurrentAccountDto?> ObterPorNumeroAsync(int numero)
        {
            return await _connection.QueryFirstOrDefaultAsync<CurrentAccountDto>(
                "SELECT idcontacorrente , numero, nome, ativo FROM contacorrente WHERE numero = @numero",
                new { numero });
        }

        public async Task<decimal> ObterTotalCreditosAsync(string idContaCorrente)
        {
            return await _connection.ExecuteScalarAsync<decimal>(
                "SELECT COALESCE(SUM(valor),0) FROM movimento WHERE idcontacorrente = @id AND tipomovimento = 'C'",
                new { id = idContaCorrente });
        }

        public async Task<decimal> ObterTotalDebitosAsync(string idContaCorrente)
        {
            return await _connection.ExecuteScalarAsync<decimal>(
                "SELECT COALESCE(SUM(valor),0) FROM movimento WHERE idcontacorrente = @id AND tipomovimento = 'D'",
                new { id = idContaCorrente });
        }
    }
}
