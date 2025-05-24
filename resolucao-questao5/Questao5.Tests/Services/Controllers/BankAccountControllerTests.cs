using MediatR;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using Questao5.Application.Commands.Requests;
using Questao5.Application.Commands.Responses;
using Questao5.Application.Queries.Requests;
using Questao5.Application.Queries.Responses;
using Questao5.Domain.Enumerators;
using Questao5.Domain.Exceptions;
using Questao5.Infrastructure.Services.Controllers;
using System.Text.Json;
using Xunit;

namespace Questao5.Tests.Services.Controllers
{
    public class BankAccountControllerTests
    {
        private readonly IMediator _mediatorMock;
        private readonly BankAccountController _controller;

        public BankAccountControllerTests()
        {
            _mediatorMock = Substitute.For<IMediator>();
            _controller = new BankAccountController(_mediatorMock);
        }

        [Fact]
        public async Task GetSaldo_DeveRetornarSaldo_QuandoSucesso()
        {
            var response = new GetBalanceResponse
            {
                NumeroContaCorrente = 123,
                NomeTitular = "João",
                DataHoraConsulta = "18/05/2025 10:30:00",
                SaldoAtual = 200
            };

            _mediatorMock.Send(Arg.Any<GetBalanceQuery>()).Returns(response);

            var result = await _controller.GetSaldo(123);
            var okResult = Assert.IsType<OkObjectResult>(result);
            var saldo = Assert.IsType<GetBalanceResponse>(okResult.Value);

            Assert.Equal(123, saldo.NumeroContaCorrente);
            Assert.Equal("João", saldo.NomeTitular);
            Assert.Equal(200, saldo.SaldoAtual);
        }

        [Fact]
        public async Task Post_DeveRetornarOk_QuandoSucesso()
        {
            var command = new CreateBankAccountCommand
            {
                NumeroContaCorrente = 123,
                TipoMovimento = "C",
                Valor = 100,
                ChaveIdempotencia = "teste"
            };

            var idMovimento = new Guid();
            var resultado = new CreateBankAccountResponse { IdMovimento = idMovimento };
            _mediatorMock.Send(command).Returns(resultado);

            var response = await _controller.Post(command);

            var okResult = Assert.IsType<OkObjectResult>(response);
            var retorno = Assert.IsType<CreateBankAccountResponse>(okResult.Value);
            Assert.Equal(idMovimento, retorno.IdMovimento);
        }

        [Fact]
        public async Task Post_DeveRetornarBadRequest_QuandoLancarBusinessException()
        {
            var command = new CreateBankAccountCommand
            {
                NumeroContaCorrente = 123582,
                TipoMovimento = "C",
                Valor = 100,
                ChaveIdempotencia = "chave-teste"
            };

            _mediatorMock.Send(Arg.Any<CreateBankAccountCommand>())
                .Throws(new BusinessException(BusinessErrorType.INVALID_ACCOUNT, "Conta inválida."));

            var response = await _controller.Post(command);

            var badRequest = Assert.IsType<BadRequestObjectResult>(response);

            var json = JsonSerializer.Serialize(badRequest.Value);
            var erro = JsonSerializer.Deserialize<Dictionary<string, string>>(json);

            Assert.Equal("INVALID_ACCOUNT", erro["tipoErro"]);
            Assert.Equal("Conta inválida.", erro["mensagem"]);
        }
    }
}
