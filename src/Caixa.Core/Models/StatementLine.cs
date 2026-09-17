namespace Caixa.Core.Models;

/// <summary>Linha de extrato com saldo acumulado.</summary>
public sealed class StatementLine
{
    public long TransactionId { get; init; }
    public DateOnly Date { get; init; }
    public string DocumentNumber { get; init; } = string.Empty;
    public string HistoryCode { get; init; } = string.Empty;
    public string HistoryDescription { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
    public long AmountCents { get; init; }
    public int Direction { get; init; }
    public bool IsCleared { get; init; }
    public bool IsArchived { get; init; }
    public long RunningBalanceCents { get; init; }
    public long SignedAmountCents => AmountCents * Direction;
}
