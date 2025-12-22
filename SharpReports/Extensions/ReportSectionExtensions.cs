using SharpReports.Core;
using SharpReports.Elements;
using SharpReports.Elements.Charts;

namespace SharpReports.Extensions;

/// <summary>
/// Extension methods for ReportSection to enable fluent element addition
/// </summary>
public static class ReportSectionExtensions
{
    /// <summary>
    /// Adds a number tile to the section
    /// </summary>
    public static ReportSection AddNumberTile(this ReportSection section, string title, double value, string? format = null, string? subtitle = null, string? tooltip = null)
    {
        return section.AddElement(new NumberTile(title, value, format, subtitle, tooltip));
    }

    /// <summary>
    /// Adds a date tile to the section
    /// </summary>
    public static ReportSection AddDateTile(this ReportSection section, string title, DateTime value, string? format = null, string? subtitle = null, string? tooltip = null)
    {
        return section.AddElement(new DateTile(title, value, format, subtitle, tooltip));
    }

    /// <summary>
    /// Adds a date tile to the section
    /// </summary>
    public static ReportSection AddDateTile(this ReportSection section, string title, DateOnly value, string? format = null, string? subtitle = null, string? tooltip = null)
    {
        return section.AddElement(new DateTile(title, value, format, subtitle, tooltip));
    }

    /// <summary>
    /// Adds free text to the section
    /// </summary>
    public static ReportSection AddText(this ReportSection section, string content, bool isHtml = false)
    {
        return section.AddElement(new FreeText(content, isHtml));
    }

    /// <summary>
    /// Adds a table to the section
    /// </summary>
    public static ReportSection AddTable(this ReportSection section, string title, IEnumerable<Dictionary<string, object>> rows)
    {
        return section.AddElement(new Table(title, rows));
    }

    /// <summary>
    /// Adds a table from column-based data to the section
    /// </summary>
    public static ReportSection AddTableFromColumns(this ReportSection section, string title, Dictionary<string, IEnumerable<object>> columns)
    {
        return section.AddElement(Table.FromColumns(title, columns));
    }

    /// <summary>
    /// Adds a bar chart to the section
    /// </summary>
    public static ReportSection AddBarChart(this ReportSection section, string title, IDictionary<string, double> data, bool isHorizontal = false, string? tooltip = null)
    {
        return section.AddElement(new BarChart(title, data, isHorizontal, tooltip));
    }

    /// <summary>
    /// Adds a bar chart to the section
    /// </summary>
    public static ReportSection AddBarChart(this ReportSection section, string title, IDictionary<string, int> data, bool isHorizontal = false, string? tooltip = null)
    {
        return section.AddElement(new BarChart(title, data, isHorizontal, tooltip));
    }

    /// <summary>
    /// Adds a stacked bar chart to the section
    /// </summary>
    public static ReportSection AddStackedBarChart(this ReportSection section, string title, IDictionary<string, IDictionary<string, double>> data, bool isHorizontal = false, string? tooltip = null)
    {
        return section.AddElement(new StackedBarChart(title, data, isHorizontal, tooltip));
    }

    /// <summary>
    /// Adds a stacked bar chart to the section
    /// </summary>
    public static ReportSection AddStackedBarChart(this ReportSection section, string title, IDictionary<string, IDictionary<string, int>> data, bool isHorizontal = false, string? tooltip = null)
    {
        return section.AddElement(new StackedBarChart(title, data, isHorizontal, tooltip));
    }

    /// <summary>
    /// Adds a simple stacked bar chart to the section
    /// </summary>
    public static ReportSection AddStackedBarChart(this ReportSection section, string title, IDictionary<string, int> data, bool isHorizontal = false, string? tooltip = null)
    {
        return section.AddElement(new StackedBarChart(title, data, isHorizontal, tooltip));
    }

    /// <summary>
    /// Adds a line chart to the section
    /// </summary>
    public static ReportSection AddLineChart(this ReportSection section, string title, IDictionary<string, IDictionary<string, double>> series, bool showPoints = true, string? tooltip = null)
    {
        return section.AddElement(new LineChart(title, series, showPoints, tooltip));
    }

    /// <summary>
    /// Adds a single-series line chart to the section
    /// </summary>
    public static ReportSection AddLineChart(this ReportSection section, string title, IDictionary<string, double> data, bool showPoints = true, string? tooltip = null)
    {
        return section.AddElement(new LineChart(title, data, showPoints, tooltip));
    }

    /// <summary>
    /// Adds a pie chart to the section
    /// </summary>
    public static ReportSection AddPieChart(this ReportSection section, string title, IDictionary<string, double> data, bool isDonut = false, string? tooltip = null)
    {
        return section.AddElement(new PieChart(title, data, isDonut, tooltip));
    }

    /// <summary>
    /// Adds a pie chart to the section
    /// </summary>
    public static ReportSection AddPieChart(this ReportSection section, string title, IDictionary<string, int> data, bool isDonut = false, string? tooltip = null)
    {
        return section.AddElement(new PieChart(title, data, isDonut, tooltip));
    }

    /// <summary>
    /// Adds a canvas to the section. Canvas will fill 100% of its section column.
    /// Use SetColumnWidths() on the section to control the canvas width.
    /// </summary>
    /// <param name="section">The section to add the canvas to</param>
    /// <param name="columns">Number of columns in the canvas. Elements will auto-flow across these columns.</param>
    /// <param name="configure">Action to configure the canvas by adding elements</param>
    /// <returns>The section for method chaining</returns>
    public static ReportSection AddCanvas(this ReportSection section, int columns, Action<Canvas> configure)
    {
        var canvas = new Canvas(columns);
        configure(canvas);
        return section.AddElement(canvas);
    }
}
