using Dapper;
using MediatR;
using Questao5.Application.Commands.Requests;
using Questao5.Application.Commands.Responses;
using Questao5.Domain.Enumerators;
using Questao5.Domain.Exceptions;
using Questao5.Domain.Repositories;
using System.Text.Json;

namespace Questao5.Application.Handlers
{
    public class CreateBankAccountHandler : IRequestHandler<CreateBankAccountCommand, CreateBankAccountResponse>
    {
        private readonly ICurrentAccountQueryRepository _currentAccountQueryRepository;
        private readonly IAccountMovementQueryRepository _accountMovementQueryRepository;
        private readonly IIdEmpotenciaQueryRepository _idEmpotenciaQueryRepotory;

        public CreateBankAccountHandler(
            ICurrentAccountQueryRepository currentAccountQueryRepository,
            IAccountMovementQueryRepository accountMovementQueryRepository,
            IIdEmpotenciaQueryRepository idEmpotenciaQueryRepotory)
        {
            _currentAccountQueryRepository = currentAccountQueryRepository;
            _accountMovementQueryRepository = accountMovementQueryRepository;
            _idEmpotenciaQueryRepotory = idEmpotenciaQueryRepotory;
        }

        public async Task<CreateBankAccountResponse> Handle(CreateBankAccountCommand request, CancellationToken cancellationToken)
        {
           
                var buscarIdEmpotencia = await _idEmpotenciaQueryRepotory.GetResultIdEmpotenciaAsync(request.ChaveIdempotencia);

                if (buscarIdEmpotencia != null)
                {
                    return JsonSerializer.Deserialize<CreateBankAccountResponse>(buscarIdEmpotencia);
                }

                // Validações
                var conta = await _currentAccountQueryRepository.ObterPorNumeroAsync(request.NumeroContaCorrente);

                if (conta == null)
                    throw new BusinessException(BusinessErrorType.INVALID_ACCOUNT, "Conta inválida.");

                if (conta.ativo == 0)
                    throw new BusinessException(BusinessErrorType.INACTIVE_ACCOUNT, "Conta inativa.");

                if (request.Valor <= 0)
                    throw new BusinessException(BusinessErrorType.INVALID_VALUE, "Valor não permitido.");

                if (request.TipoMovimento != "C" && request.TipoMovimento != "D")
                    throw new BusinessException(BusinessErrorType.INVALID_TYPE, "Tipo de conta não existe.");

                var idAccountMovement = await _accountMovementQueryRepository.InsertAccountMovement(conta.idcontacorrente, request.TipoMovimento, request.Valor);

                var response = new CreateBankAccountResponse { IdMovimento = idAccountMovement };

                await _idEmpotenciaQueryRepotory.SaveResultIdempotenciaAsync(request.ChaveIdempotencia, request, response);

                return response;
        }
            
    }
}



