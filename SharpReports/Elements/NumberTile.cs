namespace SharpReports.Elements;

/// <summary>
/// Displays a number with a title
/// </summary>
public class NumberTile : ReportElementBase
{
    public override string ElementType => "NumberTile";

    /// <summary>
    /// Gets the tile title
    /// </summary>
    public string Title { get; }

    /// <summary>
    /// Gets the numeric value to display
    /// </summary>
    public double Value { get; }

    /// <summary>
    /// Gets the format string for the value (e.g., "N2", "C", "P")
    /// </summary>
    public string? Format { get; }

    /// <summary>
    /// Gets the optional subtitle/description
    /// </summary>
    public string? Subtitle { get; }

    public NumberTile(string title, double value, string? format = null, string? subtitle = null)
    {
        Title = title ?? throw new ArgumentNullException(nameof(title));
        Value = value;
        Format = format;
        Subtitle = subtitle;
    }

    /// <summary>
    /// Gets the formatted value string
    /// </summary>
    public string GetFormattedValue()
    {
        return Format != null ? Value.ToString(Format) : Value.ToString("N0");
    }
}
