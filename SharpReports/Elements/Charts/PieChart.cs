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

    public PieChart(string title, IDictionary<string, double> data, bool isDonut = false)
        : base(title)
    {
        if (data == null) throw new ArgumentNullException(nameof(data));
        Data = data is Dictionary<string, double> dict ? dict : new Dictionary<string, double>(data);
        IsDonut = isDonut;
    }

    public PieChart(string title, IDictionary<string, int> data, bool isDonut = false)
        : base(title)
    {
        if (data == null) throw new ArgumentNullException(nameof(data));
        Data = data.ToDictionary(kvp => kvp.Key, kvp => (double)kvp.Value);
        IsDonut = isDonut;
    }
}
