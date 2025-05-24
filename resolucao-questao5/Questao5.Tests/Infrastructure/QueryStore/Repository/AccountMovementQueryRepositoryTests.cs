using Dapper;
using Microsoft.Data.Sqlite;
using Questao5.Infrastructure.Database.QueryStore.Repository;
using Xunit;

namespace Questao5.Tests.Infrastructure.QueryStore.Repository
{
    public class AccountMovementQueryRepositoryTests : IDisposable
    {
        private readonly SqliteConnection _connection;
        private readonly AccountMovementQueryRepository _repository;

        public AccountMovementQueryRepositoryTests()
        {
            _connection = new SqliteConnection("DataSource=:memory:");
            _connection.Open();

            CriarTabelaMovimento(_connection);

            _repository = new AccountMovementQueryRepository(_connection);
        }

        private void CriarTabelaMovimento(SqliteConnection conn)
        {
            conn.Execute(@"
            CREATE TABLE movimento (
                idmovimento TEXT PRIMARY KEY,
                idcontacorrente TEXT,
                datamovimento TEXT,
                tipomovimento TEXT,
                valor REAL
            );
        ");
        }

        [Fact]
        public async Task InsertAccountMovement_DeveInserirOMovimentoERetornarIdValido()
        {
            string idContaCorrente = "conta1";
            string tipoMovimento = "C";
            decimal valor = 150.75m;

            var idMovimento = await _repository.InsertAccountMovement(idContaCorrente, tipoMovimento, valor);

            Assert.NotEqual(Guid.Empty, idMovimento);

            var movimento = await _connection.QuerySingleOrDefaultAsync<dynamic>(
                "SELECT * FROM movimento WHERE idmovimento = @id", new { id = idMovimento });

            Assert.NotNull(movimento);
            Assert.Equal(idContaCorrente, (string)movimento.idcontacorrente);
            Assert.Equal(tipoMovimento, (string)movimento.tipomovimento);
            Assert.Equal(valor, (decimal)movimento.valor);

            Assert.False(string.IsNullOrWhiteSpace((string)movimento.datamovimento));
        }

        public void Dispose()
        {
            _connection?.Close();
            _connection?.Dispose();
        }
    }
}
