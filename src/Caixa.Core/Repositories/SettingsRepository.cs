using Caixa.Core.Data;
using Caixa.Core.Models;

namespace Caixa.Core.Repositories;

/// <summary>Parâmetros gerais do sistema.</summary>
public sealed class SettingsRepository
{
    private readonly Database _db;
    public SettingsRepository(Database db) => _db = db;

    public AppSettings Get()
    {
        using var cn = _db.OpenConnection();
        using var cmd = cn.CreateCommand();
        cmd.CommandText = "SELECT value FROM settings WHERE key='CompanyName';";
        return new AppSettings { CompanyName = Convert.ToString(cmd.ExecuteScalar()) ?? "CONTROLE BANCÁRIO" };
    }

    public void Save(AppSettings settings)
    {
        using var cn = _db.OpenConnection();
        using var cmd = cn.CreateCommand();
        cmd.CommandText = """
            INSERT INTO settings(key,value) VALUES('CompanyName',$v)
            ON CONFLICT(key) DO UPDATE SET value=excluded.value;
            """;
        cmd.Parameters.AddWithValue("$v", settings.CompanyName.Trim());
        cmd.ExecuteNonQuery();
    }
}
