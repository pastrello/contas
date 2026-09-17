using System.Globalization;

namespace Caixa.Core.Utilities;

/// <summary>
/// Conversões monetárias. Internamente o banco guarda centavos como INTEGER,
/// evitando erros de ponto flutuante em valores financeiros.
/// </summary>
public static class Money
{
    public static long ToCents(decimal value) => checked((long)Math.Round(value * 100m, 0, MidpointRounding.AwayFromZero));
    public static decimal FromCents(long cents) => cents / 100m;
    public static string Format(long cents) => FromCents(cents).ToString("C2", CultureInfo.GetCultureInfo("pt-BR"));
}
