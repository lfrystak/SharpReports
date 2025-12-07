namespace SharpReports.Elements.Charts;

/// <summary>
/// Displays a line chart
/// </summary>
public class LineChart : ChartBase
{
    public override string ElementType => "LineChart";

    /// <summary>
    /// Gets the chart data series (series name -> (label -> value))
    /// </summary>
    public Dictionary<string, Dictionary<string, double>> Series { get; }

    /// <summary>
    /// Gets whether to show points on the line
    /// </summary>
    public bool ShowPoints { get; }

    public LineChart(string title, IDictionary<string, IDictionary<string, double>> series, bool showPoints = true)
        : base(title)
    {
        if (series == null) throw new ArgumentNullException(nameof(series));
        Series = series.ToDictionary(
            kvp => kvp.Key,
            kvp => kvp.Value is Dictionary<string, double> dict ? dict : new Dictionary<string, double>(kvp.Value)
        );
        ShowPoints = showPoints;
    }

    /// <summary>
    /// Alternative constructor for single series
    /// </summary>
    public LineChart(string title, IDictionary<string, double> data, bool showPoints = true)
        : base(title)
    {
        if (data == null)
            throw new ArgumentNullException(nameof(data));

        var dataDict = data is Dictionary<string, double> dict ? dict : new Dictionary<string, double>(data);
        Series = new Dictionary<string, Dictionary<string, double>>
        {
            [title] = dataDict
        };
        ShowPoints = showPoints;
    }
}
