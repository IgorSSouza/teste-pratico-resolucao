using NSubstitute;
using Questao5.Application.Commands.Requests;
using Questao5.Application.Commands.Responses;
using Questao5.Application.Handlers;
using Questao5.Domain.Enumerators;
using Questao5.Domain.Exceptions;
using Questao5.Domain.Repositories;
using Questao5.Infrastructure.Database.QueryStore.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;

namespace Questao5.Tests
{
    public class CreateBankAccountHandlerTests
    {
        private readonly ICurrentAccountQueryRepository _contaRepo = Substitute.For<ICurrentAccountQueryRepository>();
        private readonly IAccountMovementQueryRepository _movimentoRepo = Substitute.For<IAccountMovementQueryRepository>();
        private readonly IIdEmpotenciaQueryRepository _idempotenciaRepo = Substitute.For<IIdEmpotenciaQueryRepository>();

        private CreateBankAccountHandler CriarHandler() =>
            new CreateBankAccountHandler(_contaRepo, _movimentoRepo, _idempotenciaRepo);

        [Fact]
        public async Task Deve_retornar_resultado_salvo_se_chave_idempotencia_existir()
        {
            var request = new CreateBankAccountCommand
            {
                ChaveIdempotencia = "abc123"
            };

            var idMovimento = Guid.NewGuid();
            var responseEsperado = new CreateBankAccountResponse { IdMovimento = idMovimento };
            var jsonSalvo = JsonSerializer.Serialize(responseEsperado);

            _idempotenciaRepo.GetResultIdEmpotenciaAsync("abc123").Returns(jsonSalvo);

            var handler = CriarHandler();

            var result = await handler.Handle(request, default);

            Assert.Equal(idMovimento, result.IdMovimento);
        }

        [Fact]
        public async Task Deve_lancar_excecao_quando_conta_nao_existir()
        {
            var request = new CreateBankAccountCommand
            {
                NumeroContaCorrente = 999,
                ChaveIdempotencia = "nova",
                TipoMovimento = "C",
                Valor = 100
            };

            _idempotenciaRepo.GetResultIdEmpotenciaAsync("nova").Returns((string?)null);
            _contaRepo.ObterPorNumeroAsync(999).Returns((CurrentAccountDto?)null);

            var handler = CriarHandler();

            var ex = await Assert.ThrowsAsync<BusinessException>(() =>
                handler.Handle(request, default)
            );

            Assert.Equal(BusinessErrorType.INVALID_ACCOUNT, ex.ErrorType);
        }

        [Fact]
        public async Task Deve_lancar_excecao_quando_conta_estiver_inativa()
        {
            var request = new CreateBankAccountCommand
            {
                NumeroContaCorrente = 10,
                ChaveIdempotencia = "chave-inativa",
                TipoMovimento = "C",
                Valor = 50
            };

            _idempotenciaRepo.GetResultIdEmpotenciaAsync("chave-inativa").Returns((string?)null);
            _contaRepo.ObterPorNumeroAsync(10).Returns(new CurrentAccountDto { ativo = 0 });

            var handler = CriarHandler();

            var ex = await Assert.ThrowsAsync<BusinessException>(() =>
                handler.Handle(request, default)
            );

            Assert.Equal(BusinessErrorType.INACTIVE_ACCOUNT, ex.ErrorType);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-10)]
        public async Task Deve_lancar_excecao_quando_valor_for_invalido(decimal valor)
        {
            var request = new CreateBankAccountCommand
            {
                NumeroContaCorrente = 10,
                ChaveIdempotencia = "valor-invalido",
                TipoMovimento = "C",
                Valor = valor
            };

            _idempotenciaRepo.GetResultIdEmpotenciaAsync("valor-invalido").Returns((string?)null);
            _contaRepo.ObterPorNumeroAsync(10).Returns(new CurrentAccountDto { ativo = 1 });

            var handler = CriarHandler();

            var ex = await Assert.ThrowsAsync<BusinessException>(() =>
                handler.Handle(request, default)
            );

            Assert.Equal(BusinessErrorType.INVALID_VALUE, ex.ErrorType);
        }

        [Fact]
        public async Task Deve_lancar_excecao_quando_tipo_for_invalido()
        {
            var request = new CreateBankAccountCommand
            {
                NumeroContaCorrente = 10,
                ChaveIdempotencia = "tipo-invalido",
                TipoMovimento = "X",
                Valor = 100
            };

            _idempotenciaRepo.GetResultIdEmpotenciaAsync("tipo-invalido").Returns((string?)null);
            _contaRepo.ObterPorNumeroAsync(10).Returns(new CurrentAccountDto { ativo = 1 });

            var handler = CriarHandler();

            var ex = await Assert.ThrowsAsync<BusinessException>(() =>
                handler.Handle(request, default)
            );

            Assert.Equal(BusinessErrorType.INVALID_TYPE, ex.ErrorType);
        }

        [Fact]
        public async Task Deve_retornar_idmovimento_quando_tudo_for_valido()
        {
            var request = new CreateBankAccountCommand
            {
                NumeroContaCorrente = 123,
                TipoMovimento = "C",
                Valor = 100,
                ChaveIdempotencia = "nova-chave"
            };

            var conta = new CurrentAccountDto { idcontacorrente = "abc123", ativo = 1 };

            var idMovimento = Guid.NewGuid();
            _idempotenciaRepo.GetResultIdEmpotenciaAsync("nova-chave").Returns((string?)null);
            _contaRepo.ObterPorNumeroAsync(123).Returns(conta);
            _movimentoRepo.InsertAccountMovement("abc123", "C", 100).Returns(idMovimento);

            var handler = CriarHandler();

            var result = await handler.Handle(request, default);

            Assert.Equal(idMovimento, result.IdMovimento);
            await _idempotenciaRepo.Received(1).SaveResultIdempotenciaAsync("nova-chave", request, result);
        }
    }
}
