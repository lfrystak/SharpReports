using SharpReports.Core;

namespace SharpReports.Elements;

/// <summary>
/// A flexible container element that allows mixing tiles and charts in a custom layout with auto-flowing columns
/// </summary>
public class Canvas : ReportElementBase
{
    /// <inheritdoc/>
    public override string ElementType => "Canvas";

    /// <summary>
    /// Gets the number of columns in the canvas. Elements will auto-flow left-to-right across these columns.
    /// </summary>
    public int Columns { get; }

    /// <summary>
    /// Gets the list of elements contained within this canvas
    /// </summary>
    public List<IReportElement> Elements { get; }

    /// <summary>
    /// Initializes a new instance of the Canvas class
    /// </summary>
    /// <param name="columns">Number of columns for the canvas layout. Elements will auto-flow across these columns.</param>
    public Canvas(int columns)
    {
        if (columns < 1)
            throw new ArgumentException("Canvas must have at least 1 column", nameof(columns));

        Columns = columns;
        Elements = new List<IReportElement>();
    }

    /// <summary>
    /// Initializes a new instance of the Canvas class with a specific ID
    /// </summary>
    /// <param name="id">The unique identifier for this canvas</param>
    /// <param name="columns">Number of columns for the canvas layout. Elements will auto-flow across these columns.</param>
    public Canvas(string id, int columns) : base(id)
    {
        if (columns < 1)
            throw new ArgumentException("Canvas must have at least 1 column", nameof(columns));

        Columns = columns;
        Elements = new List<IReportElement>();
    }

    /// <summary>
    /// Adds an element to the canvas
    /// </summary>
    /// <param name="element">The element to add</param>
    /// <returns>This canvas instance for method chaining</returns>
    public Canvas AddElement(IReportElement element)
    {
        if (element == null)
            throw new ArgumentNullException(nameof(element));

        Elements.Add(element);
        return this;
    }
}
