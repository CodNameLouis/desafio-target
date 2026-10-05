using Desafio.Core.Comissoes;

namespace Desafio.Tests;

public class CalculadoraComissaoTests
{
    [Theory]
    [InlineData(0, 0)]
    [InlineData(99.99, 0)]
    [InlineData(100.00, 1.00)]       // 100 já entra na faixa de 1%
    [InlineData(499.99, 4.9999)]
    [InlineData(500.00, 25.00)]      // "a partir de" 500 => 5%
    [InlineData(1200.50, 60.025)]
    public void CalcularPorVenda_aplica_faixas_corretas(decimal valor, decimal esperado)
    {
        Assert.Equal(esperado, CalculadoraComissao.CalcularPorVenda(valor));
    }

    [Fact]
    public void CalcularPorVenda_rejeita_valor_negativo()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => CalculadoraComissao.CalcularPorVenda(-1m));
    }

    [Fact]
    public void CalcularPorVendedor_agrupa_e_soma_comissoes()
    {
        var vendas = new[]
        {
            new Venda("Ana", 50m),     // 0
            new Venda("Ana", 200m),    // 2,00
            new Venda("Ana", 1000m),   // 50,00
            new Venda("Bruno", 600m),  // 30,00
        };

        var resultado = CalculadoraComissao.CalcularPorVendedor(vendas);

        var ana = Assert.Single(resultado, r => r.Vendedor == "Ana");
        Assert.Equal(3, ana.QuantidadeVendas);
        Assert.Equal(1250m, ana.TotalVendido);
        Assert.Equal(52m, ana.TotalComissao);

        var bruno = Assert.Single(resultado, r => r.Vendedor == "Bruno");
        Assert.Equal(30m, bruno.TotalComissao);
    }

    [Fact]
    public void Dados_do_desafio_geram_comissoes_esperadas()
    {
        var vendas = LeitorVendas.LerArquivo(Caminhos.Dados("vendas.json"));

        var resultado = CalculadoraComissao.CalcularPorVendedor(vendas)
            .ToDictionary(r => r.Vendedor);

        Assert.Equal(36, vendas.Count);
        Assert.Equal(495.68m, resultado["João Silva"].TotalComissao);
        Assert.Equal(465.95m, resultado["Maria Souza"].TotalComissao);
        Assert.Equal(404.98m, resultado["Ana Lima"].TotalComissao);
        Assert.Equal(379.37m, resultado["Carlos Oliveira"].TotalComissao);
    }
}
