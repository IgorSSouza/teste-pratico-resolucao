namespace Questao5.Domain.Repositories
{
    public interface IAccountMovementQueryRepository
    {
        Task<Guid>InsertAccountMovement(string idContaCorrente, string tipoMovimento, decimal valor);
    }
}
