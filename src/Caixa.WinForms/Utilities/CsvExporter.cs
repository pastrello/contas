using System.Text;

namespace Caixa.WinForms.Utilities;

/// <summary>Exportação simples da grade para CSV UTF-8 com separador ';', amigável ao Excel pt-BR.</summary>
internal static class CsvExporter
{
    public static void Export(DataGridView grid, string path)
    {
        static string Q(object? value)
        {
            var s = Convert.ToString(value) ?? string.Empty;
            return '"' + s.Replace("\"", "\"\"") + '"';
        }

        using var writer = new StreamWriter(path, false, new UTF8Encoding(encoderShouldEmitUTF8Identifier: true));
        var visible = grid.Columns.Cast<DataGridViewColumn>().Where(c => c.Visible).ToList();
        writer.WriteLine(string.Join(';', visible.Select(c => Q(c.HeaderText))));
        foreach (DataGridViewRow row in grid.Rows)
            writer.WriteLine(string.Join(';', visible.Select(c => Q(row.Cells[c.Index].FormattedValue))));
    }
}
