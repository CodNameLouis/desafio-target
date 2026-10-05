namespace Desafio.Core.Comissoes;

public sealed record ComissaoVendedor(
    string Vendedor,
    int QuantidadeVendas,
    decimal TotalVendido,
    decimal TotalComissao);
