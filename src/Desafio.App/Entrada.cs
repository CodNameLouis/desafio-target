using System.Globalization;

namespace Desafio.App;

/// <summary>Leitura validada de dados digitados no console.</summary>
internal static class Entrada
{
    private static readonly CultureInfo PtBr = CultureInfo.GetCultureInfo("pt-BR");

    public static string Texto(string rotulo)
    {
        while (true)
        {
            Console.Write(rotulo);
            var linha = Console.ReadLine();
            if (linha is null) // fim da entrada (Ctrl+Z ou stdin encerrado)
                Environment.Exit(0);

            var texto = linha.Trim();
            if (texto.Length > 0)
                return texto;

            Erro("Campo obrigatório.");
        }
    }

    public static int Inteiro(string rotulo, int minimo = int.MinValue)
    {
        while (true)
        {
            var texto = Texto(rotulo);
            if (int.TryParse(texto, NumberStyles.Integer, PtBr, out var valor) && valor >= minimo)
                return valor;

            Erro(minimo > int.MinValue ? $"Informe um número inteiro maior ou igual a {minimo}." : "Informe um número inteiro.");
        }
    }

    /// <summary>Aceita "1.234,56", "1234,56" ou "1234.56".</summary>
    public static decimal Valor(string rotulo)
    {
        while (true)
        {
            var texto = Texto(rotulo).Replace("R$", "").Trim();
            if (TentarConverterValor(texto, out var valor) && valor >= 0)
                return valor;

            Erro("Valor inválido. Exemplos: 1500,75 ou 1500.75");
        }
    }

    public static DateOnly Data(string rotulo)
    {
        string[] formatos = ["dd/MM/yyyy", "d/M/yyyy", "dd-MM-yyyy", "yyyy-MM-dd"];
        while (true)
        {
            var texto = Texto(rotulo);
            if (DateOnly.TryParseExact(texto, formatos, PtBr, DateTimeStyles.None, out var data))
                return data;

            Erro("Data inválida. Use o formato dd/mm/aaaa.");
        }
    }

    /// <summary>Console.Clear lança exceção quando a saída está redirecionada (pipe, CI).</summary>
    public static void LimparTela()
    {
        if (!Console.IsOutputRedirected)
            Console.Clear();
    }

    public static void Pausar()
    {
        Console.WriteLine();
        Console.Write("Pressione ENTER para continuar...");
        Console.ReadLine();
    }

    public static void Erro(string mensagem)
    {
        var corAnterior = Console.ForegroundColor;
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"  {mensagem}");
        Console.ForegroundColor = corAnterior;
    }

    public static void Sucesso(string mensagem)
    {
        var corAnterior = Console.ForegroundColor;
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine(mensagem);
        Console.ForegroundColor = corAnterior;
    }

    private static bool TentarConverterValor(string texto, out decimal valor)
    {
        // Somente ponto e sem vírgula: trata o ponto como separador decimal (ex.: 1500.75).
        if (!texto.Contains(',') && texto.Count(c => c == '.') == 1)
            return decimal.TryParse(texto, NumberStyles.Number, CultureInfo.InvariantCulture, out valor);

        return decimal.TryParse(texto, NumberStyles.Number, PtBr, out valor);
    }
}
