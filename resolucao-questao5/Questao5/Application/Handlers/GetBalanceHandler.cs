using MediatR;
using Questao5.Application.Queries.Requests;
using Questao5.Application.Queries.Responses;
using Questao5.Domain.Enumerators;
using Questao5.Domain.Exceptions;
using Questao5.Domain.Repositories;

namespace Questao5.Application.Handlers
{
    public class GetBalanceHandler : IRequestHandler<GetBalanceQuery, GetBalanceResponse>
    {
        private readonly ICurrentAccountQueryRepository _currentAccountQueryRepository;

        public GetBalanceHandler(ICurrentAccountQueryRepository currentAccountQueryRepository)
        {
            _currentAccountQueryRepository = currentAccountQueryRepository;
        }


        public async Task<GetBalanceResponse> Handle(GetBalanceQuery request, CancellationToken cancellationToken)
        {
            var conta = await _currentAccountQueryRepository.ObterPorNumeroAsync(request.NumeroContaCorrente);

            if (conta == null)
                throw new BusinessException(BusinessErrorType.INVALID_ACCOUNT, "Conta inválida.");

            if (conta.ativo == 0)
                throw new BusinessException(BusinessErrorType.INACTIVE_ACCOUNT, "Conta inativa.");

            var creditos = await _currentAccountQueryRepository.ObterTotalCreditosAsync(conta.idcontacorrente);

            var debitos = await _currentAccountQueryRepository.ObterTotalDebitosAsync(conta.idcontacorrente);

            var saldoAtual = debitos - creditos;

            return new GetBalanceResponse
            {
                NumeroContaCorrente = (int)conta.numero,
                NomeTitular = conta.nome,
                DataHoraConsulta = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"),
                SaldoAtual = saldoAtual
            };
        }
    }
}

