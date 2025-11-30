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
