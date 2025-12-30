namespace SharpReports.Elements.Charts;

/// <summary>
/// Base class for all chart types
/// </summary>
public abstract class ChartBase : ReportElementBase
{
    /// <summary>
    /// Gets the chart title
    /// </summary>
    public string Title { get; }

    /// <summary>
    /// Gets the optional tooltip text shown on hover
    /// </summary>
    public string? Tooltip { get; }

    protected ChartBase(string title, string? tooltip = null)
    {
        Title = title ?? throw new ArgumentNullException(nameof(title));
        Tooltip = tooltip;
    }
}
