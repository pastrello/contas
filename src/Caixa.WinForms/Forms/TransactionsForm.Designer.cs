namespace Caixa.WinForms.Forms;

partial class TransactionsForm
{
    private ComboBox cmbAccount = null!;
    private ComboBox cmbHistory = null!;
    private DateTimePicker date = null!;
    private TextBox txtDocument = null!;
    private NumericUpDown numAmount = null!;
    private TextBox txtDescription = null!;
    private CheckBox chkCleared = null!;
    private CheckBox chkShowArchived = null!;
    private DataGridView grid = null!;
    private Label lblReal = null!;
    private Label lblCleared = null!;
    private Label lblPending = null!;
    private Label lblLegacy = null!;
    private Button btnSave = null!;
    private Button btnDelete = null!;
    private Button btnToggle = null!;

    private void InitializeComponent()
    {
        cmbAccount = new ComboBox(); cmbHistory = new ComboBox(); date = new DateTimePicker(); txtDocument = new TextBox();
        numAmount = new NumericUpDown(); txtDescription = new TextBox(); chkCleared = new CheckBox(); chkShowArchived = new CheckBox();
        grid = new DataGridView(); lblReal = new Label(); lblCleared = new Label(); lblPending = new Label(); lblLegacy = new Label();
        btnSave = new Button(); btnDelete = new Button(); btnToggle = new Button(); var btnNew = new Button(); var btnClose = new Button();
        ((System.ComponentModel.ISupportInitialize)numAmount).BeginInit(); SuspendLayout();

        Controls.Add(new Label { Text="Conta", Location=new Point(18,15), AutoSize=true });
        cmbAccount.Location=new Point(18,37); cmbAccount.Size=new Size(430,23); cmbAccount.DropDownStyle=ComboBoxStyle.DropDownList; cmbAccount.TabIndex=0;
        chkShowArchived.Text="Mostrar baixados"; chkShowArchived.Location=new Point(470,39); chkShowArchived.AutoSize=true; chkShowArchived.TabStop=false;
        Controls.Add(new Label { Text="Saldo bancário", Location=new Point(700,12), AutoSize=true });
        lblCleared.Location=new Point(700,34); lblCleared.Size=new Size(150,25); lblCleared.Font=new Font("Segoe UI",11F,FontStyle.Bold); lblCleared.TextAlign=ContentAlignment.MiddleRight;
        Controls.Add(new Label { Text="Saldo real", Location=new Point(870,12), AutoSize=true });
        lblReal.Location=new Point(870,34); lblReal.Size=new Size(150,25); lblReal.Font=new Font("Segoe UI",11F,FontStyle.Bold); lblReal.TextAlign=ContentAlignment.MiddleRight;
        Controls.Add(new Label { Text="Pendente", Location=new Point(1040,12), AutoSize=true });
        lblPending.Location=new Point(1040,34); lblPending.Size=new Size(140,25); lblPending.Font=new Font("Segoe UI",11F,FontStyle.Bold); lblPending.TextAlign=ContentAlignment.MiddleRight;

        grid.Location=new Point(18,75); grid.Size=new Size(1162,330); grid.Anchor=AnchorStyles.Top|AnchorStyles.Bottom|AnchorStyles.Left|AnchorStyles.Right; grid.TabStop=false;
        grid.SelectionChanged += (_,_) => SelectCurrent();

        var y=425;
        Controls.Add(new Label { Text="Data", Location=new Point(18,y), AutoSize=true });
        Controls.Add(new Label { Text="Documento", Location=new Point(130,y), AutoSize=true });
        Controls.Add(new Label { Text="Histórico", Location=new Point(250,y), AutoSize=true });
        Controls.Add(new Label { Text="Valor", Location=new Point(565,y), AutoSize=true });
        date.Location=new Point(18,y+22); date.Size=new Size(100,23); date.Format=DateTimePickerFormat.Short; date.TabIndex=1;
        txtDocument.Location=new Point(130,y+22); txtDocument.Size=new Size(105,23); txtDocument.MaxLength=30; txtDocument.TabIndex=2;
        cmbHistory.Location=new Point(250,y+22); cmbHistory.Size=new Size(295,23); cmbHistory.DropDownStyle=ComboBoxStyle.DropDownList; cmbHistory.TabIndex=3;
        numAmount.Location=new Point(565,y+22); numAmount.Size=new Size(150,23); numAmount.DecimalPlaces=2; numAmount.ThousandsSeparator=true; numAmount.Maximum=999999999999m; numAmount.TabIndex=4;
        chkCleared.Text="Contabilizado"; chkCleared.Location=new Point(735,y+23); chkCleared.AutoSize=true; chkCleared.TabIndex=5; chkCleared.TabStop=false;
        lblLegacy.Text="Registro protegido por compatibilidade: somente leitura"; lblLegacy.Location=new Point(855,y+24); lblLegacy.Size=new Size(325,20); lblLegacy.Visible=false;
        Controls.Add(new Label { Text="Complemento / finalidade", Location=new Point(18,y+58), AutoSize=true });
        txtDescription.Location=new Point(18,y+80); txtDescription.Size=new Size(1162,23); txtDescription.Anchor=AnchorStyles.Left|AnchorStyles.Right|AnchorStyles.Bottom; txtDescription.MaxLength=250; txtDescription.TabIndex=6;

        btnNew.Text="Novo"; btnNew.Location=new Point(18,y+120); btnNew.TabStop=false; btnNew.Click += (_,_) => NewItem();
        btnSave.Text="Salvar"; btnSave.Location=new Point(103,y+120); btnSave.TabStop=false; btnSave.Click += (_,_) => SaveItem();
        btnDelete.Text="Excluir"; btnDelete.Location=new Point(188,y+120); btnDelete.TabStop=false; btnDelete.Click += (_,_) => DeleteItem();
        btnToggle.Text="Contabilizar / desfazer"; btnToggle.Location=new Point(273,y+120); btnToggle.Size=new Size(155,23); btnToggle.TabStop=false; btnToggle.Click += (_,_) => ToggleCleared();
        btnClose.Text="Fechar"; btnClose.Location=new Point(1105,y+120); btnClose.Anchor=AnchorStyles.Right|AnchorStyles.Bottom; btnClose.TabStop=false; btnClose.Click += (_,_) => Close();

        Controls.AddRange([cmbAccount,chkShowArchived,lblCleared,lblReal,lblPending,grid,date,txtDocument,cmbHistory,numAmount,chkCleared,lblLegacy,txtDescription,btnNew,btnSave,btnDelete,btnToggle,btnClose]);
        ClientSize=new Size(1200,590); MinimumSize=new Size(1000,560); StartPosition=FormStartPosition.CenterParent; Text="Lançamentos";
        ((System.ComponentModel.ISupportInitialize)numAmount).EndInit(); ResumeLayout(false); PerformLayout();
    }
}
