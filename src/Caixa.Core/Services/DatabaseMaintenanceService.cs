using Caixa.Core.Data;
using Caixa.Core.Models;
using Microsoft.Data.Sqlite;

namespace Caixa.Core.Services;

/// <summary>
/// Rotinas de diagnóstico/manutenção do SQLite.
/// PRAGMA optimize é a manutenção leve recomendada para índices/estatísticas.
/// VACUUM regrava o arquivo inteiro e por isso sempre é precedido de backup.
/// </summary>
public sealed class DatabaseMaintenanceService
{
    private readonly Database _db;
    private readonly BackupService _backup;

    public DatabaseMaintenanceService(Database db, BackupService backup)
    {
        _db = db;
        _backup = backup;
    }

    public DatabaseMaintenanceReport QuickCheck() => Inspect(fullIntegrityCheck: false);

    public DatabaseMaintenanceReport IntegrityCheck() => Inspect(fullIntegrityCheck: true);

    public DatabaseMaintenanceReport Optimize()
    {
        using (var cn = _db.OpenConnection())
        {
            using var cmd = cn.CreateCommand();
            cmd.CommandText = "PRAGMA optimize; PRAGMA wal_checkpoint(TRUNCATE);";
            cmd.ExecuteNonQuery();
        }

        SqliteConnection.ClearAllPools();
        return QuickCheck();
    }

    public VacuumResult VacuumWithSafetyBackup()
    {
        var before = File.Exists(_db.DatabasePath) ? new FileInfo(_db.DatabasePath).Length : 0;
        var safety = _backup.CreateAutomatic(AppPaths.PreMaintenanceBackupDirectory, "pre-manutencao");

        SqliteConnection.ClearAllPools();
        using (var cn = _db.OpenConnection())
        {
            using var cmd = cn.CreateCommand();
            cmd.CommandText = "VACUUM;";
            cmd.ExecuteNonQuery();
        }
        SqliteConnection.ClearAllPools();

        var report = Optimize();
        var after = File.Exists(_db.DatabasePath) ? new FileInfo(_db.DatabasePath).Length : 0;
        return new VacuumResult(safety.ZipPath, before, after, report);
    }

    private DatabaseMaintenanceReport Inspect(bool fullIntegrityCheck)
    {
        using var cn = _db.OpenConnection();

        static long ScalarLong(SqliteConnection connection, string sql)
        {
            using var cmd = connection.CreateCommand();
            cmd.CommandText = sql;
            return Convert.ToInt64(cmd.ExecuteScalar());
        }

        static string ScalarText(SqliteConnection connection, string sql)
        {
            using var cmd = connection.CreateCommand();
            cmd.CommandText = sql;
            return Convert.ToString(cmd.ExecuteScalar()) ?? string.Empty;
        }

        var checkSql = fullIntegrityCheck ? "PRAGMA integrity_check;" : "PRAGMA quick_check;";
        var check = ScalarText(cn, checkSql);

        return new DatabaseMaintenanceReport(
            _db.DatabasePath,
            File.Exists(_db.DatabasePath) ? new FileInfo(_db.DatabasePath).Length : 0,
            Convert.ToInt32(ScalarLong(cn, "PRAGMA user_version;")),
            ScalarText(cn, "PRAGMA journal_mode;"),
            ScalarLong(cn, "PRAGMA page_size;"),
            ScalarLong(cn, "PRAGMA page_count;"),
            ScalarLong(cn, "PRAGMA freelist_count;"),
            ScalarLong(cn, "SELECT COUNT(*) FROM accounts;"),
            ScalarLong(cn, "SELECT COUNT(*) FROM histories;"),
            ScalarLong(cn, "SELECT COUNT(*) FROM transactions;"),
            check);
    }
}
