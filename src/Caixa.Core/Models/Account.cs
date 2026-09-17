namespace Caixa.Core.Models;

/// <summary>Conta financeira. Os saldos correntes são calculados a partir do saldo inicial e dos lançamentos.</summary>
public sealed class Account
{
    public long Id { get; set; }
    public string Code { get; set; } = string.Empty;
    public string Number { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public long OpeningBalanceCents { get; set; }
    public long? LegacyStoredRealBalanceCents { get; set; }
    public long? LegacyStoredClearedBalanceCents { get; set; }
    public bool IsActive { get; set; } = true;

    public override string ToString() => $"{Code} - {Number} - {Name}";
}
