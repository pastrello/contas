namespace Caixa.WinForms.Forms;

partial class HistoriesForm
{
    private DataGridView grid = null!;
    private TextBox txtCode = null!;
    private TextBox txtDescription = null!;
    private ComboBox cmbDirection = null!;
    private CheckBox chkActive = null!;

    private void InitializeComponent()
    {
        grid = new DataGridView(); txtCode = new TextBox(); txtDescription = new TextBox(); cmbDirection = new ComboBox(); chkActive = new CheckBox();
        var btnNew = new Button(); var btnSave = new Button(); var btnDelete = new Button(); var btnClose = new Button();
        SuspendLayout();
        var l1 = new Label { Text = "Código", Location = new Point(20,18), AutoSize = true };
        var l2 = new Label { Text = "Descrição", Location = new Point(100,18), AutoSize = true };
        var l3 = new Label { Text = "Tipo", Location = new Point(420,18), AutoSize = true };
        txtCode.Location = new Point(20,40); txtCode.Size = new Size(60,23); txtCode.MaxLength = 2; txtCode.TabIndex = 0;
        txtDescription.Location = new Point(100,40); txtDescription.Size = new Size(300,23); txtDescription.MaxLength = 100; txtDescription.TabIndex = 1;
        cmbDirection.Location = new Point(420,40); cmbDirection.Size = new Size(120,23); cmbDirection.DropDownStyle = ComboBoxStyle.DropDownList; cmbDirection.TabIndex = 2;
        chkActive.Location = new Point(560,42); chkActive.Text = "Ativo"; chkActive.AutoSize = true; chkActive.Checked = true; chkActive.TabIndex = 3; chkActive.TabStop = false;
        btnNew.Text="Novo"; btnNew.Location=new Point(20,82); btnNew.TabStop=false; btnNew.Click += (_,_) => NewItem();
        btnSave.Text="Salvar"; btnSave.Location=new Point(105,82); btnSave.TabStop=false; btnSave.Click += (_,_) => SaveItem();
        btnDelete.Text="Excluir"; btnDelete.Location=new Point(190,82); btnDelete.TabStop=false; btnDelete.Click += (_,_) => DeleteItem();
        btnClose.Text="Fechar"; btnClose.Location=new Point(595,82); btnClose.TabStop=false; btnClose.Click += (_,_) => Close();
        grid.Location = new Point(20,125); grid.Size = new Size(650,330); grid.Anchor = AnchorStyles.Top|AnchorStyles.Bottom|AnchorStyles.Left|AnchorStyles.Right; grid.TabStop=false;
        grid.SelectionChanged += (_,_) => SelectCurrent();
        Controls.AddRange([l1,l2,l3,txtCode,txtDescription,cmbDirection,chkActive,btnNew,btnSave,btnDelete,btnClose,grid]);
        ClientSize = new Size(690,475); MinimumSize = new Size(650,450); StartPosition=FormStartPosition.CenterParent; Text="Históricos";
        ResumeLayout(false); PerformLayout();
    }
}
