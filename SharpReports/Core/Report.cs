namespace SharpReports.Core;

/// <summary>
/// Represents a complete report with title, sections, and metadata
/// </summary>
public class Report
{
    /// <summary>
    /// Gets the report title
    /// </summary>
    public string Title { get; }

    /// <summary>
    /// Gets the report footer text
    /// </summary>
    public string? Footer { get; internal set; }

    /// <summary>
    /// Gets the logo URL (optional)
    /// </summary>
    public string? LogoUrl { get; internal set; }

    /// <summary>
    /// Gets the report sections
    /// </summary>
    public List<ReportSection> Sections { get; } = new();

    /// <summary>
    /// Gets the report generation timestamp
    /// </summary>
    public DateTime GeneratedAt { get; } = DateTime.UtcNow;

    public Report(string title)
    {
        Title = title ?? throw new ArgumentNullException(nameof(title));
    }
}
