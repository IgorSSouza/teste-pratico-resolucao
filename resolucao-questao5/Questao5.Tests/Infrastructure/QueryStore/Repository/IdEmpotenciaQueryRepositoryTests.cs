using Dapper;
using Microsoft.Data.Sqlite;
using Questao5.Infrastructure.Database.QueryStore.Repository;
using Xunit;

namespace Questao5.Tests.Infrastructure.QueryStore.Repository
{
    public class IdEmpotenciaQueryRepositoryTests : IDisposable
    {
        private readonly SqliteConnection _connection;
        private readonly IdEmpotenciaQueryRepository _repository;

        public IdEmpotenciaQueryRepositoryTests()
        {
            _connection = new SqliteConnection("DataSource=:memory:");
            _connection.Open();

            CriarTabelaIdempotencia(_connection);

            _repository = new IdEmpotenciaQueryRepository(_connection);
        }

        private void CriarTabelaIdempotencia(SqliteConnection conn)
        {
            conn.Execute(@"
            CREATE TABLE idempotencia (
                chave_idempotencia TEXT PRIMARY KEY,
                requisicao TEXT,
                resultado TEXT
            );
        ");
        }

        [Fact]
        public async Task SaveResultIdempotenciaAsync_DeveSalvarEGetResultIdEmpotenciaAsync_DeveRetornarResultado()
        {
            string chave = "chave123";
            var requisicao = new { Valor = 10, Nome = "Teste" };
            var resposta = new { Sucesso = true, Mensagem = "Ok" };

            await _repository.SaveResultIdempotenciaAsync(chave, requisicao, resposta);

            var resultadoJson = await _repository.GetResultIdEmpotenciaAsync(chave);

            Assert.NotNull(resultadoJson);
            Assert.Contains("Ok", resultadoJson); 

            var resultadoObj = System.Text.Json.JsonSerializer.Deserialize<dynamic>(resultadoJson!);
            Assert.NotNull(resultadoObj);
        }

        [Fact]
        public async Task GetResultIdEmpotenciaAsync_DeveRetornarNullQuandoChaveNaoExistir()
        {
            string chaveInexistente = "inexistente";

            var resultado = await _repository.GetResultIdEmpotenciaAsync(chaveInexistente);

            Assert.Null(resultado);
        }

        public void Dispose()
        {
            _connection?.Close();
            _connection?.Dispose();
        }
    }
}
