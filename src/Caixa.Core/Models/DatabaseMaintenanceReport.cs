namespace Caixa.Core.Models;

/// <summary>Retrato técnico do banco SQLite usado pela tela de manutenção.</summary>
public sealed record DatabaseMaintenanceReport(
    string DatabasePath,
    long DatabaseSizeBytes,
    int UserVersion,
    string JournalMode,
    long PageSizeBytes,
    long PageCount,
    long FreePageCount,
    long AccountCount,
    long HistoryCount,
    long TransactionCount,
    string CheckResult);

public sealed record VacuumResult(
    string SafetyBackupPath,
    long SizeBeforeBytes,
    long SizeAfterBytes,
    DatabaseMaintenanceReport Report);
