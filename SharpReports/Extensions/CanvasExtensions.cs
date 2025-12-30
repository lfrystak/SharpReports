using SharpReports.Elements;
using SharpReports.Elements.Charts;

namespace SharpReports.Extensions;

/// <summary>
/// Extension methods for Canvas to enable fluent element addition
/// </summary>
public static class CanvasExtensions
{
    /// <summary>
    /// Adds a number tile to the canvas
    /// </summary>
    public static Canvas AddNumberTile(this Canvas canvas, string title, double value, string? format = null, string? subtitle = null, string? tooltip = null)
    {
        return canvas.AddElement(new NumberTile(title, value, format, subtitle, tooltip));
    }

    /// <summary>
    /// Adds a date tile to the canvas
    /// </summary>
    public static Canvas AddDateTile(this Canvas canvas, string title, DateTime value, string? format = null, string? subtitle = null, string? tooltip = null)
    {
        return canvas.AddElement(new DateTile(title, value, format, subtitle, tooltip));
    }

    /// <summary>
    /// Adds a date tile to the canvas
    /// </summary>
    public static Canvas AddDateTile(this Canvas canvas, string title, DateOnly value, string? format = null, string? subtitle = null, string? tooltip = null)
    {
        return canvas.AddElement(new DateTile(title, value, format, subtitle, tooltip));
    }

    /// <summary>
    /// Adds free text to the canvas
    /// </summary>
    public static Canvas AddText(this Canvas canvas, string content, bool isHtml = false)
    {
        return canvas.AddElement(new FreeText(content, isHtml));
    }

    /// <summary>
    /// Adds a table to the canvas
    /// </summary>
    public static Canvas AddTable(this Canvas canvas, string title, IEnumerable<Dictionary<string, object>> rows)
    {
        return canvas.AddElement(new Table(title, rows));
    }

    /// <summary>
    /// Adds a table from column-based data to the canvas
    /// </summary>
    public static Canvas AddTableFromColumns(this Canvas canvas, string title, Dictionary<string, IEnumerable<object>> columns)
    {
        return canvas.AddElement(Table.FromColumns(title, columns));
    }

    /// <summary>
    /// Adds a bar chart to the canvas
    /// </summary>
    public static Canvas AddBarChart(this Canvas canvas, string title, IDictionary<string, double> data, bool isHorizontal = false, string? tooltip = null)
    {
        return canvas.AddElement(new BarChart(title, data, isHorizontal, tooltip));
    }

    /// <summary>
    /// Adds a bar chart to the canvas
    /// </summary>
    public static Canvas AddBarChart(this Canvas canvas, string title, IDictionary<string, int> data, bool isHorizontal = false, string? tooltip = null)
    {
        return canvas.AddElement(new BarChart(title, data, isHorizontal, tooltip));
    }

    /// <summary>
    /// Adds a stacked bar chart to the canvas
    /// </summary>
    public static Canvas AddStackedBarChart(this Canvas canvas, string title, IDictionary<string, IDictionary<string, double>> data, bool isHorizontal = false, string? tooltip = null)
    {
        return canvas.AddElement(new StackedBarChart(title, data, isHorizontal, tooltip));
    }

    /// <summary>
    /// Adds a stacked bar chart to the canvas
    /// </summary>
    public static Canvas AddStackedBarChart(this Canvas canvas, string title, IDictionary<string, IDictionary<string, int>> data, bool isHorizontal = false, string? tooltip = null)
    {
        return canvas.AddElement(new StackedBarChart(title, data, isHorizontal, tooltip));
    }

    /// <summary>
    /// Adds a simple stacked bar chart to the canvas
    /// </summary>
    public static Canvas AddStackedBarChart(this Canvas canvas, string title, IDictionary<string, int> data, bool isHorizontal = false, string? tooltip = null)
    {
        return canvas.AddElement(new StackedBarChart(title, data, isHorizontal, tooltip));
    }

    /// <summary>
    /// Adds a line chart to the canvas
    /// </summary>
    public static Canvas AddLineChart(this Canvas canvas, string title, IDictionary<string, IDictionary<string, double>> series, bool showPoints = true, string? tooltip = null)
    {
        return canvas.AddElement(new LineChart(title, series, showPoints, tooltip));
    }

    /// <summary>
    /// Adds a single-series line chart to the canvas
    /// </summary>
    public static Canvas AddLineChart(this Canvas canvas, string title, IDictionary<string, double> data, bool showPoints = true, string? tooltip = null)
    {
        return canvas.AddElement(new LineChart(title, data, showPoints, tooltip));
    }

    /// <summary>
    /// Adds a pie chart to the canvas
    /// </summary>
    public static Canvas AddPieChart(this Canvas canvas, string title, IDictionary<string, double> data, bool isDonut = false, string? tooltip = null)
    {
        return canvas.AddElement(new PieChart(title, data, isDonut, tooltip));
    }

    /// <summary>
    /// Adds a pie chart to the canvas
    /// </summary>
    public static Canvas AddPieChart(this Canvas canvas, string title, IDictionary<string, int> data, bool isDonut = false, string? tooltip = null)
    {
        return canvas.AddElement(new PieChart(title, data, isDonut, tooltip));
    }
}
