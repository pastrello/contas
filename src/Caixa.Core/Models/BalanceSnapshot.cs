namespace Caixa.Core.Models;

/// <summary>Resultado do cálculo de saldos de uma conta.</summary>
public sealed record BalanceSnapshot(
    long OpeningBalanceCents,
    long RealBalanceCents,
    long ClearedBalanceCents,
    long PendingNetCents);
