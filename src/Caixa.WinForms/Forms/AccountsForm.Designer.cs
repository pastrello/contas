namespace Caixa.WinForms.Forms;

partial class AccountsForm
{
    private DataGridView grid = null!;
    private TextBox txtCode = null!;
    private TextBox txtNumber = null!;
    private TextBox txtName = null!;
    private NumericUpDown numOpening = null!;
    private CheckBox chkActive = null!;
    private Button btnNew = null!;
    private Button btnSave = null!;
    private Button btnDelete = null!;
    private Button btnClose = null!;

    private void InitializeComponent()
    {
        grid = new DataGridView(); txtCode = new TextBox(); txtNumber = new TextBox(); txtName = new TextBox();
        numOpening = new NumericUpDown(); chkActive = new CheckBox(); btnNew = new Button(); btnSave = new Button(); btnDelete = new Button(); btnClose = new Button();
        ((System.ComponentModel.ISupportInitialize)numOpening).BeginInit();
        SuspendLayout();

        var lblCode = new Label { Text = "Código", Location = new Point(20, 18), AutoSize = true };
        var lblNumber = new Label { Text = "Número da conta", Location = new Point(100, 18), AutoSize = true };
        var lblName = new Label { Text = "Banco / descrição", Location = new Point(255, 18), AutoSize = true };
        var lblOpening = new Label { Text = "Saldo inicial", Location = new Point(570, 18), AutoSize = true };
        txtCode.Location = new Point(20, 40); txtCode.Size = new Size(60, 23); txtCode.MaxLength = 2; txtCode.TabIndex = 0;
        txtNumber.Location = new Point(100, 40); txtNumber.Size = new Size(135, 23); txtNumber.MaxLength = 30; txtNumber.TabIndex = 1;
        txtName.Location = new Point(255, 40); txtName.Size = new Size(295, 23); txtName.MaxLength = 100; txtName.TabIndex = 2;
        numOpening.Location = new Point(570, 40); numOpening.Size = new Size(150, 23); numOpening.DecimalPlaces = 2; numOpening.ThousandsSeparator = true; numOpening.Minimum = -999999999999m; numOpening.Maximum = 999999999999m; numOpening.TabIndex = 3;
        chkActive.Location = new Point(740, 40); chkActive.Text = "Ativa"; chkActive.Checked = true; chkActive.AutoSize = true; chkActive.TabIndex = 4; chkActive.TabStop = false;

        btnNew.Text = "Novo"; btnNew.Location = new Point(20, 82); btnNew.TabStop = false; btnNew.Click += (_, _) => NewItem();
        btnSave.Text = "Salvar"; btnSave.Location = new Point(105, 82); btnSave.TabStop = false; btnSave.Click += (_, _) => SaveItem();
        btnDelete.Text = "Excluir"; btnDelete.Location = new Point(190, 82); btnDelete.TabStop = false; btnDelete.Click += (_, _) => DeleteItem();
        btnClose.Text = "Fechar"; btnClose.Location = new Point(785, 82); btnClose.TabStop = false; btnClose.Click += (_, _) => Close();

        grid.Location = new Point(20, 125); grid.Size = new Size(840, 385); grid.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right; grid.TabStop = false;
        grid.SelectionChanged += (_, _) => SelectCurrent();

        Controls.AddRange([lblCode,lblNumber,lblName,lblOpening,txtCode,txtNumber,txtName,numOpening,chkActive,btnNew,btnSave,btnDelete,btnClose,grid]);
        ClientSize = new Size(880, 530); MinimumSize = new Size(800, 500); StartPosition = FormStartPosition.CenterParent; Text = "Contas";
        ((System.ComponentModel.ISupportInitialize)numOpening).EndInit();
        ResumeLayout(false); PerformLayout();
    }
}
