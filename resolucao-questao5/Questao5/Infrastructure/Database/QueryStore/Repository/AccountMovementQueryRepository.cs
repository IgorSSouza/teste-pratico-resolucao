using Dapper;
using Questao5.Domain.Repositories;
using System.Data;

namespace Questao5.Infrastructure.Database.QueryStore.Repository
{
    public class AccountMovementQueryRepository : IAccountMovementQueryRepository
    {
        private readonly IDbConnection _connection;

        public AccountMovementQueryRepository(IDbConnection connection)
        {
            _connection = connection;
        }
        public async Task<Guid> InsertAccountMovement(string idContaCorrente, string tipoMovimento, decimal valor)
        {
            var idMovimento = Guid.NewGuid();
            var data = DateTime.Now.ToString("dd/MM/yyyy");

            await _connection.ExecuteAsync(@"
            INSERT INTO movimento(idmovimento, idcontacorrente, datamovimento, tipomovimento, valor)
            VALUES(@id, @idConta, @data, @tipo, @valor)",
                new
                {
                    id = idMovimento,
                    idConta = idContaCorrente,
                    data,
                    tipo = tipoMovimento,
                    valor
                });

            return idMovimento;
        }
    }
}
