using System;

namespace Questao1.Application.Services
{
    public class ContaBancariaService
    {
        public ContaBancaria CriarConta(int numero, string titular, double? depositoInicial = null)
        {
            return new ContaBancaria(numero, titular, depositoInicial ?? 0);
        }

        public void RealizarDeposito(ContaBancaria conta, double valor)
        {
            conta.Depositar(valor);
        }

        public void RealizarSaque(ContaBancaria conta, double valor)
        {
            conta.Sacar(valor);
        }

        public void AtualizarTitular(ContaBancaria conta, string novoTitular)
        {
            conta.AlterarTitular(novoTitular);
        }

        public static implicit operator ContaBancariaService(ContaBancaria v)
        {
            throw new NotImplementedException();
        }
    }
}
