namespace Desafio.Core.Juros;

public sealed record ResultadoJuros(
    decimal ValorOriginal,
    DateOnly Vencimento,
    DateOnly DataCalculo,
    int DiasAtraso,
    decimal Juros,
    decimal ValorAtualizado);

/// <summary>
/// Calcula juros simples de 2,5% ao dia sobre o valor original,
/// contados a partir do dia seguinte ao vencimento.
/// </summary>
public static class CalculadoraJuros
{
    public const decimal TaxaDiaria = 0.025m;

    public static ResultadoJuros Calcular(decimal valor, DateOnly vencimento, DateOnly dataCalculo)
    {
        if (valor < 0)
            throw new ArgumentOutOfRangeException(nameof(valor), "O valor não pode ser negativo.");

        var diasAtraso = Math.Max(0, dataCalculo.DayNumber - vencimento.DayNumber);
        var juros = Math.Round(valor * TaxaDiaria * diasAtraso, 2, MidpointRounding.AwayFromZero);

        return new ResultadoJuros(valor, vencimento, dataCalculo, diasAtraso, juros, valor + juros);
    }

    public static ResultadoJuros CalcularHoje(decimal valor, DateOnly vencimento, TimeProvider? relogio = null)
    {
        var hoje = DateOnly.FromDateTime((relogio ?? TimeProvider.System).GetLocalNow().DateTime);
        return Calcular(valor, vencimento, hoje);
    }
}
