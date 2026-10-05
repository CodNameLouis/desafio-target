using System.Text.Json;

namespace Desafio.Core.Estoque;

public static class LeitorEstoque
{
    private sealed record ItemJson(int CodigoProduto, string DescricaoProduto, int Estoque);
    private sealed record Arquivo(List<ItemJson>? Estoque);

    public static IReadOnlyList<Produto> LerJson(string json)
    {
        var arquivo = JsonSerializer.Deserialize<Arquivo>(json, OpcoesJson.Padrao)
            ?? throw new FormatException("JSON de estoque vazio ou inválido.");

        var itens = arquivo.Estoque ?? throw new FormatException("O JSON não possui a propriedade \"estoque\".");

        return itens
            .Select(i => new Produto(i.CodigoProduto, i.DescricaoProduto, i.Estoque))
            .ToList();
    }

    public static IReadOnlyList<Produto> LerArquivo(string caminho) => LerJson(File.ReadAllText(caminho));
}
