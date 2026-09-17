namespace Caixa.Core.Data;

/// <summary>Centraliza os caminhos de dados para evitar caminhos fixos espalhados pelo projeto.</summary>
public static class AppPaths
{
    public static string DataDirectory
    {
        get
        {
            var path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "CaixaModernizado");
            Directory.CreateDirectory(path);
            return path;
        }
    }

    public static string DatabasePath => Path.Combine(DataDirectory, "caixa.db");

    public static string PreRestoreBackupDirectory
    {
        get
        {
            var path = Path.Combine(DataDirectory, "Backups", "PreRestore");
            Directory.CreateDirectory(path);
            return path;
        }
    }

    public static string PreMaintenanceBackupDirectory
    {
        get
        {
            var path = Path.Combine(DataDirectory, "Backups", "PreMaintenance");
            Directory.CreateDirectory(path);
            return path;
        }
    }
}
