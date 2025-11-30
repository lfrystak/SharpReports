namespace SharpReports.Elements.Charts;

/// <summary>
/// Displays a bar chart
/// </summary>
public class BarChart : ChartBase
{
    public override string ElementType => "BarChart";

    /// <summary>
    /// Gets the chart data (label -> value)
    /// </summary>
    public Dictionary<string, double> Data { get; }

    /// <summary>
    /// Gets whether the chart should be horizontal (default: false, vertical bars)
    /// </summary>
    public bool IsHorizontal { get; }

    public BarChart(string title, Dictionary<string, double> data, bool isHorizontal = false)
        : base(title)
    {
        Data = data ?? throw new ArgumentNullException(nameof(data));
        IsHorizontal = isHorizontal;
    }
}
