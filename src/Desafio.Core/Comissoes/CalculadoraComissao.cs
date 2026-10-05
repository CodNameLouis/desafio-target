namespace Desafio.Core.Comissoes;

/// <summary>
/// Regras de comissão aplicadas a cada venda individualmente:
/// abaixo de R$ 100,00 não gera comissão; abaixo de R$ 500,00 gera 1%;
/// a partir de R$ 500,00 gera 5%.
/// </summary>
public static class CalculadoraComissao
{
    public const decimal ValorMinimoComissao = 100m;
    public const decimal ValorFaixaSuperior = 500m;
    public const decimal PercentualFaixaInferior = 0.01m;
    public const decimal PercentualFaixaSuperior = 0.05m;

    public static decimal PercentualPara(decimal valorVenda) => valorVenda switch
    {
        < 0 => throw new ArgumentOutOfRangeException(nameof(valorVenda), "O valor da venda não pode ser negativo."),
        < ValorMinimoComissao => 0m,
        < ValorFaixaSuperior => PercentualFaixaInferior,
        _ => PercentualFaixaSuperior
    };

    public static decimal CalcularPorVenda(decimal valorVenda) => valorVenda * PercentualPara(valorVenda);

    /// <summary>
    /// Agrupa as vendas por vendedor e soma as comissões de cada venda.
    /// O arredondamento para centavos é feito apenas no total, evitando acúmulo de erro por venda.
    /// </summary>
    public static IReadOnlyList<ComissaoVendedor> CalcularPorVendedor(IEnumerable<Venda> vendas)
    {
        ArgumentNullException.ThrowIfNull(vendas);

        return vendas
            .GroupBy(v => v.Vendedor.Trim(), StringComparer.OrdinalIgnoreCase)
            .Select(grupo => new ComissaoVendedor(
                Vendedor: grupo.First().Vendedor.Trim(),
                QuantidadeVendas: grupo.Count(),
                TotalVendido: grupo.Sum(v => v.Valor),
                TotalComissao: Arredondar(grupo.Sum(v => CalcularPorVenda(v.Valor)))))
            .OrderByDescending(c => c.TotalComissao)
            .ToList();
    }

    private static decimal Arredondar(decimal valor) => Math.Round(valor, 2, MidpointRounding.AwayFromZero);
}
