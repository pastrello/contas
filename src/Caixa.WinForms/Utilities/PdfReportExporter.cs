using MigraDoc.DocumentObjectModel;
using MigraDoc.DocumentObjectModel.Tables;
using MigraDoc.Rendering;

namespace Caixa.WinForms.Utilities;

/// <summary>
/// Exporta uma DataGridView para um relatório PDF paginado.
/// MigraDoc cuida de quebra de páginas, repetição do cabeçalho da tabela e paginação.
/// </summary>
internal static class PdfReportExporter
{
    public sealed record Options(
        string CompanyName,
        string Title,
        string? Account,
        DateOnly? StartDate,
        DateOnly? EndDate,
        string? FilterDescription,
        string? Summary);

    public static void OfferOpen(IWin32Window owner, string path)
    {
        var answer = MessageBox.Show(owner,
            $"PDF gerado com sucesso.\r\n\r\n{path}\r\n\r\nDeseja abrir o relatório agora?",
            "Relatório PDF", MessageBoxButtons.YesNo, MessageBoxIcon.Information);

        if (answer != DialogResult.Yes) return;

        try
        {
            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo(path)
            {
                UseShellExecute = true
            });
        }
        catch (Exception ex)
        {
            MessageBox.Show(owner,
                $"O PDF foi salvo, mas não foi possível abrir o visualizador padrão.\r\n\r\n{ex.Message}",
                "Relatório PDF", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
    }

    public static void Export(DataGridView grid, string path, Options options)
    {
        ArgumentNullException.ThrowIfNull(grid);
        ArgumentNullException.ThrowIfNull(options);
        if (string.IsNullOrWhiteSpace(path)) throw new ArgumentException("Informe o arquivo PDF de destino.", nameof(path));

        var columns = grid.Columns.Cast<DataGridViewColumn>()
            .Where(c => c.Visible)
            .OrderBy(c => c.DisplayIndex)
            .ToList();

        if (columns.Count == 0)
            throw new InvalidOperationException("O relatório não possui colunas visíveis para exportar.");

        var document = new Document();
        document.Info.Title = options.Title;
        document.Info.Author = options.CompanyName;
        document.Info.Subject = "Relatório gerado pelo Caixa Modernizado";

        ConfigureStyles(document);

        var section = document.AddSection();
        section.PageSetup.PageFormat = PageFormat.A4;
        section.PageSetup.Orientation = MigraDoc.DocumentObjectModel.Orientation.Landscape;
        section.PageSetup.TopMargin = Unit.FromCentimeter(1.15);
        section.PageSetup.BottomMargin = Unit.FromCentimeter(1.35);
        section.PageSetup.LeftMargin = Unit.FromCentimeter(1.0);
        section.PageSetup.RightMargin = Unit.FromCentimeter(1.0);
        section.PageSetup.HeaderDistance = Unit.FromCentimeter(0.45);
        section.PageSetup.FooterDistance = Unit.FromCentimeter(0.55);

        AddFooter(section);
        AddReportHeader(section, options);
        AddGridTable(section, grid, columns);
        AddSummary(section, options.Summary);

        var renderer = new PdfDocumentRenderer { Document = document };
        renderer.PdfDocument.ViewerPreferences.FitWindow = true;
        renderer.RenderDocument();
        renderer.Save(path);
    }

    private static void ConfigureStyles(Document document)
    {
        var normal = document.Styles[StyleNames.Normal];
        normal.Font.Name = "Arial";
        normal.Font.Size = Unit.FromPoint(8);

        var footer = document.Styles[StyleNames.Footer];
        footer.Font.Name = "Arial";
        footer.Font.Size = Unit.FromPoint(7);
        footer.Font.Color = Colors.DarkGray;
    }

    private static void AddFooter(Section section)
    {
        var footer = section.Footers.Primary.AddParagraph();
        footer.Style = StyleNames.Footer;
        footer.Format.Alignment = ParagraphAlignment.Center;
        footer.AddText("Caixa Modernizado  |  Página ");
        footer.AddPageField();
        footer.AddText(" de ");
        footer.AddNumPagesField();
    }

    private static void AddReportHeader(Section section, Options options)
    {
        var company = section.AddParagraph();
        company.Format.Alignment = ParagraphAlignment.Center;
        company.Format.Font.Name = "Arial";
        company.Format.Font.Size = Unit.FromPoint(11);
        company.Format.Font.Bold = true;
        company.AddText(options.CompanyName.Trim());

        var title = section.AddParagraph();
        title.Format.Alignment = ParagraphAlignment.Center;
        title.Format.Font.Name = "Arial";
        title.Format.Font.Size = Unit.FromPoint(15);
        title.Format.Font.Bold = true;
        title.Format.SpaceAfter = Unit.FromPoint(5);
        title.AddText(options.Title);

        var metadata = new List<string>();
        if (!string.IsNullOrWhiteSpace(options.Account))
            metadata.Add($"Conta: {options.Account}");
        if (options.StartDate.HasValue && options.EndDate.HasValue)
            metadata.Add($"Período: {options.StartDate.Value:dd/MM/yyyy} a {options.EndDate.Value:dd/MM/yyyy}");
        if (!string.IsNullOrWhiteSpace(options.FilterDescription))
            metadata.Add(options.FilterDescription.Trim());

        foreach (var line in metadata)
        {
            var paragraph = section.AddParagraph(line);
            paragraph.Format.Font.Name = "Arial";
            paragraph.Format.Font.Size = Unit.FromPoint(8.5);
            paragraph.Format.SpaceAfter = Unit.FromPoint(1.5);
        }

        var generated = section.AddParagraph($"Emitido em: {DateTime.Now:dd/MM/yyyy HH:mm:ss}");
        generated.Format.Font.Name = "Arial";
        generated.Format.Font.Size = Unit.FromPoint(7.5);
        generated.Format.Font.Color = Colors.DarkGray;
        generated.Format.SpaceAfter = Unit.FromPoint(7);
    }

    private static void AddGridTable(Section section, DataGridView grid, IReadOnlyList<DataGridViewColumn> columns)
    {
        var table = section.AddTable();
        table.Borders.Width = Unit.FromPoint(0.35);
        table.Borders.Color = Colors.LightGray;
        table.Format.Font.Name = "Arial";
        table.Format.Font.Size = Unit.FromPoint(7.3);
        table.Rows.VerticalAlignment = VerticalAlignment.Center;

        const double availableWidthCm = 27.2;
        var weights = columns.Select(c => Math.Max(45, c.Width)).ToArray();
        var totalWeight = weights.Sum();

        for (var i = 0; i < columns.Count; i++)
        {
            var width = availableWidthCm * weights[i] / totalWeight;
            width = Math.Max(columns[i] is DataGridViewCheckBoxColumn ? 1.2 : 1.45, width);
            var column = table.AddColumn(Unit.FromCentimeter(width));
            column.LeftPadding = Unit.FromMillimeter(1.1);
            column.RightPadding = Unit.FromMillimeter(1.1);
            column.Format.Alignment = GetAlignment(columns[i]);
        }

        var header = table.AddRow();
        header.HeadingFormat = true;
        header.Format.Font.Bold = true;
        header.Shading.Color = Colors.LightGray;
        header.TopPadding = Unit.FromMillimeter(1.2);
        header.BottomPadding = Unit.FromMillimeter(1.2);

        for (var i = 0; i < columns.Count; i++)
        {
            var paragraph = header.Cells[i].AddParagraph(columns[i].HeaderText);
            paragraph.Format.Alignment = ParagraphAlignment.Center;
            paragraph.Format.Font.Size = Unit.FromPoint(7.2);
        }

        var bodyIndex = 0;
        foreach (DataGridViewRow gridRow in grid.Rows)
        {
            if (gridRow.IsNewRow) continue;
            var row = table.AddRow();
            row.TopPadding = Unit.FromMillimeter(0.75);
            row.BottomPadding = Unit.FromMillimeter(0.75);
            if (bodyIndex++ % 2 == 1) row.Shading.Color = Colors.WhiteSmoke;

            for (var i = 0; i < columns.Count; i++)
            {
                var gridColumn = columns[i];
                var cell = gridRow.Cells[gridColumn.Index];
                var paragraph = row.Cells[i].AddParagraph(CellText(cell));
                paragraph.Format.Alignment = GetAlignment(gridColumn);
            }
        }
    }

    private static void AddSummary(Section section, string? summaryText)
    {
        if (string.IsNullOrWhiteSpace(summaryText)) return;
        var paragraph = section.AddParagraph();
        paragraph.Format.SpaceBefore = Unit.FromPoint(7);
        paragraph.Format.Font.Name = "Arial";
        paragraph.Format.Font.Size = Unit.FromPoint(8.5);
        paragraph.Format.Font.Bold = true;
        paragraph.AddText(summaryText.Trim());
    }

    private static ParagraphAlignment GetAlignment(DataGridViewColumn column)
    {
        if (column is DataGridViewCheckBoxColumn) return ParagraphAlignment.Center;
        return column.DefaultCellStyle.Alignment switch
        {
            DataGridViewContentAlignment.BottomRight or DataGridViewContentAlignment.MiddleRight or DataGridViewContentAlignment.TopRight => ParagraphAlignment.Right,
            DataGridViewContentAlignment.BottomCenter or DataGridViewContentAlignment.MiddleCenter or DataGridViewContentAlignment.TopCenter => ParagraphAlignment.Center,
            _ => ParagraphAlignment.Left
        };
    }

    private static string CellText(DataGridViewCell cell)
    {
        if (cell is DataGridViewCheckBoxCell)
        {
            var value = cell.Value;
            if (value is bool b) return b ? "Sim" : "Não";
            if (value is null || value == DBNull.Value) return string.Empty;
            return Convert.ToBoolean(value) ? "Sim" : "Não";
        }
        var text = Convert.ToString(cell.FormattedValue) ?? string.Empty;
        return text.Replace('\r', ' ').Replace('\n', ' ').Replace('\t', ' ').Trim();
    }
}
