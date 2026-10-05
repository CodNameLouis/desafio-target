using System.Text.Json;

namespace Desafio.Core.Comissoes;

public static class LeitorVendas
{
    private sealed record Arquivo(List<Venda>? Vendas);

    public static IReadOnlyList<Venda> LerJson(string json)
    {
        var arquivo = JsonSerializer.Deserialize<Arquivo>(json, OpcoesJson.Padrao)
            ?? throw new FormatException("JSON de vendas vazio ou inválido.");

        return arquivo.Vendas ?? throw new FormatException("O JSON não possui a propriedade \"vendas\".");
    }

    public static IReadOnlyList<Venda> LerArquivo(string caminho) => LerJson(File.ReadAllText(caminho));
}
