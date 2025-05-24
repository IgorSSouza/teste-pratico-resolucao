using Dapper;
using Microsoft.Data.Sqlite;
using Questao5.Infrastructure.Database.QueryStore.Repository;
using Xunit;

namespace Questao5.Tests.Infrastructure.QueryStore.Repository
{
    public class CurrentAccountQueryRepositoryTests : IDisposable
    {
        private readonly SqliteConnection _connection;
        private readonly CurrentAccountQueryRepository _repository;

        public CurrentAccountQueryRepositoryTests()
        {
            _connection = new SqliteConnection("DataSource=:memory:");
            _connection.Open();

            CriarTabelasDeTeste(_connection);
            InserirDadosDeTeste(_connection);

            _repository = new CurrentAccountQueryRepository(_connection);

        }

        private void CriarTabelasDeTeste(SqliteConnection conn)
        {
            conn.Execute(@"
            CREATE TABLE contacorrente (
                idcontacorrente TEXT PRIMARY KEY,
                numero INTEGER,
                nome TEXT,
                ativo INTEGER
            );

            CREATE TABLE movimento (
                idmovimento TEXT PRIMARY KEY,
                idcontacorrente TEXT,
                valor REAL,
                tipomovimento TEXT
            );
        ");
        }
        private void InserirDadosDeTeste(SqliteConnection conn)
        {
            conn.Execute("INSERT INTO contacorrente VALUES ('1', 123, 'João Silva', 1)");
            conn.Execute("INSERT INTO movimento VALUES ('m1', '1', 100.0, 'C')");
            conn.Execute("INSERT INTO movimento VALUES ('m2', '1', 40.0, 'D')");
        }

        [Fact]
        public async Task ObterPorNumeroAsync_DeveRetornarConta()
        {
            var conta = await _repository.ObterPorNumeroAsync(123);

            Assert.NotNull(conta);
            Assert.Equal("João Silva", conta!.nome);
            Assert.Equal(123, conta.numero);
            Assert.Equal(1,conta.ativo);
        }

        [Fact]
        public async Task ObterTotalCreditosAsync_DeveRetornarSoma()
        {
            var total = await _repository.ObterTotalCreditosAsync("1");

            Assert.Equal(100.0m, total);
        }

        [Fact]
        public async Task ObterTotalDebitosAsync_DeveRetornarSoma()
        {
            var total = await _repository.ObterTotalDebitosAsync("1");

            Assert.Equal(40.0m, total);
        }

        public void Dispose()
        {
            _connection?.Close();
            _connection?.Dispose();
        }
    }
}
