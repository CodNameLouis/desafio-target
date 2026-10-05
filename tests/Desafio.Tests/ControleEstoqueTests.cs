using Desafio.Core.Estoque;

namespace Desafio.Tests;

public class ControleEstoqueTests
{
    private static ControleEstoque NovoControle() => new(
    [
        new Produto(101, "Caneta Azul", 150),
        new Produto(102, "Caderno Universitário", 75),
    ]);

    [Fact]
    public void Entrada_soma_ao_estoque_e_retorna_estoque_final()
    {
        var controle = NovoControle();

        var mov = controle.Movimentar(101, TipoMovimentacao.Entrada, 50, "Compra de fornecedor");

        Assert.Equal(150, mov.EstoqueAnterior);
        Assert.Equal(200, mov.EstoqueFinal);
        Assert.Equal(200, controle.ObterProduto(101).Quantidade);
    }

    [Fact]
    public void Saida_subtrai_do_estoque()
    {
        var controle = NovoControle();

        var mov = controle.Movimentar(102, TipoMovimentacao.Saida, 25, "Venda");

        Assert.Equal(50, mov.EstoqueFinal);
    }

    [Fact]
    public void Cada_movimentacao_recebe_id_unico_sequencial()
    {
        var controle = NovoControle();

        var m1 = controle.Movimentar(101, TipoMovimentacao.Entrada, 1, "A");
        var m2 = controle.Movimentar(102, TipoMovimentacao.Saida, 1, "B");
        var m3 = controle.Movimentar(101, TipoMovimentacao.Saida, 1, "C");

        Assert.Equal([1, 2, 3], new[] { m1.Id, m2.Id, m3.Id });
        Assert.Equal(3, controle.Movimentacoes.Count);
    }

    [Fact]
    public void Saida_maior_que_estoque_e_rejeitada_sem_alterar_saldo()
    {
        var controle = NovoControle();

        Assert.Throws<InvalidOperationException>(() =>
            controle.Movimentar(102, TipoMovimentacao.Saida, 76, "Venda"));

        Assert.Equal(75, controle.ObterProduto(102).Quantidade);
        Assert.Empty(controle.Movimentacoes);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-5)]
    public void Quantidade_deve_ser_positiva(int quantidade)
    {
        var controle = NovoControle();

        Assert.Throws<ArgumentOutOfRangeException>(() =>
            controle.Movimentar(101, TipoMovimentacao.Entrada, quantidade, "Ajuste"));
    }

    [Fact]
    public void Descricao_e_obrigatoria()
    {
        var controle = NovoControle();

        Assert.Throws<ArgumentException>(() =>
            controle.Movimentar(101, TipoMovimentacao.Entrada, 1, "   "));
    }

    [Fact]
    public void Produto_inexistente_e_rejeitado()
    {
        var controle = NovoControle();

        Assert.Throws<KeyNotFoundException>(() =>
            controle.Movimentar(999, TipoMovimentacao.Entrada, 1, "Compra"));
    }

    [Fact]
    public void Le_estoque_inicial_do_json_do_desafio()
    {
        var produtos = LeitorEstoque.LerArquivo(Caminhos.Dados("estoque.json"));

        Assert.Equal(5, produtos.Count);
        var lapis = Assert.Single(produtos, p => p.Codigo == 104);
        Assert.Equal("Lápis Preto HB", lapis.Descricao);
        Assert.Equal(320, lapis.Quantidade);
    }
}
