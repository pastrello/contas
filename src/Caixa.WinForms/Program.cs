using Caixa.Core;
using Caixa.WinForms.Forms;

namespace Caixa.WinForms;

internal static class Program
{
    private const string SingleInstanceMutexName = @"Local\CaixaModernizado.SingleInstance";

    [STAThread]
    private static void Main()
    {
        using var singleInstanceMutex = new Mutex(
            initiallyOwned: true,
            name: SingleInstanceMutexName,
            createdNew: out var isFirstInstance);

        if (!isFirstInstance)
        {
            MessageBox.Show(
                "O Caixa Modernizado já está em execução.\r\n\r\nUse a janela que já está aberta.",
                "Caixa Modernizado",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
            return;
        }

        Application.SetHighDpiMode(HighDpiMode.PerMonitorV2);
        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault(false);

        try
        {
            var app = new CaixaApplication();
            Application.Run(new MainForm(app));
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Falha ao iniciar o Caixa Modernizado:\r\n\r\n{ex}", "Erro fatal",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        finally
        {
            singleInstanceMutex.ReleaseMutex();
        }
    }
}
