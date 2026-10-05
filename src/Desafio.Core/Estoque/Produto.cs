namespace Desafio.Core.Estoque;

public sealed class Produto
{
    public Produto(int codigo, string descricao, int quantidade)
    {
        if (quantidade < 0)
            throw new ArgumentOutOfRangeException(nameof(quantidade), "O estoque inicial não pode ser negativo.");

        Codigo = codigo;
        Descricao = descricao;
        Quantidade = quantidade;
    }

    public int Codigo { get; }
    public string Descricao { get; }
    public int Quantidade { get; private set; }

    internal void Entrada(int quantidade) => Quantidade += quantidade;

    internal void Saida(int quantidade)
    {
        if (quantidade > Quantidade)
            throw new InvalidOperationException(
                $"Estoque insuficiente para \"{Descricao}\": disponível {Quantidade}, solicitado {quantidade}.");

        Quantidade -= quantidade;
    }
}
