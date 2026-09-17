namespace Caixa.Core.Models;

/// <summary>Filtros opcionais para a consulta avançada de lançamentos.</summary>
public sealed class TransactionSearchCriteria
{
    public long? AccountId { get; set; }
    public long? HistoryId { get; set; }
    public DateOnly? StartDate { get; set; }
    public DateOnly? EndDate { get; set; }
    public string DocumentContains { get; set; } = string.Empty;
    public string DescriptionContains { get; set; } = string.Empty;
    public bool? Cleared { get; set; }
    public bool? Archived { get; set; }
    public long? MinimumAmountCents { get; set; }
    public long? MaximumAmountCents { get; set; }
    public bool IncludeLegacyRolled { get; set; }
}
