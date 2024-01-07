using System;
using System.Collections.Generic;


public abstract class Conta
{
    
    public virtual double SaldoEmReais { get; set; }

    
    public Conta(double saldoEmReais)
    {
        SaldoEmReais = saldoEmReais;
    }
}


public interface ITarifa
{
    double Calcular();
}


public class ContaCorrente : Conta, ITarifa
{
    public ContaCorrente(double saldoEmReais) : base(saldoEmReais) { }

    public override double SaldoEmReais
    {
        get { return base.SaldoEmReais; }
        set { base.SaldoEmReais = value; }
    }

    
    public double Calcular()
    {
        return SaldoEmReais * 0.015; 
    }
}


public class ContaInternacional : Conta, ITarifa
{
    private const double TaxaDeCambio = 5.0; 

    public double SaldoEmDolar { get; set; }

    public ContaInternacional(double saldoEmDolar) : base(saldoEmDolar * TaxaDeCambio)
    {
        SaldoEmDolar = saldoEmDolar;
    }

    
    public double Calcular()
    {
        return SaldoEmReais * 0.025; 
    }
}


public class ContaCripto : Conta
{
    public double SaldoEmDolar { get; set; }

    public ContaCripto(double saldoEmDolar) : base(saldoEmDolar)
    {
        SaldoEmDolar = saldoEmDolar;
    }
}


public class SistemaDeTarifas
{
    
    public double ValorTotalDoSaldo { get; private set; }
    public double ValorTotalDaTarifa { get; private set; }

    
    public void AcumularTarifaESaldo(Conta conta)
    {
        double tarifa = 0;

        if (conta is ITarifa contaComTarifa)
        {
            tarifa = contaComTarifa.Calcular();
        }

        ValorTotalDoSaldo += conta.SaldoEmReais;
        ValorTotalDaTarifa += tarifa;
    }
}

class Program
{
    static void Main(string[] args)
    {
        SistemaDeTarifas sistemaDeTarifas = new SistemaDeTarifas();
        List<Conta> contas = new List<Conta>();

        while (true)
        {
            Console.WriteLine("Escolha o tipo de conta (1 - Conta Corrente, 2 - Conta Internacional, 3 - Conta Cripto, 4 - Sair):");
            int escolha = int.Parse(Console.ReadLine());

            if (escolha == 4)
            {
                break;
            }

            Console.WriteLine("Digite o saldo da conta:");

            double saldo = double.Parse(Console.ReadLine());

            Conta conta = null;

            switch (escolha)
            {
                case 1:
                    conta = new ContaCorrente(saldo);
                    break;
                case 2:
                    conta = new ContaInternacional(saldo);
                    break;
                case 3:
                    conta = new ContaCripto(saldo);
                    break;
                default:
                    Console.WriteLine("Escolha inválida. Tente novamente.");
                    continue;
            }

            contas.Add(conta);
            sistemaDeTarifas.AcumularTarifaESaldo(conta);
        }

        Console.WriteLine("\nResumo das Contas:");
        foreach (var conta in contas)
        {
            Console.WriteLine($"Tipo de Conta: {conta.GetType().Name}");
            Console.WriteLine($"Saldo em Reais: {conta.SaldoEmReais:C}");
            if (conta is ITarifa contaComTarifa)
            {
                Console.WriteLine($"Tarifa: {contaComTarifa.Calcular():C}");
            }
            Console.WriteLine();
        }

        Console.WriteLine($"Valor Total do Saldo em Reais: {sistemaDeTarifas.ValorTotalDoSaldo:C}");
        Console.WriteLine($"Valor Total da Tarifa: {sistemaDeTarifas.ValorTotalDaTarifa:C}");
    }
}
