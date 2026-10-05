using Desafio.Core.Comissoes;

namespace Desafio.App.Telas;

internal static class TelaComissoes
{
    public static void Exibir(string caminhoVendas)
    {
        Console.WriteLine("=== 1. Comissão por vendedor ===");
        Console.WriteLine("Regra por venda: < R$ 100,00 = 0% | < R$ 500,00 = 1% | >= R$ 500,00 = 5%");
        Console.WriteLine();

        var vendas = LeitorVendas.LerArquivo(caminhoVendas);
        var comissoes = CalculadoraComissao.CalcularPorVendedor(vendas);

        Console.WriteLine($"{"Vendedor",-20} {"Vendas",6} {"Total vendido",16} {"Comissão",14}");
        Console.WriteLine(new string('-', 59));

        foreach (var c in comissoes)
            Console.WriteLine($"{c.Vendedor,-20} {c.QuantidadeVendas,6} {c.TotalVendido,16:C} {c.TotalComissao,14:C}");

        Console.WriteLine(new string('-', 59));
        Console.WriteLine(
            $"{"TOTAL",-20} {comissoes.Sum(c => c.QuantidadeVendas),6} " +
            $"{comissoes.Sum(c => c.TotalVendido),16:C} {comissoes.Sum(c => c.TotalComissao),14:C}");
    }
}
