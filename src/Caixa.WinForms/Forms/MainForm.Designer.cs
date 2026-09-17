namespace Caixa.WinForms.Forms;

partial class MainForm
{
    private System.ComponentModel.IContainer? components = null;
    private MenuStrip menuMain = null!;
    private ToolStripMenuItem mnuRegistrations = null!;
    private ToolStripMenuItem mnuAccounts = null!;
    private ToolStripMenuItem mnuHistories = null!;
    private ToolStripMenuItem mnuSettings = null!;
    private ToolStripMenuItem mnuMovement = null!;
    private ToolStripMenuItem mnuTransactions = null!;
    private ToolStripMenuItem mnuTransactionSearch = null!;
    private ToolStripMenuItem mnuStatement = null!;
    private ToolStripMenuItem mnuConsistency = null!;
    private ToolStripMenuItem mnuCheckForecast = null!;
    private ToolStripMenuItem mnuArchive = null!;
    private ToolStripMenuItem mnuArchived = null!;
    private ToolStripMenuItem mnuHistoryReport = null!;
    private ToolStripMenuItem mnuTools = null!;
    private ToolStripMenuItem mnuBackup = null!;
    private ToolStripMenuItem mnuRestore = null!;
    private ToolStripMenuItem mnuMaintenance = null!;
    private ToolStripMenuItem mnuHelp = null!;
    private ToolStripMenuItem mnuAbout = null!;
    private ToolStripMenuItem mnuExit = null!;
    private Label lblTitle = null!;
    private Label lblCompany = null!;
    private Label lblDatabase = null!;
    private Label lblAccountCount = null!;
    private DataGridView gridAccounts = null!;

    protected override void Dispose(bool disposing)
    {
        if (disposing) components?.Dispose();
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        components = new System.ComponentModel.Container();
        menuMain = new MenuStrip();
        mnuRegistrations = new ToolStripMenuItem("Cadastros");
        mnuAccounts = new ToolStripMenuItem("Contas");
        mnuHistories = new ToolStripMenuItem("Históricos");
        mnuSettings = new ToolStripMenuItem("Parâmetros");
        mnuMovement = new ToolStripMenuItem("Movimentação");
        mnuTransactions = new ToolStripMenuItem("Lançamentos");
        mnuTransactionSearch = new ToolStripMenuItem("Consulta avançada de lançamentos");
        mnuStatement = new ToolStripMenuItem("Extrato");
        mnuConsistency = new ToolStripMenuItem("Consistência / pendências");
        mnuCheckForecast = new ToolStripMenuItem("Previsão de cheques");
        mnuArchive = new ToolStripMenuItem("Baixa / arquivar contabilizados");
        mnuArchived = new ToolStripMenuItem("Consulta de baixados");
        mnuHistoryReport = new ToolStripMenuItem("Relatório por histórico");
        mnuTools = new ToolStripMenuItem("Ferramentas");
        mnuBackup = new ToolStripMenuItem("Criar backup compactado do SQLite");
        mnuRestore = new ToolStripMenuItem("Restaurar backup do SQLite");
        mnuMaintenance = new ToolStripMenuItem("Manutenção do banco SQLite");
        mnuHelp = new ToolStripMenuItem("Ajuda");
        mnuAbout = new ToolStripMenuItem("Sobre");
        mnuExit = new ToolStripMenuItem("Sair");
        lblTitle = new Label();
        lblCompany = new Label();
        lblDatabase = new Label();
        lblAccountCount = new Label();
        gridAccounts = new DataGridView();

        menuMain.Items.AddRange([mnuRegistrations, mnuMovement, mnuTools, mnuHelp, mnuExit]);
        mnuRegistrations.DropDownItems.AddRange([mnuAccounts, mnuHistories, new ToolStripSeparator(), mnuSettings]);
        mnuMovement.DropDownItems.AddRange([mnuTransactions, mnuTransactionSearch, new ToolStripSeparator(),
            mnuStatement, mnuConsistency, mnuCheckForecast, new ToolStripSeparator(),
            mnuArchive, mnuArchived, mnuHistoryReport]);
        mnuTools.DropDownItems.AddRange([mnuBackup, mnuRestore, new ToolStripSeparator(), mnuMaintenance]);
        mnuHelp.DropDownItems.Add(mnuAbout);

        mnuAccounts.Click += mnuAccounts_Click;
        mnuHistories.Click += mnuHistories_Click;
        mnuSettings.Click += mnuSettings_Click;
        mnuTransactions.Click += mnuTransactions_Click;
        mnuTransactionSearch.Click += mnuTransactionSearch_Click;
        mnuStatement.Click += mnuStatement_Click;
        mnuConsistency.Click += mnuConsistency_Click;
        mnuCheckForecast.Click += mnuCheckForecast_Click;
        mnuArchive.Click += mnuArchive_Click;
        mnuArchived.Click += mnuArchived_Click;
        mnuHistoryReport.Click += mnuHistoryReport_Click;
        mnuBackup.Click += mnuBackup_Click;
        mnuRestore.Click += mnuRestore_Click;
        mnuMaintenance.Click += mnuMaintenance_Click;
        mnuAbout.Click += mnuAbout_Click;
        mnuExit.Click += mnuExit_Click;

        lblTitle.AutoSize = true;
        lblTitle.Font = new Font("Segoe UI", 20F, FontStyle.Bold);
        lblTitle.Location = new Point(24, 52);
        lblTitle.Text = "Controle Bancário";

        lblCompany.AutoSize = true;
        lblCompany.Font = new Font("Segoe UI", 11F);
        lblCompany.Location = new Point(27, 96);
        lblCompany.Text = "Empresa";

        lblAccountCount.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        lblAccountCount.AutoSize = true;
        lblAccountCount.Location = new Point(860, 105);
        lblAccountCount.Text = "0 conta(s)";

        gridAccounts.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        gridAccounts.Location = new Point(28, 138);
        gridAccounts.Size = new Size(944, 432);

        lblDatabase.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        lblDatabase.AutoEllipsis = true;
        lblDatabase.Location = new Point(28, 584);
        lblDatabase.Size = new Size(944, 23);
        lblDatabase.Text = "Banco:";

        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(1000, 625);
        Controls.Add(gridAccounts);
        Controls.Add(lblDatabase);
        Controls.Add(lblAccountCount);
        Controls.Add(lblCompany);
        Controls.Add(lblTitle);
        Controls.Add(menuMain);
        MainMenuStrip = menuMain;
        MinimumSize = new Size(900, 550);
        StartPosition = FormStartPosition.CenterScreen;
        Text = "Caixa Modernizado";
    }
}
