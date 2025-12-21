namespace SharpReports.Elements;

/// <summary>
/// Displays a date or datetime with a title
/// </summary>
public class DateTile : ReportElementBase
{
    public override string ElementType => "DateTile";

    /// <summary>
    /// Gets the tile title
    /// </summary>
    public string Title { get; }

    /// <summary>
    /// Gets the DateTime value to display (nullable to support DateOnly)
    /// </summary>
    public DateTime? DateTimeValue { get; }

    /// <summary>
    /// Gets the DateOnly value to display (nullable to support DateTime)
    /// </summary>
    public DateOnly? DateOnlyValue { get; }

    /// <summary>
    /// Gets the format string for the value (e.g., "yyyy-MM-dd", "dd MMM yyyy")
    /// </summary>
    public string? Format { get; }

    /// <summary>
    /// Gets the optional subtitle/description
    /// </summary>
    public string? Subtitle { get; }

    /// <summary>
    /// Gets the optional tooltip text shown on hover
    /// </summary>
    public string? Tooltip { get; }

    public DateTile(string title, DateTime value, string? format = null, string? subtitle = null, string? tooltip = null)
    {
        Title = title ?? throw new ArgumentNullException(nameof(title));
        DateTimeValue = value;
        DateOnlyValue = null;
        Format = format;
        Subtitle = subtitle;
        Tooltip = tooltip;
    }

    public DateTile(string title, DateOnly value, string? format = null, string? subtitle = null, string? tooltip = null)
    {
        Title = title ?? throw new ArgumentNullException(nameof(title));
        DateTimeValue = null;
        DateOnlyValue = value;
        Format = format;
        Subtitle = subtitle;
        Tooltip = tooltip;
    }

    /// <summary>
    /// Gets the formatted value string
    /// </summary>
    public string GetFormattedValue()
    {
        if (DateTimeValue.HasValue)
        {
            return Format != null ? DateTimeValue.Value.ToString(Format) : DateTimeValue.Value.ToString("g");
        }
        else if (DateOnlyValue.HasValue)
        {
            return Format != null ? DateOnlyValue.Value.ToString(Format) : DateOnlyValue.Value.ToString("d");
        }
        return string.Empty;
    }
}
