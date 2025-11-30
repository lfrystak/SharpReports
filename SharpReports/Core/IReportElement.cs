namespace SharpReports.Core;

/// <summary>
/// Base interface for all report elements (charts, tiles, text, tables, etc.)
/// </summary>
public interface IReportElement
{
    /// <summary>
    /// Gets the unique identifier for this element
    /// </summary>
    string Id { get; }

    /// <summary>
    /// Gets the type of this element (e.g., "NumberTile", "BarChart")
    /// </summary>
    string ElementType { get; }
}
