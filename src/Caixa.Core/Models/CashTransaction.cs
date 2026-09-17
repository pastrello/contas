namespace Caixa.Core.Models;

/// <summary>Lançamento financeiro.</summary>
public sealed class CashTransaction
{
    public long Id { get; set; }
    public long AccountId { get; set; }
    public DateOnly Date { get; set; } = DateOnly.FromDateTime(DateTime.Today);
    public string DocumentNumber { get; set; } = string.Empty;
    public long HistoryId { get; set; }
    public long AmountCents { get; set; }
    public bool IsCleared { get; set; }
    public bool IsArchived { get; set; }
    public bool LegacyRolledIntoOpeningBalance { get; set; }
    public string Description { get; set; } = string.Empty;
    public string? LegacySource { get; set; }
    public int? LegacyRecordNumber { get; set; }

    public string HistoryCode { get; set; } = string.Empty;
    public string HistoryDescription { get; set; } = string.Empty;
    public int Direction { get; set; }
    public string AccountCode { get; set; } = string.Empty;
    public string AccountName { get; set; } = string.Empty;
}
