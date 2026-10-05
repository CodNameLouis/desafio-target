namespace Desafio.Core.Estoque;

/// <summary>
/// Registra entradas e saídas de mercadoria. Cada movimentação recebe um
/// identificador sequencial único e devolve o estoque final do produto.
/// </summary>
public sealed class ControleEstoque
{
    private readonly Dictionary<int, Produto> _produtos;
    private readonly List<Movimentacao> _movimentacoes = [];
    private readonly TimeProvider _relogio;
    private int _ultimoId;

    public ControleEstoque(IEnumerable<Produto> produtos, TimeProvider? relogio = null)
    {
        ArgumentNullException.ThrowIfNull(produtos);

        _produtos = new Dictionary<int, Produto>();
        foreach (var produto in produtos)
        {
            if (!_produtos.TryAdd(produto.Codigo, produto))
                throw new ArgumentException($"Código de produto duplicado: {produto.Codigo}.", nameof(produtos));
        }

        _relogio = relogio ?? TimeProvider.System;
    }

    public IReadOnlyCollection<Produto> Produtos => _produtos.Values;
    public IReadOnlyList<Movimentacao> Movimentacoes => _movimentacoes;

    public Produto ObterProduto(int codigo) =>
        _produtos.TryGetValue(codigo, out var produto)
            ? produto
            : throw new KeyNotFoundException($"Produto {codigo} não encontrado.");

    public Movimentacao Movimentar(int codigoProduto, TipoMovimentacao tipo, int quantidade, string descricao)
    {
        if (quantidade <= 0)
            throw new ArgumentOutOfRangeException(nameof(quantidade), "A quantidade deve ser maior que zero.");
        if (string.IsNullOrWhiteSpace(descricao))
            throw new ArgumentException("Informe uma descrição para a movimentação.", nameof(descricao));
        if (!Enum.IsDefined(tipo))
            throw new ArgumentOutOfRangeException(nameof(tipo), "Tipo de movimentação inválido.");

        var produto = ObterProduto(codigoProduto);
        var estoqueAnterior = produto.Quantidade;

        if (tipo == TipoMovimentacao.Entrada)
            produto.Entrada(quantidade);
        else
            produto.Saida(quantidade);

        var movimentacao = new Movimentacao(
            Id: ++_ultimoId,
            DataHora: _relogio.GetLocalNow().DateTime,
            CodigoProduto: produto.Codigo,
            Tipo: tipo,
            Descricao: descricao.Trim(),
            Quantidade: quantidade,
            EstoqueAnterior: estoqueAnterior,
            EstoqueFinal: produto.Quantidade);

        _movimentacoes.Add(movimentacao);
        return movimentacao;
    }
}
