using Caixa.Core.Data;
using Caixa.Core.Models;
using Microsoft.Data.Sqlite;

namespace Caixa.Core.Repositories;

/// <summary>CRUD de contas financeiras.</summary>
public sealed class AccountRepository
{
    private readonly Database _db;
    public AccountRepository(Database db) => _db = db;

    public List<Account> GetAll(bool activeOnly = false)
    {
        using var cn = _db.OpenConnection();
        using var cmd = cn.CreateCommand();
        cmd.CommandText = """
            SELECT id, code, number, name, opening_balance_cents,
                   legacy_stored_real_cents, legacy_stored_cleared_cents, is_active
            FROM accounts
            WHERE ($activeOnly = 0 OR is_active = 1)
            ORDER BY CAST(code AS INTEGER), code, name;
            """;
        cmd.Parameters.AddWithValue("$activeOnly", activeOnly ? 1 : 0);
        using var rd = cmd.ExecuteReader();
        var list = new List<Account>();
        while (rd.Read()) list.Add(Read(rd));
        return list;
    }

    public Account? GetById(long id)
    {
        using var cn = _db.OpenConnection();
        using var cmd = cn.CreateCommand();
        cmd.CommandText = """
            SELECT id, code, number, name, opening_balance_cents,
                   legacy_stored_real_cents, legacy_stored_cleared_cents, is_active
            FROM accounts WHERE id=$id;
            """;
        cmd.Parameters.AddWithValue("$id", id);
        using var rd = cmd.ExecuteReader();
        return rd.Read() ? Read(rd) : null;
    }

    public long Save(Account account)
    {
        Validate(account);
        using var cn = _db.OpenConnection();
        using var cmd = cn.CreateCommand();
        if (account.Id == 0)
        {
            cmd.CommandText = """
                INSERT INTO accounts(code, number, name, opening_balance_cents,
                    legacy_stored_real_cents, legacy_stored_cleared_cents, is_active)
                VALUES($code,$number,$name,$opening,$legacyReal,$legacyCleared,$active);
                SELECT last_insert_rowid();
                """;
        }
        else
        {
            cmd.CommandText = """
                UPDATE accounts SET code=$code, number=$number, name=$name,
                    opening_balance_cents=$opening,
                    legacy_stored_real_cents=$legacyReal,
                    legacy_stored_cleared_cents=$legacyCleared,
                    is_active=$active
                WHERE id=$id;
                SELECT $id;
                """;
            cmd.Parameters.AddWithValue("$id", account.Id);
        }
        AddParams(cmd, account);
        account.Id = Convert.ToInt64(cmd.ExecuteScalar());
        return account.Id;
    }

    public void Delete(long id)
    {
        using var cn = _db.OpenConnection();
        using var check = cn.CreateCommand();
        check.CommandText = "SELECT COUNT(*) FROM transactions WHERE account_id=$id;";
        check.Parameters.AddWithValue("$id", id);
        if (Convert.ToInt64(check.ExecuteScalar()) > 0)
            throw new InvalidOperationException("A conta possui lançamentos e não pode ser excluída. Desative-a em vez disso.");

        using var cmd = cn.CreateCommand();
        cmd.CommandText = "DELETE FROM accounts WHERE id=$id;";
        cmd.Parameters.AddWithValue("$id", id);
        cmd.ExecuteNonQuery();
    }

    private static void Validate(Account a)
    {
        if (string.IsNullOrWhiteSpace(a.Code)) throw new InvalidOperationException("Informe o código da conta.");
        a.Code = a.Code.Trim().PadLeft(2, '0');
        if (string.IsNullOrWhiteSpace(a.Name)) throw new InvalidOperationException("Informe a descrição/banco da conta.");
    }

    private static void AddParams(SqliteCommand cmd, Account a)
    {
        cmd.Parameters.AddWithValue("$code", a.Code.Trim());
        cmd.Parameters.AddWithValue("$number", a.Number.Trim());
        cmd.Parameters.AddWithValue("$name", a.Name.Trim());
        cmd.Parameters.AddWithValue("$opening", a.OpeningBalanceCents);
        cmd.Parameters.AddWithValue("$legacyReal", (object?)a.LegacyStoredRealBalanceCents ?? DBNull.Value);
        cmd.Parameters.AddWithValue("$legacyCleared", (object?)a.LegacyStoredClearedBalanceCents ?? DBNull.Value);
        cmd.Parameters.AddWithValue("$active", a.IsActive ? 1 : 0);
    }

    private static Account Read(SqliteDataReader rd) => new()
    {
        Id = rd.GetInt64(0),
        Code = rd.GetString(1),
        Number = rd.GetString(2),
        Name = rd.GetString(3),
        OpeningBalanceCents = rd.GetInt64(4),
        LegacyStoredRealBalanceCents = rd.IsDBNull(5) ? null : rd.GetInt64(5),
        LegacyStoredClearedBalanceCents = rd.IsDBNull(6) ? null : rd.GetInt64(6),
        IsActive = rd.GetInt64(7) != 0
    };
}
