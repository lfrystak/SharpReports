using System.Globalization;
using System.Text.Json;
using SharpReports.Core;
using SharpReports.Elements;
using SharpReports.Elements.Charts;

namespace SharpReports.Rendering;

/// <summary>
/// Renders reports as JSON
/// </summary>
public class JsonRenderer : IRenderer
{
    private readonly JsonSerializerOptions _options = new()
    {
        WriteIndented = true,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };

    private CultureInfo _currentCulture = CultureInfo.InvariantCulture;

    public string Render(Report report, Theme? theme = null)
    {
        _currentCulture = report.Culture;
        var reportData = new
        {
            title = report.Title,
            footer = report.Footer,
            logoUrl = report.LogoUrl,
            generatedAt = report.GeneratedAt,
            sections = report.Sections.Select(s => new
            {
                title = s.Title,
                columns = s.Columns,
                elements = s.Elements.Select(e => SerializeElement(e)).ToList()
            }).ToList()
        };

        return JsonSerializer.Serialize(reportData, _options);
    }

    private object SerializeElement(IReportElement element)
    {
        return element switch
        {
            NumberTile tile => new
            {
                type = "NumberTile",
                id = tile.Id,
                title = tile.Title,
                value = tile.Value,
                format = tile.Format,
                subtitle = tile.Subtitle,
                formattedValue = tile.GetFormattedValue(_currentCulture)
            },
            DateTile dateTile => new
            {
                type = "DateTile",
                id = dateTile.Id,
                title = dateTile.Title,
                dateTimeValue = dateTile.DateTimeValue,
                dateOnlyValue = dateTile.DateOnlyValue?.ToDateTime(TimeOnly.MinValue),
                format = dateTile.Format,
                subtitle = dateTile.Subtitle,
                formattedValue = dateTile.GetFormattedValue(_currentCulture)
            },
            FreeText text => new
            {
                type = "FreeText",
                id = text.Id,
                content = text.Content,
                isHtml = text.IsHtml
            },
            Table table => new
            {
                type = "Table",
                id = table.Id,
                title = table.Title,
                columns = table.Columns,
                rows = table.Rows
            },
            BarChart chart => new
            {
                type = "BarChart",
                id = chart.Id,
                title = chart.Title,
                data = chart.Data,
                isHorizontal = chart.IsHorizontal
            },
            StackedBarChart chart => new
            {
                type = "StackedBarChart",
                id = chart.Id,
                title = chart.Title,
                data = chart.Data,
                isHorizontal = chart.IsHorizontal
            },
            LineChart chart => new
            {
                type = "LineChart",
                id = chart.Id,
                title = chart.Title,
                series = chart.Series,
                showPoints = chart.ShowPoints
            },
            PieChart chart => new
            {
                type = "PieChart",
                id = chart.Id,
                title = chart.Title,
                data = chart.Data,
                isDonut = chart.IsDonut
            },
            Canvas canvas => new
            {
                type = "Canvas",
                id = canvas.Id,
                columns = canvas.Columns,
                elements = canvas.Elements.Select(e => SerializeElement(e)).ToList()
            },
            _ => new
            {
                type = element.ElementType,
                id = element.Id
            }
        };
    }
}
