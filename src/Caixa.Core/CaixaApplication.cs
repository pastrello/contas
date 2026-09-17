using Caixa.Core.Data;
using Caixa.Core.Repositories;
using Caixa.Core.Services;

namespace Caixa.Core;

/// <summary>
/// Ponto de composição da aplicação. Mantém a criação das dependências fora das telas.
/// </summary>
public sealed class CaixaApplication
{
    public CaixaApplication(string? databasePath = null)
    {
        Database = new Database(databasePath);
        Database.Initialize();
        Accounts = new AccountRepository(Database);
        Histories = new HistoryRepository(Database);
        Transactions = new TransactionRepository(Database);
        Settings = new SettingsRepository(Database);
        Balances = new BalanceService(Accounts, Transactions);
        Reports = new ReportService(Accounts, Transactions, Balances);
        Backup = new BackupService(Database);
        Maintenance = new DatabaseMaintenanceService(Database, Backup);
    }

    public Database Database { get; }
    public AccountRepository Accounts { get; }
    public HistoryRepository Histories { get; }
    public TransactionRepository Transactions { get; }
    public SettingsRepository Settings { get; }
    public BalanceService Balances { get; }
    public ReportService Reports { get; }
    public BackupService Backup { get; }
    public DatabaseMaintenanceService Maintenance { get; }
}
