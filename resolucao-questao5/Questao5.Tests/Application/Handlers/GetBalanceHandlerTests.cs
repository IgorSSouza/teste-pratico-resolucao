using NSubstitute;
using Questao5.Application.Handlers;
using Questao5.Application.Queries.Requests;
using Questao5.Domain.Exceptions;
using Questao5.Domain.Repositories;
using Questao5.Infrastructure.Database.QueryStore.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Questao5.Tests.Application.Handlers
{
    public class GetBalanceHandlerTests
    {
        private readonly ICurrentAccountQueryRepository _contaRepo;

        public GetBalanceHandlerTests()
        {
            _contaRepo = Substitute.For<ICurrentAccountQueryRepository>();
        }

        private GetBalanceHandler CriarHandler() => new GetBalanceHandler(_contaRepo);

        [Fact]
        public async Task Deve_lancar_excecao_quando_conta_nao_existir()
        {
            var request = new GetBalanceQuery { NumeroContaCorrente = 123 };

            _contaRepo.ObterPorNumeroAsync(123).Returns((CurrentAccountDto?)null);

            var handler = CriarHandler();

            await Assert.ThrowsAsync<BusinessException>(() => handler.Handle(request, CancellationToken.None));
        }

        [Fact]
        public async Task Deve_lancar_excecao_quando_conta_estiver_inativa()
        {
            var request = new GetBalanceQuery { NumeroContaCorrente = 123 };

            var conta = new CurrentAccountDto { ativo = 0, numero = 123, nome = "João", idcontacorrente = "abc" };
            _contaRepo.ObterPorNumeroAsync(123).Returns(conta);

            var handler = CriarHandler();

            await Assert.ThrowsAsync<BusinessException>(() => handler.Handle(request, CancellationToken.None));
        }

        [Fact]
        public async Task Deve_retornar_saldo_quando_conta_for_valida()
        {
            var request = new GetBalanceQuery { NumeroContaCorrente = 123 };

            var conta = new CurrentAccountDto
            {
                ativo = 1,
                numero = 123,
                nome = "João",
                idcontacorrente = "abc"
            };

            _contaRepo.ObterPorNumeroAsync(123).Returns(conta);
            _contaRepo.ObterTotalCreditosAsync("abc").Returns(200);
            _contaRepo.ObterTotalDebitosAsync("abc").Returns(500);

            var handler = CriarHandler();

            var result = await handler.Handle(request, CancellationToken.None);

            Assert.Equal(123, result.NumeroContaCorrente);
            Assert.Equal("João", result.NomeTitular);
            Assert.Equal(300, result.SaldoAtual);
            Assert.False(string.IsNullOrWhiteSpace(result.DataHoraConsulta));
        }
    }
}
