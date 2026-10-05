namespace Desafio.Core.Estoque;

public sealed record Movimentacao(
    int Id,
    DateTime DataHora,
    int CodigoProduto,
    TipoMovimentacao Tipo,
    string Descricao,
    int Quantidade,
    int EstoqueAnterior,
    int EstoqueFinal);
