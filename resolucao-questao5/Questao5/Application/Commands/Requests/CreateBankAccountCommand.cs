using MediatR;
using Questao5.Application.Commands.Responses;

namespace Questao5.Application.Commands.Requests
{
    /// <summary>
    /// Modelo para resposta do saldo do usuário.
    /// </summary>
    public class CreateBankAccountCommand : IRequest<CreateBankAccountResponse>
    {
        /// <summary>
        /// Chave da requisição.
        /// </summary>
        /// <example>R8526</example>
        public string ChaveIdempotencia { get; set; }
        /// <summary>
        /// Número da conta corrente.
        /// </summary>
        /// <example>12345</example>
        public int NumeroContaCorrente { get; set; }
        /// <summary>
        /// Tipo de Movimentação.
        /// </summary>
        /// <example>'C' Crédito ou 'D' Débito</example>
        public string TipoMovimento { get; set; }
        /// <summary>
        /// Valor da Movimentação.
        /// </summary>
        /// <example>150.50</example>
        public decimal Valor { get; set; }
    }
}
