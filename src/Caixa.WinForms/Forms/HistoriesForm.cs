using Caixa.Core;
using Caixa.Core.Models;
using Caixa.WinForms.Utilities;

namespace Caixa.WinForms.Forms;

/// <summary>Cadastro de históricos com direção explícita Entrada/Saída.</summary>
public partial class HistoriesForm : Form
{
    private readonly CaixaApplication _app;
    private long _currentId;
    private bool _suppressGridSelection;
    private readonly UnsavedChangesGuard _unsaved = new();

    public HistoriesForm(CaixaApplication app)
    {
        _app = app;
        InitializeComponent();
        UiHelpers.ConfigureGrid(grid);
        grid.Columns.Add(UiHelpers.TextColumn("Código", nameof(HistoryRow.Code), 70));
        grid.Columns.Add(UiHelpers.TextColumn("Descrição", nameof(HistoryRow.Description), 300));
        grid.Columns.Add(UiHelpers.TextColumn("Tipo", nameof(HistoryRow.Direction), 100));
        grid.Columns.Add(UiHelpers.CheckColumn("Ativo", nameof(HistoryRow.Active), 70));
        cmbDirection.DataSource = new[] { new DirectionOption(1, "Entrada"), new DirectionOption(-1, "Saída") };
        Load += (_, _) => { Reload(); NewItem(); };
        FormClosing += (_, e) => { if (!_unsaved.ConfirmClose(this, CaptureState())) e.Cancel = true; };
    }

    private void Reload(long? selectId = null)
    {
        _suppressGridSelection = true;
        try
        {
            var rows = _app.Histories.GetAll().Select(h => new HistoryRow
            {
                Id = h.Id, Code = h.Code, Description = h.Description,
                Direction = h.DirectionText, Active = h.IsActive
            }).ToList();
            grid.DataSource = rows;

            if (selectId.HasValue)
            {
                foreach (DataGridViewRow r in grid.Rows)
                {
                    if ((r.DataBoundItem as HistoryRow)?.Id != selectId.Value) continue;
                    r.Selected = true;
                    grid.CurrentCell = r.Cells[0];
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
            grid.CurrentCell = null;
            grid.ClearSelection();
            txtCode.Text = (_app.Histories.GetAll().Select(h => int.TryParse(h.Code, out var n) ? n : 0).DefaultIfEmpty().Max() + 1).ToString("00");
            txtDescription.Clear();
            cmbDirection.SelectedIndex = 0;
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
        if (grid.CurrentRow?.DataBoundItem is not HistoryRow row) return;
        var h = _app.Histories.GetById(row.Id); if (h is null) return;
        _currentId = h.Id; txtCode.Text = h.Code; txtDescription.Text = h.Description;
        cmbDirection.SelectedIndex = h.Direction >= 0 ? 0 : 1; chkActive.Checked = h.IsActive;
        _unsaved.Accept(CaptureState());
        UpdateModeTitle();
    }

    private bool SaveItem()
    {
        try
        {
            var opt = (DirectionOption)cmbDirection.SelectedItem!;
            var h = new HistoryItem { Id = _currentId, Code = txtCode.Text, Description = txtDescription.Text, Direction = opt.Value, IsActive = chkActive.Checked };
            var id = _app.Histories.Save(h); _currentId = id; Reload(id);
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
        if (MessageBox.Show(this, "Excluir este histórico?", "Confirmação", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;
        try { _app.Histories.Delete(_currentId); NewItem(); Reload(); }
        catch (Exception ex) { UiHelpers.ShowError(this, ex); }
    }

    private string CaptureState()
        => string.Join("\u001F", _currentId, txtCode.Text, txtDescription.Text, cmbDirection.SelectedIndex, chkActive.Checked);

    private void UpdateModeTitle()
        => Text = _currentId == 0 ? "Históricos - NOVO REGISTRO" : $"Históricos - EDITANDO {txtCode.Text}";

    private sealed record DirectionOption(int Value, string Text) { public override string ToString() => Text; }
    private sealed class HistoryRow
    {
        public long Id { get; init; }
        public string Code { get; init; } = string.Empty;
        public string Description { get; init; } = string.Empty;
        public string Direction { get; init; } = string.Empty;
        public bool Active { get; init; }
    }
}
