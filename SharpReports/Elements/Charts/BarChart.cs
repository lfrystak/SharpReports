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

    public BarChart(string title, IDictionary<string, double> data, bool isHorizontal = false, string? tooltip = null)
        : base(title, tooltip)
    {
        if (data == null) throw new ArgumentNullException(nameof(data));
        Data = data is Dictionary<string, double> dict ? dict : new Dictionary<string, double>(data);
        IsHorizontal = isHorizontal;
    }

    public BarChart(string title, IDictionary<string, int> data, bool isHorizontal = false, string? tooltip = null)
        : base(title, tooltip)
    {
        if (data == null) throw new ArgumentNullException(nameof(data));
        Data = data.ToDictionary(kvp => kvp.Key, kvp => (double)kvp.Value);
        IsHorizontal = isHorizontal;
    }
}
