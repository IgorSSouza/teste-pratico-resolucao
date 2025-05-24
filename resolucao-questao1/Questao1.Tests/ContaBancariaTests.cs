using Xunit;

namespace Questao1.Tests
{
    public class ContaBancariaTests
    {
        [Fact]
        public void Deve_Criar_Conta_Com_Saldo_Zero_Quando_Deposito_Inicial_Nao_Informado()
        {
            var conta = new ContaBancaria(123, "Fulano");
            Assert.Equal(0, conta.Saldo);
        }

        [Fact]
        public void Deve_Realizar_Deposito_Corretamente()
        {
            var conta = new ContaBancaria(123, "Fulano");
            conta.Depositar(100);
            Assert.Equal(100, conta.Saldo);
        }

        [Fact]
        public void Deve_Realizar_Saque_E_Subtrair_Taxa()
        {
            var conta = new ContaBancaria(123, "Fulano", 100);
            conta.Sacar(50);
            Assert.Equal(46.5, conta.Saldo);
        }

        [Fact]
        public void Deve_Atualizar_Titular_Corretamente()
        {
            var conta = new ContaBancaria(123, "Fulano");
            conta.AlterarTitular("Ciclano");
            Assert.Equal("Ciclano", conta.Titular);
        }

        [Fact]
        public void Conta_Pode_Ficar_Negativa()
        {
            var conta = new ContaBancaria(123, "Fulano");
            conta.Sacar(10);
            Assert.Equal(-13.5, conta.Saldo);
        }
    }
}
