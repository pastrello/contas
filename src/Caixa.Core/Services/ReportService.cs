using Caixa.Core.Models;
using Caixa.Core.Repositories;

namespace Caixa.Core.Services;

/// <summary>
/// Consultas e relatórios financeiros.
/// A interface decide como exibir/exportar os dados; as regras ficam aqui.
/// </summary>
public sealed class ReportService
{
    private readonly AccountRepository _accounts;
    private readonly TransactionRepository _transactions;
    private readonly BalanceService _balances;

    public ReportService(AccountRepository accounts, TransactionRepository transactions, BalanceService balances)
    {
        _accounts = accounts;
        _transactions = transactions;
        _balances = balances;
    }

    public List<StatementLine> Statement(long accountId, DateOnly start, DateOnly end)
        => _balances.BuildStatement(accountId, start, end);

    public List<CashTransaction> Consistency(long accountId, DateOnly start, DateOnly end)
        => _transactions.GetByPeriod(accountId, start, end, cleared: false)
            .Where(t => !t.LegacyRolledIntoOpeningBalance).ToList();

    /// <summary>Previsão de cheques: histórico 51 ainda não contabilizado.</summary>
    public List<CashTransaction> CheckForecast(long accountId, DateOnly start, DateOnly end)
        => _transactions.GetByPeriod(accountId, start, end, cleared: false)
            .Where(t => t.HistoryCode == "51" && !t.LegacyRolledIntoOpeningBalance).ToList();

    public List<CashTransaction> Archived(long accountId, DateOnly start, DateOnly end)
        => _transactions.GetByPeriod(accountId, start, end, archived: true);

    public List<CashTransaction> ByHistories(long accountId, DateOnly start, DateOnly end, IReadOnlyCollection<long> historyIds)
        => _transactions.GetByPeriod(accountId, start, end, historyIds: historyIds);

    public List<CashTransaction> SearchTransactions(TransactionSearchCriteria criteria)
        => _transactions.Search(criteria);

    public Account GetAccount(long id) => _accounts.GetById(id) ?? throw new InvalidOperationException("Conta não encontrada.");
}
