namespace Desafio.Tests;

internal static class Caminhos
{
    public static string Dados(string arquivo) => Path.Combine(AppContext.BaseDirectory, "data", arquivo);
}
