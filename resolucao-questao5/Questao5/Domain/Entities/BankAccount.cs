namespace Questao5.Domain.Entities
{
    public class BankAccount
    {
        public Guid IdMovimento { get; set; }
        public Guid IdContaCorrente { get; set; }
        public string DataMovimento { get; set; }
        public string TipoMovimento { get; set; } 
        public decimal Valor { get; set; }
    }
}
