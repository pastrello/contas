using Caixa.Core;
using Caixa.Core.Models;
using Caixa.WinForms.Utilities;

namespace Caixa.WinForms.Forms;

/// <summary>Parâmetros gerais da aplicação.</summary>
public sealed class SettingsForm : Form
{
    private readonly CaixaApplication _app;
    private readonly TextBox _company = new();
    private readonly UnsavedChangesGuard _unsaved = new();

    public SettingsForm(CaixaApplication app)
    {
        _app = app;
        Text = "Parâmetros do sistema"; StartPosition = FormStartPosition.CenterParent; ClientSize = new Size(560, 155);
        var label = new Label { Text = "Razão social / nome exibido", Location = new Point(20, 20), AutoSize = true };
        _company.Location = new Point(20, 45); _company.Size = new Size(515, 23); _company.MaxLength = 120;
        var save = new Button { Text = "Salvar", Location = new Point(375, 100) };
        var close = new Button { Text = "Fechar", Location = new Point(460, 100) };
        save.Click += (_, _) => Save(); close.Click += (_, _) => Close();
        Controls.AddRange([label, _company, save, close]);
        Load += (_, _) => { _company.Text = _app.Settings.Get().CompanyName; _unsaved.Accept(CaptureState()); };
        FormClosing += (_, e) => { if (!_unsaved.ConfirmClose(this, CaptureState())) e.Cancel = true; };
    }

    private string CaptureState() => _company.Text;

    private void Save()
    {
        try
        {
            if (string.IsNullOrWhiteSpace(_company.Text)) throw new InvalidOperationException("Informe o nome da empresa.");
            _app.Settings.Save(new AppSettings { CompanyName = _company.Text });
            _unsaved.Accept(CaptureState());
            DialogResult = DialogResult.OK; Close();
        }
        catch (Exception ex) { UiHelpers.ShowError(this, ex); }
    }
}
