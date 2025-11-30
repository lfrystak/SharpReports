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
    public static ReportSection AddNumberTile(this ReportSection section, string title, double value, string? format = null, string? subtitle = null)
    {
        return section.AddElement(new NumberTile(title, value, format, subtitle));
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
    public static ReportSection AddBarChart(this ReportSection section, string title, Dictionary<string, double> data, bool isHorizontal = false)
    {
        return section.AddElement(new BarChart(title, data, isHorizontal));
    }

    /// <summary>
    /// Adds a stacked bar chart to the section
    /// </summary>
    public static ReportSection AddStackedBarChart(this ReportSection section, string title, Dictionary<string, Dictionary<string, double>> data, bool isHorizontal = false)
    {
        return section.AddElement(new StackedBarChart(title, data, isHorizontal));
    }

    /// <summary>
    /// Adds a simple stacked bar chart to the section
    /// </summary>
    public static ReportSection AddStackedBarChart(this ReportSection section, string title, Dictionary<string, int> data, bool isHorizontal = false)
    {
        return section.AddElement(new StackedBarChart(title, data, isHorizontal));
    }

    /// <summary>
    /// Adds a line chart to the section
    /// </summary>
    public static ReportSection AddLineChart(this ReportSection section, string title, Dictionary<string, Dictionary<string, double>> series, bool showPoints = true)
    {
        return section.AddElement(new LineChart(title, series, showPoints));
    }

    /// <summary>
    /// Adds a single-series line chart to the section
    /// </summary>
    public static ReportSection AddLineChart(this ReportSection section, string title, Dictionary<string, double> data, bool showPoints = true)
    {
        return section.AddElement(new LineChart(title, data, showPoints));
    }

    /// <summary>
    /// Adds a pie chart to the section
    /// </summary>
    public static ReportSection AddPieChart(this ReportSection section, string title, Dictionary<string, double> data, bool isDonut = false)
    {
        return section.AddElement(new PieChart(title, data, isDonut));
    }
}
