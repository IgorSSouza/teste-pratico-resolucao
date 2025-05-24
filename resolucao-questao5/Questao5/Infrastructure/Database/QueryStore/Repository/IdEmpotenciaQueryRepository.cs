using Dapper;
using Questao5.Domain.Repositories;
using System.Data;
using System.Text.Json;

namespace Questao5.Infrastructure.Database.QueryStore.Repository
{
    public class IdEmpotenciaQueryRepository : IIdEmpotenciaQueryRepository
    {
        private readonly IDbConnection _connection;

        public IdEmpotenciaQueryRepository(IDbConnection connection)
        {
            _connection = connection;
        }

        public async Task<string?> GetResultIdEmpotenciaAsync(string chave)
        {
            return await _connection.QueryFirstOrDefaultAsync<string>(
                  "SELECT resultado FROM idempotencia WHERE chave_idempotencia = @chave", new { chave });
        }

        public async Task SaveResultIdempotenciaAsync<TRequest, TResponse>(string chave, TRequest requisicao, TResponse resposta)
        {
            await _connection.ExecuteAsync(
                @"INSERT INTO idempotencia(chave_idempotencia, requisicao, resultado)
              VALUES(@chave, @req, @res)",
                new
                {
                    chave,
                    req = JsonSerializer.Serialize(requisicao),
                    res = JsonSerializer.Serialize(resposta)
                });
        }
    }
}
