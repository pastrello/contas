using System.Text;
using Caixa.Core;
using Caixa.Core.Models;
using Caixa.WinForms.Utilities;

namespace Caixa.WinForms.Forms;

/// <summary>Diagnóstico e manutenção do arquivo SQLite.</summary>
public sealed class DatabaseMaintenanceForm : Form
{
    private readonly CaixaApplication _app;
    private readonly TextBox _info = new();
    private readonly Label _status = new();

    public DatabaseMaintenanceForm(CaixaApplication app)
    {
        _app = app;
        Text = "Manutenção do banco SQLite";
        StartPosition = FormStartPosition.CenterParent;
        ClientSize = new Size(760, 520);
        MinimumSize = new Size(680, 470);
        BuildUi();
        Load += (_, _) => Run("Verificação rápida", () => _app.Maintenance.QuickCheck());
    }

    private void BuildUi()
    {
        var intro = new Label
        {
            Text = "Ferramentas de diagnóstico do SQLite. VACUUM cria um backup preventivo antes de regravar o banco.",
            Location = new Point(18, 18), Size = new Size(715, 36)
        };

        _info.Location = new Point(18, 60);
        _info.Size = new Size(724, 365);
        _info.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        _info.Multiline = true;
        _info.ReadOnly = true;
        _info.ScrollBars = ScrollBars.Vertical;
        _info.Font = new Font("Consolas", 9.5F);
        _info.WordWrap = false;

        var quick = new Button { Text = "Quick check", Location = new Point(18, 442), Size = new Size(100, 28), Anchor = AnchorStyles.Bottom | AnchorStyles.Left };
        quick.Click += (_, _) => Run("Verificação rápida", () => _app.Maintenance.QuickCheck());
        var full = new Button { Text = "Integridade completa", Location = new Point(128, 442), Size = new Size(140, 28), Anchor = AnchorStyles.Bottom | AnchorStyles.Left };
        full.Click += (_, _) => Run("Verificação completa", () => _app.Maintenance.IntegrityCheck());
        var optimize = new Button { Text = "Otimizar", Location = new Point(278, 442), Size = new Size(100, 28), Anchor = AnchorStyles.Bottom | AnchorStyles.Left };
        optimize.Click += (_, _) => Run("Otimização", () => _app.Maintenance.Optimize());
        var vacuum = new Button { Text = "Compactar (VACUUM)", Location = new Point(388, 442), Size = new Size(145, 28), Anchor = AnchorStyles.Bottom | AnchorStyles.Left };
        vacuum.Click += (_, _) => Vacuum();
        var close = new Button { Text = "Fechar", Location = new Point(657, 442), Size = new Size(85, 28), Anchor = AnchorStyles.Bottom | AnchorStyles.Right };
        close.Click += (_, _) => Close();

        _status.Location = new Point(18, 480);
        _status.Size = new Size(724, 22);
        _status.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;

        Controls.AddRange([intro, _info, quick, full, optimize, vacuum, close, _status]);
    }

    private void Run(string operation, Func<DatabaseMaintenanceReport> action)
    {
        try
        {
            Cursor = Cursors.WaitCursor;
            _status.Text = operation + " em andamento...";
            Application.DoEvents();
            var report = action();
            _info.Text = Format(report);
            _status.Text = $"{operation} concluída em {DateTime.Now:dd/MM/yyyy HH:mm:ss}.";
        }
        catch (Exception ex)
        {
            _status.Text = operation + " não concluída.";
            UiHelpers.ShowError(this, ex);
        }
        finally
        {
            Cursor = Cursors.Default;
        }
    }

    private void Vacuum()
    {
        var answer = MessageBox.Show(this,
            "O VACUUM regrava o banco SQLite para recuperar espaço livre.\r\n\r\n" +
            "Antes da operação será criado automaticamente um backup preventivo.\r\n\r\nDeseja continuar?",
            "Compactar banco", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
        if (answer != DialogResult.Yes) return;

        try
        {
            Cursor = Cursors.WaitCursor;
            _status.Text = "Criando backup preventivo e executando VACUUM...";
            Application.DoEvents();
            var result = _app.Maintenance.VacuumWithSafetyBackup();
            _info.Text = Format(result.Report) +
                         $"\r\n\r\nVACUUM\r\n------\r\n" +
                         $"Tamanho antes : {FormatBytes(result.SizeBeforeBytes)}\r\n" +
                         $"Tamanho depois: {FormatBytes(result.SizeAfterBytes)}\r\n" +
                         $"Backup prévio : {result.SafetyBackupPath}";
            _status.Text = $"VACUUM concluído em {DateTime.Now:dd/MM/yyyy HH:mm:ss}.";
        }
        catch (Exception ex)
        {
            _status.Text = "VACUUM não concluído.";
            UiHelpers.ShowError(this, ex);
        }
        finally
        {
            Cursor = Cursors.Default;
        }
    }

    private static string Format(DatabaseMaintenanceReport r)
    {
        var sb = new StringBuilder();
        sb.AppendLine("CAIXA MODERNIZADO - SQLITE");
        sb.AppendLine("=========================");
        sb.AppendLine();
        sb.AppendLine($"Arquivo.............: {r.DatabasePath}");
        sb.AppendLine($"Tamanho.............: {FormatBytes(r.DatabaseSizeBytes)}");
        sb.AppendLine($"Esquema.............: user_version={r.UserVersion}");
        sb.AppendLine($"Journal mode........: {r.JournalMode}");
        sb.AppendLine($"Page size...........: {FormatBytes(r.PageSizeBytes)}");
        sb.AppendLine($"Páginas.............: {r.PageCount:N0}");
        sb.AppendLine($"Páginas livres......: {r.FreePageCount:N0}");
        sb.AppendLine();
        sb.AppendLine("CONTEÚDO");
        sb.AppendLine("--------");
        sb.AppendLine($"Contas..............: {r.AccountCount:N0}");
        sb.AppendLine($"Históricos..........: {r.HistoryCount:N0}");
        sb.AppendLine($"Lançamentos.........: {r.TransactionCount:N0}");
        sb.AppendLine();
        sb.AppendLine("VERIFICAÇÃO");
        sb.AppendLine("-----------");
        sb.AppendLine(r.CheckResult);
        return sb.ToString();
    }

    private static string FormatBytes(long bytes)
    {
        string[] units = ["B", "KB", "MB", "GB", "TB"];
        double value = bytes;
        var unit = 0;
        while (value >= 1024 && unit < units.Length - 1) { value /= 1024; unit++; }
        return $"{value:N2} {units[unit]}";
    }
}
