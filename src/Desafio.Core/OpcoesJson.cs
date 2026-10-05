using System.Text.Json;

namespace Desafio.Core;

internal static class OpcoesJson
{
    public static readonly JsonSerializerOptions Padrao = new()
    {
        PropertyNameCaseInsensitive = true
    };
}
