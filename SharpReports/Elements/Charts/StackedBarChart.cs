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

    public StackedBarChart(string title, IDictionary<string, IDictionary<string, double>> data, bool isHorizontal = false, string? tooltip = null)
        : base(title, tooltip)
    {
        if (data == null) throw new ArgumentNullException(nameof(data));
        Data = data.ToDictionary(
            kvp => kvp.Key,
            kvp => kvp.Value is Dictionary<string, double> dict ? dict : new Dictionary<string, double>(kvp.Value)
        );
        IsHorizontal = isHorizontal;
    }

    public StackedBarChart(string title, IDictionary<string, IDictionary<string, int>> data, bool isHorizontal = false, string? tooltip = null)
        : base(title, tooltip)
    {
        if (data == null) throw new ArgumentNullException(nameof(data));
        Data = data.ToDictionary(
            kvp => kvp.Key,
            kvp => kvp.Value.ToDictionary(innerKvp => innerKvp.Key, innerKvp => (double)innerKvp.Value)
        );
        IsHorizontal = isHorizontal;
    }

    /// <summary>
    /// Alternative constructor for simple stacked data (category -> value, all in one series)
    /// </summary>
    public StackedBarChart(string title, IDictionary<string, int> data, bool isHorizontal = false, string? tooltip = null)
        : base(title, tooltip)
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
