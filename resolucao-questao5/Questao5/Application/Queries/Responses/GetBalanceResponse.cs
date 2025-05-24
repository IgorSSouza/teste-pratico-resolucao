namespace Questao5.Application.Queries.Responses
{
    public class GetBalanceResponse
    {
        public int NumeroContaCorrente { get; set; }
        public string NomeTitular { get; set; } = string.Empty;
        public string? DataHoraConsulta { get; set; }
        public decimal SaldoAtual { get; set; }
    }
}
