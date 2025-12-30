namespace SharpReports.Core;

/// <summary>
/// Represents a section in a report with a title and collection of elements
/// </summary>
public class ReportSection
{
    /// <summary>
    /// Gets the section title
    /// </summary>
    public string Title { get; }

    /// <summary>
    /// Gets the number of columns for layout (default: 1)
    /// </summary>
    public int Columns { get; private set; } = 1;

    /// <summary>
    /// Gets the custom column widths as unit values (e.g., [1, 2] means first column is 1 unit, second is 2 units, totaling 3 units).
    /// If null, columns are equal width.
    /// </summary>
    public List<int>? ColumnWidths { get; private set; }

    /// <summary>
    /// Gets the elements in this section
    /// </summary>
    public List<IReportElement> Elements { get; } = new();

    public ReportSection(string title)
    {
        Title = title ?? throw new ArgumentNullException(nameof(title));
    }

    /// <summary>
    /// Sets the number of columns for this section's layout
    /// </summary>
    public ReportSection SetColumns(int columns)
    {
        if (columns < 1)
            throw new ArgumentException("Columns must be at least 1", nameof(columns));

        Columns = columns;
        ColumnWidths = null; // Reset custom widths
        return this;
    }

    /// <summary>
    /// Sets custom column widths using unit values. Units are proportional - e.g., [1, 2] creates a 33%/67% split (1 unit / 3 total units = 33%).
    /// </summary>
    /// <param name="widths">Array of column widths as unit values (e.g., [1, 2] for 1:2 ratio, [1, 1, 1] for equal thirds)</param>
    public ReportSection SetColumnWidths(params int[] widths)
    {
        if (widths == null || widths.Length == 0)
            throw new ArgumentException("Must provide at least one column width", nameof(widths));

        if (widths.Any(w => w < 1))
            throw new ArgumentException("Column widths must be at least 1", nameof(widths));

        Columns = widths.Length;
        ColumnWidths = new List<int>(widths);
        return this;
    }

    /// <summary>
    /// Adds an element to this section
    /// </summary>
    public ReportSection AddElement(IReportElement element)
    {
        Elements.Add(element ?? throw new ArgumentNullException(nameof(element)));
        return this;
    }
}
