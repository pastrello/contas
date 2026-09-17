using Caixa.Core.Data;
using Caixa.Core.Models;
using Microsoft.Data.Sqlite;

namespace Caixa.Core.Repositories;

/// <summary>CRUD de históricos de entrada e saída.</summary>
public sealed class HistoryRepository
{
    private readonly Database _db;
    public HistoryRepository(Database db) => _db = db;

    public List<HistoryItem> GetAll(bool activeOnly = false)
    {
        using var cn = _db.OpenConnection();
        using var cmd = cn.CreateCommand();
        cmd.CommandText = """
            SELECT id, code, description, direction, is_active
            FROM histories
            WHERE ($activeOnly = 0 OR is_active = 1)
            ORDER BY CAST(code AS INTEGER), code, description;
            """;
        cmd.Parameters.AddWithValue("$activeOnly", activeOnly ? 1 : 0);
        using var rd = cmd.ExecuteReader();
        var list = new List<HistoryItem>();
        while (rd.Read()) list.Add(Read(rd));
        return list;
    }

    public HistoryItem? GetById(long id)
    {
        using var cn = _db.OpenConnection();
        using var cmd = cn.CreateCommand();
        cmd.CommandText = "SELECT id, code, description, direction, is_active FROM histories WHERE id=$id;";
        cmd.Parameters.AddWithValue("$id", id);
        using var rd = cmd.ExecuteReader();
        return rd.Read() ? Read(rd) : null;
    }

    public long Save(HistoryItem item)
    {
        item.Code = item.Code.Trim().PadLeft(2, '0');
        if (string.IsNullOrWhiteSpace(item.Description)) throw new InvalidOperationException("Informe a descrição do histórico.");
        item.Direction = item.Direction >= 0 ? 1 : -1;

        using var cn = _db.OpenConnection();
        using var cmd = cn.CreateCommand();
        if (item.Id == 0)
        {
            cmd.CommandText = """
                INSERT INTO histories(code,description,direction,is_active)
                VALUES($code,$description,$direction,$active);
                SELECT last_insert_rowid();
                """;
        }
        else
        {
            cmd.CommandText = """
                UPDATE histories SET code=$code,description=$description,direction=$direction,is_active=$active
                WHERE id=$id;
                SELECT $id;
                """;
            cmd.Parameters.AddWithValue("$id", item.Id);
        }
        cmd.Parameters.AddWithValue("$code", item.Code);
        cmd.Parameters.AddWithValue("$description", item.Description.Trim());
        cmd.Parameters.AddWithValue("$direction", item.Direction);
        cmd.Parameters.AddWithValue("$active", item.IsActive ? 1 : 0);
        item.Id = Convert.ToInt64(cmd.ExecuteScalar());
        return item.Id;
    }

    public void Delete(long id)
    {
        using var cn = _db.OpenConnection();
        using var check = cn.CreateCommand();
        check.CommandText = "SELECT COUNT(*) FROM transactions WHERE history_id=$id;";
        check.Parameters.AddWithValue("$id", id);
        if (Convert.ToInt64(check.ExecuteScalar()) > 0)
            throw new InvalidOperationException("O histórico possui lançamentos e não pode ser excluído. Desative-o em vez disso.");
        using var cmd = cn.CreateCommand();
        cmd.CommandText = "DELETE FROM histories WHERE id=$id;";
        cmd.Parameters.AddWithValue("$id", id);
        cmd.ExecuteNonQuery();
    }

    private static HistoryItem Read(SqliteDataReader rd) => new()
    {
        Id = rd.GetInt64(0),
        Code = rd.GetString(1),
        Description = rd.GetString(2),
        Direction = rd.GetInt32(3),
        IsActive = rd.GetInt64(4) != 0
    };
}
