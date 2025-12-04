using System.Text;
using SharpReports.Core;
using SharpReports.Elements;
using SharpReports.Elements.Charts;

namespace SharpReports.Rendering;

/// <summary>
/// Renders reports as HTML with Chart.js integration
/// </summary>
public class HtmlRenderer : IRenderer
{
    public string Render(Report report, Theme? theme = null)
    {
        theme ??= Theme.Default;
        var sb = new StringBuilder();

        // HTML structure
        sb.AppendLine("<!DOCTYPE html>");
        sb.AppendLine("<html lang=\"en\">");
        sb.AppendLine("<head>");
        sb.AppendLine("    <meta charset=\"UTF-8\">");
        sb.AppendLine("    <meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0\">");
        sb.AppendLine($"    <title>{EscapeHtml(report.Title)}</title>");
        sb.AppendLine("    <script src=\"https://cdn.jsdelivr.net/npm/chart.js@4.4.0/dist/chart.umd.min.js\"></script>");

        // CSS
        sb.AppendLine("    <style>");
        sb.AppendLine(GenerateCss(theme));
        sb.AppendLine("    </style>");
        sb.AppendLine("</head>");
        sb.AppendLine("<body>");

        // Header
        sb.AppendLine("    <div class=\"report-header\">");
        if (!string.IsNullOrEmpty(report.LogoUrl))
        {
            sb.AppendLine($"        <img src=\"{EscapeHtml(report.LogoUrl)}\" alt=\"Logo\" class=\"logo\">");
        }
        sb.AppendLine($"        <h1>{EscapeHtml(report.Title)}</h1>");
        sb.AppendLine("    </div>");

        // Content
        sb.AppendLine("    <div class=\"report-content\">");

        foreach (var section in report.Sections)
        {
            RenderSection(sb, section);
        }

        sb.AppendLine("    </div>");

        // Footer
        if (!string.IsNullOrEmpty(report.Footer))
        {
            sb.AppendLine("    <div class=\"report-footer\">");
            sb.AppendLine($"        <p>{EscapeHtml(report.Footer)}</p>");
            sb.AppendLine($"        <p class=\"timestamp\">Generated: {report.GeneratedAt:yyyy-MM-dd HH:mm:ss} UTC</p>");
            sb.AppendLine("    </div>");
        }

        sb.AppendLine("</body>");
        sb.AppendLine("</html>");

        return sb.ToString();
    }

    private void RenderSection(StringBuilder sb, ReportSection section)
    {
        sb.AppendLine($"    <div class=\"section\">");
        sb.AppendLine($"        <h2>{EscapeHtml(section.Title)}</h2>");
        sb.AppendLine($"        <div class=\"grid grid-cols-{section.Columns}\">");

        foreach (var element in section.Elements)
        {
            RenderElement(sb, element);
        }

        sb.AppendLine("        </div>");
        sb.AppendLine("    </div>");
    }

    private void RenderElement(StringBuilder sb, IReportElement element)
    {
        sb.AppendLine("            <div class=\"element\">");

        switch (element)
        {
            case NumberTile tile:
                RenderNumberTile(sb, tile);
                break;
            case DateTile dateTile:
                RenderDateTile(sb, dateTile);
                break;
            case FreeText text:
                RenderFreeText(sb, text);
                break;
            case Table table:
                RenderTable(sb, table);
                break;
            case BarChart barChart:
                RenderBarChart(sb, barChart);
                break;
            case StackedBarChart stackedBarChart:
                RenderStackedBarChart(sb, stackedBarChart);
                break;
            case LineChart lineChart:
                RenderLineChart(sb, lineChart);
                break;
            case PieChart pieChart:
                RenderPieChart(sb, pieChart);
                break;
            default:
                sb.AppendLine($"                <p>Unknown element type: {element.ElementType}</p>");
                break;
        }

        sb.AppendLine("            </div>");
    }

    private void RenderNumberTile(StringBuilder sb, NumberTile tile)
    {
        sb.AppendLine("                <div class=\"number-tile\">");
        sb.AppendLine($"                    <div class=\"tile-title\">{EscapeHtml(tile.Title)}</div>");
        sb.AppendLine($"                    <div class=\"tile-value\">{EscapeHtml(tile.GetFormattedValue())}</div>");
        if (!string.IsNullOrEmpty(tile.Subtitle))
        {
            sb.AppendLine($"                    <div class=\"tile-subtitle\">{EscapeHtml(tile.Subtitle)}</div>");
        }
        sb.AppendLine("                </div>");
    }

    private void RenderDateTile(StringBuilder sb, DateTile tile)
    {
        sb.AppendLine("                <div class=\"number-tile\">");
        sb.AppendLine($"                    <div class=\"tile-title\">{EscapeHtml(tile.Title)}</div>");
        sb.AppendLine($"                    <div class=\"tile-value\">{EscapeHtml(tile.GetFormattedValue())}</div>");
        if (!string.IsNullOrEmpty(tile.Subtitle))
        {
            sb.AppendLine($"                    <div class=\"tile-subtitle\">{EscapeHtml(tile.Subtitle)}</div>");
        }
        sb.AppendLine("                </div>");
    }

    private void RenderFreeText(StringBuilder sb, FreeText text)
    {
        sb.AppendLine("                <div class=\"free-text\">");
        if (text.IsHtml)
        {
            sb.AppendLine($"                    {text.Content}");
        }
        else
        {
            sb.AppendLine($"                    <p>{EscapeHtml(text.Content)}</p>");
        }
        sb.AppendLine("                </div>");
    }

    private void RenderTable(StringBuilder sb, Table table)
    {
        sb.AppendLine("                <div class=\"table-container\">");
        sb.AppendLine($"                    <h3>{EscapeHtml(table.Title)}</h3>");
        sb.AppendLine("                    <table>");
        sb.AppendLine("                        <thead>");
        sb.AppendLine("                            <tr>");
        foreach (var col in table.Columns)
        {
            sb.AppendLine($"                                <th>{EscapeHtml(col)}</th>");
        }
        sb.AppendLine("                            </tr>");
        sb.AppendLine("                        </thead>");
        sb.AppendLine("                        <tbody>");
        foreach (var row in table.Rows)
        {
            sb.AppendLine("                            <tr>");
            foreach (var col in table.Columns)
            {
                var value = row.ContainsKey(col) ? row[col]?.ToString() ?? "" : "";
                sb.AppendLine($"                                <td>{EscapeHtml(value)}</td>");
            }
            sb.AppendLine("                            </tr>");
        }
        sb.AppendLine("                        </tbody>");
        sb.AppendLine("                    </table>");
        sb.AppendLine("                </div>");
    }

    private void RenderBarChart(StringBuilder sb, BarChart chart)
    {
        var chartId = chart.Id;
        sb.AppendLine($"                <div class=\"chart-container\">");
        sb.AppendLine($"                    <h3>{EscapeHtml(chart.Title)}</h3>");
        sb.AppendLine($"                    <canvas id=\"chart-{chartId}\"></canvas>");
        sb.AppendLine("                </div>");
        sb.AppendLine("                <script>");
        sb.AppendLine($"                    new Chart(document.getElementById('chart-{chartId}'), {{");
        sb.AppendLine($"                        type: '{(chart.IsHorizontal ? "bar" : "bar")}',");
        sb.AppendLine($"                        data: {{");
        sb.AppendLine($"                            labels: {ToJsonArray(chart.Data.Keys)},");
        sb.AppendLine($"                            datasets: [{{");
        sb.AppendLine($"                                data: {ToJsonArray(chart.Data.Values)},");
        sb.AppendLine($"                                backgroundColor: 'rgba(37, 99, 235, 0.7)',");
        sb.AppendLine($"                                borderColor: 'rgba(37, 99, 235, 1)',");
        sb.AppendLine($"                                borderWidth: 1");
        sb.AppendLine($"                            }}]");
        sb.AppendLine($"                        }},");
        sb.AppendLine($"                        options: {{");
        sb.AppendLine($"                            indexAxis: '{(chart.IsHorizontal ? "y" : "x")}',");
        sb.AppendLine($"                            responsive: true,");
        sb.AppendLine($"                            maintainAspectRatio: true,");
        sb.AppendLine($"                            plugins: {{ legend: {{ display: false }} }}");
        sb.AppendLine($"                        }}");
        sb.AppendLine($"                    }});");
        sb.AppendLine("                </script>");
    }

    private void RenderStackedBarChart(StringBuilder sb, StackedBarChart chart)
    {
        var chartId = chart.Id;
        var categories = chart.Data.Keys.ToList();
        var allSeries = chart.Data.SelectMany(d => d.Value.Keys).Distinct().ToList();

        sb.AppendLine($"                <div class=\"chart-container\">");
        sb.AppendLine($"                    <h3>{EscapeHtml(chart.Title)}</h3>");
        sb.AppendLine($"                    <canvas id=\"chart-{chartId}\"></canvas>");
        sb.AppendLine("                </div>");
        sb.AppendLine("                <script>");
        sb.AppendLine($"                    new Chart(document.getElementById('chart-{chartId}'), {{");
        sb.AppendLine($"                        type: 'bar',");
        sb.AppendLine($"                        data: {{");
        sb.AppendLine($"                            labels: {ToJsonArray(categories)},");
        sb.AppendLine($"                            datasets: [");

        var colors = new[] { "rgba(37, 99, 235, 0.7)", "rgba(100, 116, 139, 0.7)", "rgba(239, 68, 68, 0.7)", "rgba(34, 197, 94, 0.7)" };
        for (int i = 0; i < allSeries.Count; i++)
        {
            var series = allSeries[i];
            var data = categories.Select(cat =>
                chart.Data[cat].ContainsKey(series) ? chart.Data[cat][series] : 0
            );

            sb.AppendLine($"                                {{");
            sb.AppendLine($"                                    label: {ToJsonString(series)},");
            sb.AppendLine($"                                    data: {ToJsonArray(data)},");
            sb.AppendLine($"                                    backgroundColor: '{colors[i % colors.Length]}'");
            sb.AppendLine($"                                }}{(i < allSeries.Count - 1 ? "," : "")}");
        }

        sb.AppendLine($"                            ]");
        sb.AppendLine($"                        }},");
        sb.AppendLine($"                        options: {{");
        sb.AppendLine($"                            indexAxis: '{(chart.IsHorizontal ? "y" : "x")}',");
        sb.AppendLine($"                            responsive: true,");
        sb.AppendLine($"                            maintainAspectRatio: true,");
        sb.AppendLine($"                            scales: {{ x: {{ stacked: true }}, y: {{ stacked: true }} }}");
        sb.AppendLine($"                        }}");
        sb.AppendLine($"                    }});");
        sb.AppendLine("                </script>");
    }

    private void RenderLineChart(StringBuilder sb, LineChart chart)
    {
        var chartId = chart.Id;
        var allLabels = chart.Series.SelectMany(s => s.Value.Keys).Distinct().OrderBy(x => x).ToList();

        sb.AppendLine($"                <div class=\"chart-container\">");
        sb.AppendLine($"                    <h3>{EscapeHtml(chart.Title)}</h3>");
        sb.AppendLine($"                    <canvas id=\"chart-{chartId}\"></canvas>");
        sb.AppendLine("                </div>");
        sb.AppendLine("                <script>");
        sb.AppendLine($"                    new Chart(document.getElementById('chart-{chartId}'), {{");
        sb.AppendLine($"                        type: 'line',");
        sb.AppendLine($"                        data: {{");
        sb.AppendLine($"                            labels: {ToJsonArray(allLabels)},");
        sb.AppendLine($"                            datasets: [");

        var colors = new[] { "rgba(37, 99, 235, 1)", "rgba(100, 116, 139, 1)", "rgba(239, 68, 68, 1)", "rgba(34, 197, 94, 1)" };
        var seriesIndex = 0;
        foreach (var series in chart.Series)
        {
            var data = allLabels.Select(label =>
                series.Value.ContainsKey(label) ? series.Value[label] : (double?)null
            );

            sb.AppendLine($"                                {{");
            sb.AppendLine($"                                    label: {ToJsonString(series.Key)},");
            sb.AppendLine($"                                    data: {ToJsonArray(data)},");
            sb.AppendLine($"                                    borderColor: '{colors[seriesIndex % colors.Length]}',");
            sb.AppendLine($"                                    backgroundColor: '{colors[seriesIndex % colors.Length].Replace("1)", "0.1)")}',");
            sb.AppendLine($"                                    tension: 0.1,");
            sb.AppendLine($"                                    pointRadius: {(chart.ShowPoints ? "3" : "0")}");
            sb.AppendLine($"                                }}{(seriesIndex < chart.Series.Count - 1 ? "," : "")}");
            seriesIndex++;
        }

        sb.AppendLine($"                            ]");
        sb.AppendLine($"                        }},");
        sb.AppendLine($"                        options: {{");
        sb.AppendLine($"                            responsive: true,");
        sb.AppendLine($"                            maintainAspectRatio: true");
        sb.AppendLine($"                        }}");
        sb.AppendLine($"                    }});");
        sb.AppendLine("                </script>");
    }

    private void RenderPieChart(StringBuilder sb, PieChart chart)
    {
        var chartId = chart.Id;
        sb.AppendLine($"                <div class=\"chart-container\">");
        sb.AppendLine($"                    <h3>{EscapeHtml(chart.Title)}</h3>");
        sb.AppendLine($"                    <canvas id=\"chart-{chartId}\"></canvas>");
        sb.AppendLine("                </div>");
        sb.AppendLine("                <script>");
        sb.AppendLine($"                    new Chart(document.getElementById('chart-{chartId}'), {{");
        sb.AppendLine($"                        type: '{(chart.IsDonut ? "doughnut" : "pie")}',");
        sb.AppendLine($"                        data: {{");
        sb.AppendLine($"                            labels: {ToJsonArray(chart.Data.Keys)},");
        sb.AppendLine($"                            datasets: [{{");
        sb.AppendLine($"                                data: {ToJsonArray(chart.Data.Values)},");
        sb.AppendLine($"                                backgroundColor: [");
        sb.AppendLine($"                                    'rgba(37, 99, 235, 0.7)',");
        sb.AppendLine($"                                    'rgba(100, 116, 139, 0.7)',");
        sb.AppendLine($"                                    'rgba(239, 68, 68, 0.7)',");
        sb.AppendLine($"                                    'rgba(34, 197, 94, 0.7)',");
        sb.AppendLine($"                                    'rgba(251, 191, 36, 0.7)',");
        sb.AppendLine($"                                    'rgba(168, 85, 247, 0.7)'");
        sb.AppendLine($"                                ]");
        sb.AppendLine($"                            }}]");
        sb.AppendLine($"                        }},");
        sb.AppendLine($"                        options: {{");
        sb.AppendLine($"                            responsive: true,");
        sb.AppendLine($"                            maintainAspectRatio: true");
        sb.AppendLine($"                        }}");
        sb.AppendLine($"                    }});");
        sb.AppendLine("                </script>");
    }

    private string GenerateCss(Theme theme)
    {
        return $@"
        :root {{
            --primary-color: {theme.PrimaryColor};
            --secondary-color: {theme.SecondaryColor};
            --background-color: {theme.BackgroundColor};
            --text-color: {theme.TextColor};
            --font-family: {theme.FontFamily};
        }}

        * {{
            margin: 0;
            padding: 0;
            box-sizing: border-box;
        }}

        body {{
            font-family: var(--font-family);
            color: var(--text-color);
            background-color: #f8fafc;
            padding: 2rem;
        }}

        .report-header {{
            background-color: var(--background-color);
            padding: 2rem;
            border-radius: 0.5rem;
            box-shadow: 0 1px 3px rgba(0, 0, 0, 0.1);
            margin-bottom: 2rem;
            display: flex;
            justify-content: space-between;
            align-items: center;
        }}

        .report-header h1 {{
            color: var(--primary-color);
            font-size: 2rem;
            font-weight: 700;
        }}

        .logo {{
            max-height: 60px;
            max-width: 200px;
        }}

        .report-content {{
            margin-bottom: 2rem;
        }}

        .section {{
            background-color: var(--background-color);
            padding: 1.5rem;
            border-radius: 0.5rem;
            box-shadow: 0 1px 3px rgba(0, 0, 0, 0.1);
            margin-bottom: 1.5rem;
        }}

        .section h2 {{
            color: var(--primary-color);
            font-size: 1.5rem;
            margin-bottom: 1.5rem;
            padding-bottom: 0.5rem;
            border-bottom: 2px solid #e2e8f0;
        }}

        .grid {{
            display: grid;
            gap: 1.5rem;
        }}

        .grid-cols-1 {{ grid-template-columns: repeat(1, 1fr); }}
        .grid-cols-2 {{ grid-template-columns: repeat(2, 1fr); }}
        .grid-cols-3 {{ grid-template-columns: repeat(3, 1fr); }}
        .grid-cols-4 {{ grid-template-columns: repeat(4, 1fr); }}

        @media (max-width: 768px) {{
            .grid {{ grid-template-columns: 1fr !important; }}
        }}

        .element {{
            background-color: #f8fafc;
            padding: 1rem;
            border-radius: 0.375rem;
            border: 1px solid #e2e8f0;
        }}

        .number-tile {{
            text-align: center;
            padding: 1.5rem;
        }}

        .tile-title {{
            font-size: 0.875rem;
            color: var(--secondary-color);
            text-transform: uppercase;
            letter-spacing: 0.05em;
            margin-bottom: 0.5rem;
        }}

        .tile-value {{
            font-size: 2.5rem;
            font-weight: 700;
            color: var(--primary-color);
            margin-bottom: 0.25rem;
        }}

        .tile-subtitle {{
            font-size: 0.875rem;
            color: var(--secondary-color);
        }}

        .free-text {{
            line-height: 1.6;
        }}

        .table-container {{
            overflow-x: auto;
        }}

        .table-container h3 {{
            font-size: 1.125rem;
            color: var(--text-color);
            margin-bottom: 1rem;
        }}

        table {{
            width: 100%;
            border-collapse: collapse;
            background-color: var(--background-color);
        }}

        th, td {{
            padding: 0.75rem;
            text-align: left;
            border-bottom: 1px solid #e2e8f0;
        }}

        th {{
            background-color: #f1f5f9;
            font-weight: 600;
            color: var(--text-color);
        }}

        tr:hover {{
            background-color: #f8fafc;
        }}

        .chart-container {{
            padding: 1rem;
        }}

        .chart-container h3 {{
            font-size: 1.125rem;
            color: var(--text-color);
            margin-bottom: 1rem;
        }}

        .chart-container canvas {{
            max-height: 300px;
        }}

        .report-footer {{
            background-color: var(--background-color);
            padding: 1.5rem;
            border-radius: 0.5rem;
            box-shadow: 0 1px 3px rgba(0, 0, 0, 0.1);
            text-align: center;
            color: var(--secondary-color);
            font-size: 0.875rem;
        }}

        .timestamp {{
            margin-top: 0.5rem;
            font-style: italic;
        }}
        ";
    }

    private string EscapeHtml(string text)
    {
        if (string.IsNullOrEmpty(text)) return text;
        return text
            .Replace("&", "&amp;")
            .Replace("<", "&lt;")
            .Replace(">", "&gt;")
            .Replace("\"", "&quot;")
            .Replace("'", "&#39;");
    }

    private string ToJsonArray(IEnumerable<string> items)
    {
        return "[" + string.Join(", ", items.Select(ToJsonString)) + "]";
    }

    private string ToJsonArray(IEnumerable<double> items)
    {
        return "[" + string.Join(", ", items) + "]";
    }

    private string ToJsonArray(IEnumerable<double?> items)
    {
        return "[" + string.Join(", ", items.Select(x => x?.ToString() ?? "null")) + "]";
    }

    private string ToJsonString(string text)
    {
        if (text == null) return "null";
        return "\"" + text
            .Replace("\\", "\\\\")
            .Replace("\"", "\\\"")
            .Replace("\n", "\\n")
            .Replace("\r", "\\r")
            .Replace("\t", "\\t") + "\"";
    }
}
