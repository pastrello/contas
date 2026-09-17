using System.Globalization;
using Caixa.Core.Utilities;

namespace Caixa.WinForms.Utilities;

internal static class UiHelpers
{
    public static void ConfigureGrid(DataGridView grid)
    {
        grid.AllowUserToAddRows = false;
        grid.AllowUserToDeleteRows = false;
        grid.AllowUserToResizeRows = false;
        grid.ReadOnly = true;
        grid.MultiSelect = false;
        grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        grid.AutoGenerateColumns = false;
        grid.RowHeadersVisible = false;
        grid.BackgroundColor = SystemColors.Window;
        grid.BorderStyle = BorderStyle.Fixed3D;
    }

    public static DataGridViewTextBoxColumn TextColumn(string header, string property, int width = 100)
        => new() { HeaderText = header, DataPropertyName = property, Width = width, SortMode = DataGridViewColumnSortMode.Automatic };

    public static DataGridViewCheckBoxColumn CheckColumn(string header, string property, int width = 75)
        => new() { HeaderText = header, DataPropertyName = property, Width = width, SortMode = DataGridViewColumnSortMode.Automatic };

    public static DataGridViewTextBoxColumn MoneyColumn(string header, string property, int width = 120)
    {
        var col = TextColumn(header, property, width);
        col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
        col.DefaultCellStyle.Format = "C2";
        col.DefaultCellStyle.FormatProvider = CultureInfo.GetCultureInfo("pt-BR");
        return col;
    }

    public static void ShowError(IWin32Window owner, Exception ex)
        => MessageBox.Show(owner, ex.Message, "Operação não concluída", MessageBoxButtons.OK, MessageBoxIcon.Warning);

    public static decimal ToDecimal(long cents) => Money.FromCents(cents);
}
