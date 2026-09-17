using Caixa.Core;
using Caixa.Core.Models;
using Caixa.Core.Utilities;
using Caixa.WinForms.Utilities;

namespace Caixa.WinForms.Forms;

/// <summary>Arquiva lançamentos contabilizados sem alterar o saldo.</summary>
public sealed class ArchiveForm : Form
{
    private readonly CaixaApplication _app;
    private readonly ComboBox _account = new();
    private readonly DateTimePicker _start = new();
    private readonly DateTimePicker _end = new();
    private readonly DataGridView _grid = new();
    private readonly Label _summary = new();

    public ArchiveForm(CaixaApplication app)
    {
        _app = app;
        Text = "Baixa / arquivamento de contabilizados"; StartPosition = FormStartPosition.CenterParent; ClientSize = new Size(900, 520);
        Controls.Add(new Label { Text="Conta", Location=new Point(18,18), AutoSize=true });
        _account.Location=new Point(18,40); _account.Size=new Size(350,23); _account.DropDownStyle=ComboBoxStyle.DropDownList;
        Controls.Add(new Label { Text="De", Location=new Point(390,18), AutoSize=true }); _start.Location=new Point(390,40); _start.Size=new Size(110,23); _start.Format=DateTimePickerFormat.Short;
        Controls.Add(new Label { Text="Até", Location=new Point(520,18), AutoSize=true }); _end.Location=new Point(520,40); _end.Size=new Size(110,23); _end.Format=DateTimePickerFormat.Short;
        var load = new Button { Text="Consultar", Location=new Point(650,39), Size=new Size(90,25) };
        var archive = new Button { Text="Arquivar período", Location=new Point(750,39), Size=new Size(125,25) };
        load.Click += (_,_) => Reload(); archive.Click += (_,_) => Archive();
        UiHelpers.ConfigureGrid(_grid); _grid.Location=new Point(18,85); _grid.Size=new Size(857,360); _grid.Anchor=AnchorStyles.Top|AnchorStyles.Bottom|AnchorStyles.Left|AnchorStyles.Right;
        _grid.Columns.Add(UiHelpers.TextColumn("Data", nameof(Row.Date), 90)); _grid.Columns.Add(UiHelpers.TextColumn("Doc.", nameof(Row.Document), 80));
        _grid.Columns.Add(UiHelpers.TextColumn("Histórico", nameof(Row.History), 180)); _grid.Columns.Add(UiHelpers.TextColumn("Complemento", nameof(Row.Description), 320));
        _grid.Columns.Add(UiHelpers.MoneyColumn("Valor", nameof(Row.Amount), 120));
        _summary.Location=new Point(18,465); _summary.Size=new Size(650,25); _summary.Anchor=AnchorStyles.Bottom|AnchorStyles.Left;
        var close = new Button { Text="Fechar", Location=new Point(800,460), Anchor=AnchorStyles.Bottom|AnchorStyles.Right }; close.Click += (_,_) => Close();
        Controls.AddRange([_account,_start,_end,load,archive,_grid,_summary,close]);
        Load += (_,_) => { _account.DataSource=_app.Accounts.GetAll(true); _start.Value=DateTime.Today.AddMonths(-1); _end.Value=DateTime.Today; Reload(); };
    }

    private Account? Account => _account.SelectedItem as Account;
    private void Reload()
    {
        if (Account is null) return;
        var rows = _app.Transactions.GetByPeriod(Account.Id, DateOnly.FromDateTime(_start.Value), DateOnly.FromDateTime(_end.Value), cleared:true, archived:false)
            .Where(t=>!t.LegacyRolledIntoOpeningBalance)
            .Select(t=>new Row { Date=t.Date.ToString("dd/MM/yyyy"), Document=t.DocumentNumber, History=$"{t.HistoryCode} - {t.HistoryDescription}", Description=t.Description, Amount=Money.FromCents(t.AmountCents) }).ToList();
        _grid.DataSource=rows; _summary.Text=$"{rows.Count} lançamento(s) contabilizado(s) ainda não arquivado(s). Total: {rows.Sum(r=>r.Amount):C2}";
    }

    private void Archive()
    {
        if (Account is null) return;
        if (MessageBox.Show(this, "Arquivar todos os lançamentos contabilizados do período?\r\n\r\nO saldo não será alterado e os registros continuarão no SQLite.",
            "Confirmação", MessageBoxButtons.YesNo, MessageBoxIcon.Question)!=DialogResult.Yes) return;
        try
        {
            var n=_app.Transactions.ArchiveCleared(Account.Id,DateOnly.FromDateTime(_start.Value),DateOnly.FromDateTime(_end.Value));
            MessageBox.Show(this,$"{n} lançamento(s) arquivado(s).","Baixa",MessageBoxButtons.OK,MessageBoxIcon.Information); Reload();
        }
        catch(Exception ex){UiHelpers.ShowError(this,ex);}
    }

    private sealed class Row { public string Date{get;init;}=""; public string Document{get;init;}=""; public string History{get;init;}=""; public string Description{get;init;}=""; public decimal Amount{get;init;} }
}
