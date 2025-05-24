using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Questao5.Application.Commands.Requests;
using Questao5.Application.Queries.Requests;
using Questao5.Domain.Exceptions;

namespace Questao5.Infrastructure.Services.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BankAccountController : ControllerBase
    {
        private readonly IMediator _mediator;

        public BankAccountController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Realiza uma movimentação na conta corrente (crédito ou débito).
        /// </summary>
        /// <param name="command">Dados para a movimentação da conta.</param>
        /// <returns>Confirmação da operação junto com os dados do usuário.</returns>
        /// <response code="200">Movimentação realizada com sucesso.</response>
        /// <response code="400">Erro de negócio (ex: conta inexistente).</response>
        [HttpPost("movimentarconta")]
        public async Task<IActionResult> Post(CreateBankAccountCommand command)
        {
            try
            {
                var result = await _mediator.Send(command);
                return Ok(result);
            }
            catch (BusinessException ex)
            {
                return BadRequest(new {tipoErro = ex.ErrorType.ToString(), mensagem = ex.Message });
            }
        }

        /// <summary>
        /// Consulta o saldo da conta corrente pelo número da conta.
        /// </summary>
        /// <param name="numero">Número da conta corrente.</param>
        /// <returns>Saldo atual da conta com os dados do usuário.</returns>
        /// <response code="200">Retorna o saldo atual junto aos dados do usuário.</response>
        /// <response code="400">Erro de negócio (ex: conta inexistente ou inativa).</response>
        [HttpGet("consultar/{numero}/saldo")]
        public async Task<IActionResult> GetSaldo(int numero)
        {
            try
            {
                var result = await _mediator.Send(new GetBalanceQuery { NumeroContaCorrente = numero });
                return Ok(result);
            }
            catch (BusinessException ex)
            {
                return BadRequest(new { tipoErro = ex.ErrorType.ToString(), mensagem = ex.Message });
            }
        }
    }
}
