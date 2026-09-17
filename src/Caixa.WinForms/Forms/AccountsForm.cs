using Caixa.Core;
using Caixa.Core.Models;
using Caixa.Core.Utilities;
using Caixa.WinForms.Utilities;

namespace Caixa.WinForms.Forms;

/// <summary>Cadastro de contas financeiras.</summary>
public partial class AccountsForm : Form
{
    private readonly CaixaApplication _app;
    private long _currentId;
    private Account? _loaded;
    private bool _suppressGridSelection;
    private readonly UnsavedChangesGuard _unsaved = new();

    public AccountsForm(CaixaApplication app)
    {
        _app = app;
        InitializeComponent();
        UiHelpers.ConfigureGrid(grid);
        grid.Columns.Add(UiHelpers.TextColumn("Código", nameof(AccountRow.Code), 60));
        grid.Columns.Add(UiHelpers.TextColumn("Número", nameof(AccountRow.Number), 110));
        grid.Columns.Add(UiHelpers.TextColumn("Banco / descrição", nameof(AccountRow.Name), 260));
        grid.Columns.Add(UiHelpers.MoneyColumn("Inicial", nameof(AccountRow.Opening), 120));
        grid.Columns.Add(UiHelpers.MoneyColumn("Bancário", nameof(AccountRow.Cleared), 120));
        grid.Columns.Add(UiHelpers.MoneyColumn("Real", nameof(AccountRow.Real), 120));
        grid.Columns.Add(UiHelpers.CheckColumn("Ativa", nameof(AccountRow.Active), 60));
        Load += (_, _) => { Reload(); NewItem(); };
        FormClosing += (_, e) => { if (!_unsaved.ConfirmClose(this, CaptureState())) e.Cancel = true; };
    }

    private void Reload(long? selectId = null)
    {
        _suppressGridSelection = true;
        try
        {
            var rows = _app.Accounts.GetAll().Select(a =>
            {
                var b = _app.Balances.Calculate(a.Id);
                return new AccountRow
                {
                    Id = a.Id, Code = a.Code, Number = a.Number, Name = a.Name,
                    Opening = Money.FromCents(a.OpeningBalanceCents),
                    Cleared = Money.FromCents(b.ClearedBalanceCents),
                    Real = Money.FromCents(b.RealBalanceCents), Active = a.IsActive
                };
            }).ToList();
            grid.DataSource = rows;

            if (selectId.HasValue)
            {
                foreach (DataGridViewRow row in grid.Rows)
                {
                    if ((row.DataBoundItem as AccountRow)?.Id != selectId.Value) continue;
                    row.Selected = true;
                    grid.CurrentCell = row.Cells[0];
                    break;
                }
            }
        }
        finally
        {
            _suppressGridSelection = false;
        }
    }

    private void NewItem()
    {
        _suppressGridSelection = true;
        try
        {
            _currentId = 0;
            _loaded = null;
            grid.CurrentCell = null;
            grid.ClearSelection();
            txtCode.Text = (_app.Accounts.GetAll().Select(a => int.TryParse(a.Code, out var n) ? n : 0).DefaultIfEmpty().Max() + 1).ToString("00");
            txtNumber.Clear();
            txtName.Clear();
            numOpening.Value = 0;
            chkActive.Checked = true;
        }
        finally
        {
            _suppressGridSelection = false;
        }

        _unsaved.Accept(CaptureState());
        UpdateModeTitle();
        txtCode.Focus();
    }

    private void SelectCurrent()
    {
        if (_suppressGridSelection) return;
        if (grid.CurrentRow?.DataBoundItem is not AccountRow row) return;
        var a = _app.Accounts.GetById(row.Id);
        if (a is null) return;
        _currentId = a.Id;
        _loaded = a;
        txtCode.Text = a.Code;
        txtNumber.Text = a.Number;
        txtName.Text = a.Name;
        numOpening.Value = Clamp(Money.FromCents(a.OpeningBalanceCents), numOpening.Minimum, numOpening.Maximum);
        chkActive.Checked = a.IsActive;
        _unsaved.Accept(CaptureState());
        UpdateModeTitle();
    }

    private bool SaveItem()
    {
        try
        {
            var a = _loaded ?? new Account();
            a.Id = _currentId;
            a.Code = txtCode.Text;
            a.Number = txtNumber.Text;
            a.Name = txtName.Text;
            a.OpeningBalanceCents = Money.ToCents(numOpening.Value);
            a.IsActive = chkActive.Checked;
            var id = _app.Accounts.Save(a);
            _loaded = a;
            _currentId = id;
            Reload(id);
            _unsaved.Accept(CaptureState());
            UpdateModeTitle();
            return true;
        }
        catch (Exception ex)
        {
            UiHelpers.ShowError(this, ex);
            return false;
        }
    }

    private void DeleteItem()
    {
        if (_currentId == 0) return;
        if (MessageBox.Show(this, "Excluir esta conta?", "Confirmação", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;
        try { _app.Accounts.Delete(_currentId); NewItem(); Reload(); }
        catch (Exception ex) { UiHelpers.ShowError(this, ex); }
    }

    private string CaptureState()
        => string.Join("\u001F", _currentId, txtCode.Text, txtNumber.Text, txtName.Text, numOpening.Value, chkActive.Checked);

    private void UpdateModeTitle()
        => Text = _currentId == 0 ? "Contas - NOVO REGISTRO" : $"Contas - EDITANDO {txtCode.Text}";

    private static decimal Clamp(decimal value, decimal min, decimal max) => Math.Min(max, Math.Max(min, value));

    private sealed class AccountRow
    {
        public long Id { get; init; }
        public string Code { get; init; } = string.Empty;
        public string Number { get; init; } = string.Empty;
        public string Name { get; init; } = string.Empty;
        public decimal Opening { get; init; }
        public decimal Cleared { get; init; }
        public decimal Real { get; init; }
        public bool Active { get; init; }
    }
}
