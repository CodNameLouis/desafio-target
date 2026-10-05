using Desafio.Core.Juros;

namespace Desafio.App.Telas;

internal static class TelaJuros
{
    public static void Exibir()
    {
        Console.WriteLine("=== 3. Cálculo de juros por atraso ===");
        Console.WriteLine($"Taxa: {CalculadoraJuros.TaxaDiaria:P1} ao dia (juros simples)");
        Console.WriteLine();

        var valor = Entrada.Valor("Valor (R$): ");
        var vencimento = Entrada.Data("Data de vencimento (dd/mm/aaaa): ");

        var r = CalculadoraJuros.CalcularHoje(valor, vencimento);

        Console.WriteLine();
        Console.WriteLine($"Data de hoje:     {r.DataCalculo:dd/MM/yyyy}");
        Console.WriteLine($"Vencimento:       {r.Vencimento:dd/MM/yyyy}");
        Console.WriteLine($"Dias em atraso:   {r.DiasAtraso}");
        Console.WriteLine($"Valor original:   {r.ValorOriginal:C}");
        Console.WriteLine($"Juros:            {r.Juros:C}");
        Entrada.Sucesso($"Valor atualizado: {r.ValorAtualizado:C}");

        if (r.DiasAtraso == 0)
            Console.WriteLine("Título não está vencido: não há juros a cobrar.");
    }
}
