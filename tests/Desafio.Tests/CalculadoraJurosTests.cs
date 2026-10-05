using Desafio.Core.Juros;

namespace Desafio.Tests;

public class CalculadoraJurosTests
{
    private static readonly DateOnly Hoje = new(2026, 10, 5);

    [Fact]
    public void Calcula_juros_simples_de_2_5_porcento_ao_dia()
    {
        var r = CalculadoraJuros.Calcular(1000m, vencimento: new DateOnly(2026, 9, 25), dataCalculo: Hoje);

        Assert.Equal(10, r.DiasAtraso);
        Assert.Equal(250m, r.Juros);          // 1000 * 2,5% * 10
        Assert.Equal(1250m, r.ValorAtualizado);
    }

    [Fact]
    public void Um_dia_de_atraso()
    {
        var r = CalculadoraJuros.Calcular(100m, new DateOnly(2026, 10, 4), Hoje);

        Assert.Equal(1, r.DiasAtraso);
        Assert.Equal(2.50m, r.Juros);
    }

    [Theory]
    [InlineData(2026, 10, 5)]   // vence hoje
    [InlineData(2026, 12, 1)]   // vence no futuro
    public void Sem_atraso_nao_ha_juros(int ano, int mes, int dia)
    {
        var r = CalculadoraJuros.Calcular(500m, new DateOnly(ano, mes, dia), Hoje);

        Assert.Equal(0, r.DiasAtraso);
        Assert.Equal(0m, r.Juros);
        Assert.Equal(500m, r.ValorAtualizado);
    }

    [Fact]
    public void Juros_sao_arredondados_para_centavos()
    {
        var r = CalculadoraJuros.Calcular(33.33m, new DateOnly(2026, 10, 2), Hoje);

        Assert.Equal(3, r.DiasAtraso);
        Assert.Equal(2.50m, r.Juros);         // 33,33 * 0,075 = 2,49975
    }

    [Fact]
    public void Valor_negativo_e_rejeitado()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => CalculadoraJuros.Calcular(-1m, Hoje, Hoje));
    }
}
