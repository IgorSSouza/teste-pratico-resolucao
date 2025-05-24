namespace Questao5.Domain.Repositories
{
    public interface IIdEmpotenciaQueryRepository
    {
        Task<string?> GetResultIdEmpotenciaAsync(string chave);
        Task SaveResultIdempotenciaAsync<TRequest, TResponse>(string chave, TRequest requisicao, TResponse resposta);
    }
}

