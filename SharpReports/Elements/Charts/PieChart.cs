namespace SharpReports.Elements.Charts;

/// <summary>
/// Displays a pie chart
/// </summary>
public class PieChart : ChartBase
{
    public override string ElementType => "PieChart";

    /// <summary>
    /// Gets the chart data (label -> value)
    /// </summary>
    public Dictionary<string, double> Data { get; }

    /// <summary>
    /// Gets whether to display as a donut chart (default: false)
    /// </summary>
    public bool IsDonut { get; }

    public PieChart(string title, Dictionary<string, double> data, bool isDonut = false)
        : base(title)
    {
        Data = data ?? throw new ArgumentNullException(nameof(data));
        IsDonut = isDonut;
    }
}
