namespace SharpReports.Elements.Charts;

/// <summary>
/// Displays a stacked bar chart
/// </summary>
public class StackedBarChart : ChartBase
{
    public override string ElementType => "StackedBarChart";

    /// <summary>
    /// Gets the chart data (category -> (series -> value))
    /// </summary>
    public Dictionary<string, Dictionary<string, double>> Data { get; }

    /// <summary>
    /// Gets whether the chart should be horizontal (default: false, vertical bars)
    /// </summary>
    public bool IsHorizontal { get; }

    public StackedBarChart(string title, Dictionary<string, Dictionary<string, double>> data, bool isHorizontal = false)
        : base(title)
    {
        Data = data ?? throw new ArgumentNullException(nameof(data));
        IsHorizontal = isHorizontal;
    }

    /// <summary>
    /// Alternative constructor for simple stacked data (category -> value, all in one series)
    /// </summary>
    public StackedBarChart(string title, Dictionary<string, int> data, bool isHorizontal = false)
        : base(title)
    {
        if (data == null)
            throw new ArgumentNullException(nameof(data));

        // Convert to the full format with a single series
        Data = data.ToDictionary(
            kvp => kvp.Key,
            kvp => new Dictionary<string, double> { ["Value"] = kvp.Value }
        );
        IsHorizontal = isHorizontal;
    }
}
