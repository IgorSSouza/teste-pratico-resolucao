using System;
using System.Globalization;

namespace Questao1
{
    public class ContaBancaria {

        private const double TaxaSaque = 3.50;

        public int Numero { get; }
        public string Titular { get; private set; }
        public double Saldo { get; private set; }

        public ContaBancaria(int numero, string titular, double depositoInicial = 0)
        {
            if (string.IsNullOrWhiteSpace(titular)) throw new ArgumentException("Titular inválido");

            Numero = numero;
            Titular = titular;
            Saldo = depositoInicial;
        }

        public void AlterarTitular(string novoTitular)
        {
            if (string.IsNullOrWhiteSpace(novoTitular)) throw new ArgumentException("Nome inválido");
            Titular = novoTitular;
        }

        public void Depositar(double valor)
        {
            if (valor <= 0) throw new ArgumentException("Depósito deve ser positivo");
            Saldo += valor;
        }

        public void Sacar(double valor)
        {
            if (valor <= 0) throw new ArgumentException("Saque deve ser positivo");
            Saldo -= (valor + TaxaSaque);
        }

        public override string ToString()
        {
            return $"Conta {Numero}, Titular: {Titular}, Saldo: $ {Saldo.ToString("F2", CultureInfo.InvariantCulture)}";
        }
    }
}
