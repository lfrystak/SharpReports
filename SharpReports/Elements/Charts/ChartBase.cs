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

    protected ChartBase(string title)
    {
        Title = title ?? throw new ArgumentNullException(nameof(title));
    }
}
