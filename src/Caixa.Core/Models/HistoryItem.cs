namespace Caixa.Core.Models;

/// <summary>Histórico/tipo de lançamento com direção explícita.</summary>
public sealed class HistoryItem
{
    public long Id { get; set; }
    public string Code { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int Direction { get; set; } = 1;
    public bool IsActive { get; set; } = true;

    public string DirectionText => Direction >= 0 ? "Entrada" : "Saída";
    public override string ToString() => $"{Code} - {Description}";
}
