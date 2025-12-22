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
    private Theme? _currentTheme;

    public string Render(Report report, Theme? theme = null)
    {
        theme ??= Theme.Default;
        _currentTheme = theme;
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
        sb.AppendLine($"        <h1>{EscapeHtml(report.Title)}</h1>");
        if (!string.IsNullOrEmpty(report.LogoUrl))
        {
            sb.AppendLine($"        <img src=\"{EscapeHtml(report.LogoUrl)}\" alt=\"Logo\" class=\"logo\">");
        }
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

        // Use custom column widths if specified, otherwise use equal-width grid
        if (section.ColumnWidths != null && section.ColumnWidths.Count > 0)
        {
            // Calculate percentages from unit values
            var totalUnits = section.ColumnWidths.Sum();
            var percentages = section.ColumnWidths.Select(w => (double)w / totalUnits * 100);
            var gridTemplate = string.Join(" ", percentages.Select(p => p.ToString("F2", System.Globalization.CultureInfo.InvariantCulture) + "%"));
            sb.AppendLine($"        <div class=\"grid\" style=\"grid-template-columns: {gridTemplate};\">");
        }
        else
        {
            sb.AppendLine($"        <div class=\"grid grid-cols-{section.Columns}\">");
        }

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
            case Canvas canvas:
                RenderCanvas(sb, canvas);
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
        if (!string.IsNullOrEmpty(tile.Tooltip))
        {
            sb.AppendLine("                    <div class=\"tile-tooltip\">");
            sb.AppendLine("                        <span class=\"tooltip-icon\">i</span>");
            sb.AppendLine($"                        <div class=\"tooltip-content\">{EscapeHtml(tile.Tooltip)}</div>");
            sb.AppendLine("                    </div>");
        }
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
        if (!string.IsNullOrEmpty(tile.Tooltip))
        {
            sb.AppendLine("                    <div class=\"tile-tooltip\">");
            sb.AppendLine("                        <span class=\"tooltip-icon\">i</span>");
            sb.AppendLine($"                        <div class=\"tooltip-content\">{EscapeHtml(tile.Tooltip)}</div>");
            sb.AppendLine("                    </div>");
        }
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
        var colors = GetChartColors();
        var primaryColor = colors[0];
        var borderColor = primaryColor.Replace("0.7)", "1)");

        sb.AppendLine($"                <div class=\"chart-container\">");
        if (!string.IsNullOrEmpty(chart.Tooltip))
        {
            sb.AppendLine("                    <div class=\"chart-tooltip\">");
            sb.AppendLine("                        <span class=\"tooltip-icon\">i</span>");
            sb.AppendLine($"                        <div class=\"tooltip-content\">{EscapeHtml(chart.Tooltip)}</div>");
            sb.AppendLine("                    </div>");
        }
        sb.AppendLine($"                    <h3>{EscapeHtml(chart.Title)}</h3>");
        sb.AppendLine($"                    <canvas id=\"chart-{chartId}\"></canvas>");
        sb.AppendLine("                </div>");
        sb.AppendLine("                <script>");
        sb.AppendLine($"                    new Chart(document.getElementById('chart-{chartId}'), {{");
        sb.AppendLine($"                        type: 'bar',");
        sb.AppendLine($"                        data: {{");
        sb.AppendLine($"                            labels: {ToJsonArray(chart.Data.Keys)},");
        sb.AppendLine($"                            datasets: [{{");
        sb.AppendLine($"                                data: {ToJsonArray(chart.Data.Values)},");
        sb.AppendLine($"                                backgroundColor: '{primaryColor}',");
        sb.AppendLine($"                                borderColor: '{borderColor}',");
        sb.AppendLine($"                                borderWidth: 1,");
        sb.AppendLine($"                                borderRadius: 4");
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
        var colors = GetChartColors();

        sb.AppendLine($"                <div class=\"chart-container\">");
        if (!string.IsNullOrEmpty(chart.Tooltip))
        {
            sb.AppendLine("                    <div class=\"chart-tooltip\">");
            sb.AppendLine("                        <span class=\"tooltip-icon\">i</span>");
            sb.AppendLine($"                        <div class=\"tooltip-content\">{EscapeHtml(chart.Tooltip)}</div>");
            sb.AppendLine("                    </div>");
        }
        sb.AppendLine($"                    <h3>{EscapeHtml(chart.Title)}</h3>");
        sb.AppendLine($"                    <canvas id=\"chart-{chartId}\"></canvas>");
        sb.AppendLine("                </div>");
        sb.AppendLine("                <script>");
        sb.AppendLine($"                    new Chart(document.getElementById('chart-{chartId}'), {{");
        sb.AppendLine($"                        type: 'bar',");
        sb.AppendLine($"                        data: {{");
        sb.AppendLine($"                            labels: {ToJsonArray(categories)},");
        sb.AppendLine($"                            datasets: [");

        for (int i = 0; i < allSeries.Count; i++)
        {
            var series = allSeries[i];
            var data = categories.Select(cat =>
                chart.Data[cat].ContainsKey(series) ? chart.Data[cat][series] : 0
            );

            sb.AppendLine($"                                {{");
            sb.AppendLine($"                                    label: {ToJsonString(series)},");
            sb.AppendLine($"                                    data: {ToJsonArray(data)},");
            sb.AppendLine($"                                    backgroundColor: '{colors[i % colors.Length]}',");
            sb.AppendLine($"                                    borderRadius: 4");
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
        var colors = GetChartColors();

        sb.AppendLine($"                <div class=\"chart-container\">");
        if (!string.IsNullOrEmpty(chart.Tooltip))
        {
            sb.AppendLine("                    <div class=\"chart-tooltip\">");
            sb.AppendLine("                        <span class=\"tooltip-icon\">i</span>");
            sb.AppendLine($"                        <div class=\"tooltip-content\">{EscapeHtml(chart.Tooltip)}</div>");
            sb.AppendLine("                    </div>");
        }
        sb.AppendLine($"                    <h3>{EscapeHtml(chart.Title)}</h3>");
        sb.AppendLine($"                    <canvas id=\"chart-{chartId}\"></canvas>");
        sb.AppendLine("                </div>");
        sb.AppendLine("                <script>");
        sb.AppendLine($"                    new Chart(document.getElementById('chart-{chartId}'), {{");
        sb.AppendLine($"                        type: 'line',");
        sb.AppendLine($"                        data: {{");
        sb.AppendLine($"                            labels: {ToJsonArray(allLabels)},");
        sb.AppendLine($"                            datasets: [");

        var seriesIndex = 0;
        foreach (var series in chart.Series)
        {
            var data = allLabels.Select(label =>
                series.Value.ContainsKey(label) ? series.Value[label] : (double?)null
            );

            var borderColor = colors[seriesIndex % colors.Length].Replace("0.7)", "1)");
            var bgColor = colors[seriesIndex % colors.Length].Replace("0.7)", "0.1)");

            sb.AppendLine($"                                {{");
            sb.AppendLine($"                                    label: {ToJsonString(series.Key)},");
            sb.AppendLine($"                                    data: {ToJsonArray(data)},");
            sb.AppendLine($"                                    borderColor: '{borderColor}',");
            sb.AppendLine($"                                    backgroundColor: '{bgColor}',");
            sb.AppendLine($"                                    tension: 0.3,");
            sb.AppendLine($"                                    pointRadius: {(chart.ShowPoints ? "4" : "0")},");
            sb.AppendLine($"                                    pointHoverRadius: 6,");
            sb.AppendLine($"                                    borderWidth: 2");
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
        var colors = GetChartColors();

        sb.AppendLine($"                <div class=\"chart-container\">");
        if (!string.IsNullOrEmpty(chart.Tooltip))
        {
            sb.AppendLine("                    <div class=\"chart-tooltip\">");
            sb.AppendLine("                        <span class=\"tooltip-icon\">i</span>");
            sb.AppendLine($"                        <div class=\"tooltip-content\">{EscapeHtml(chart.Tooltip)}</div>");
            sb.AppendLine("                    </div>");
        }
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

        for (int i = 0; i < colors.Length; i++)
        {
            sb.AppendLine($"                                    '{colors[i]}'{(i < colors.Length - 1 ? "," : "")}");
        }

        sb.AppendLine($"                                ],");
        sb.AppendLine($"                                borderWidth: 2,");
        sb.AppendLine($"                                borderColor: '#ffffff'");
        sb.AppendLine($"                            }}]");
        sb.AppendLine($"                        }},");
        sb.AppendLine($"                        options: {{");
        sb.AppendLine($"                            responsive: true,");
        sb.AppendLine($"                            maintainAspectRatio: true");
        sb.AppendLine($"                        }}");
        sb.AppendLine($"                    }});");
        sb.AppendLine("                </script>");
    }

    private void RenderCanvas(StringBuilder sb, Canvas canvas)
    {
        // Render canvas container - always fills 100% of its section column
        sb.AppendLine($"                <div class=\"canvas-container\">");
        sb.AppendLine($"                    <div class=\"canvas-grid grid-cols-{canvas.Columns}\">");

        // Render each element in the canvas
        foreach (var element in canvas.Elements)
        {
            sb.AppendLine("                        <div class=\"canvas-element element\">");
            RenderCanvasElement(sb, element);
            sb.AppendLine("                        </div>");
        }

        sb.AppendLine("                    </div>");
        sb.AppendLine("                </div>");
    }

    private void RenderCanvasElement(StringBuilder sb, IReportElement element)
    {
        // Render the element content without the wrapping div
        // This is similar to RenderElement but without the outer element div
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
                sb.AppendLine($"                        <p>Unknown element type: {element.ElementType}</p>");
                break;
        }
    }

    private string GenerateCss(Theme theme)
    {
        var shadowStyle = GetShadowStyle(theme.ShadowIntensity);
        var transitionStyle = theme.EnableAnimations ? "transition: all 0.2s ease-in-out;" : "";
        var gradientOverlay = theme.EnableGradients
            ? "background: linear-gradient(135deg, var(--background-color) 0%, rgba(255,255,255,0.95) 100%);"
            : "";

        return $@"
        :root {{
            --primary-color: {theme.PrimaryColor};
            --secondary-color: {theme.SecondaryColor};
            --background-color: {theme.BackgroundColor};
            --text-color: {theme.TextColor};
            --accent-color: {theme.AccentColor};
            --font-family: {theme.FontFamily};
            --border-radius: {theme.BorderRadius};
            --border-radius-sm: calc({theme.BorderRadius} * 0.75);
            --border-radius-lg: calc({theme.BorderRadius} * 1.5);
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
            line-height: 1.6;
        }}

        @media (max-width: 640px) {{
            body {{
                padding: 1rem;
            }}
        }}

        .report-header {{
            background-color: var(--background-color);
            {gradientOverlay}
            padding: 2rem;
            border-radius: var(--border-radius-lg);
            {shadowStyle}
            margin-bottom: 2rem;
            display: flex;
            justify-content: space-between;
            align-items: center;
            {transitionStyle}
        }}

        .report-header h1 {{
            color: var(--primary-color);
            font-size: clamp(1.5rem, 4vw, 2.25rem);
            font-weight: 700;
            letter-spacing: -0.025em;
        }}

        .logo {{
            max-height: 60px;
            max-width: 200px;
            {transitionStyle}
        }}

        .report-content {{
            margin-bottom: 2rem;
        }}

        .section {{
            background-color: var(--background-color);
            padding: 1.75rem;
            border-radius: var(--border-radius-lg);
            {shadowStyle}
            margin-bottom: 1.75rem;
            {transitionStyle}
        }}

        .section:hover {{
            {(theme.EnableAnimations ? GetHoverShadowStyle(theme.ShadowIntensity) : shadowStyle)}
        }}

        .section h2 {{
            color: var(--primary-color);
            font-size: clamp(1.25rem, 3vw, 1.75rem);
            font-weight: 600;
            margin-bottom: 1.75rem;
            padding-bottom: 0.75rem;
            border-bottom: 3px solid;
            border-image: linear-gradient(90deg, var(--primary-color) 0%, transparent 100%) 1;
            letter-spacing: -0.015em;
        }}

        .grid {{
            display: grid;
            gap: 1.5rem;
        }}

        .grid-cols-1 {{ grid-template-columns: repeat(1, 1fr); }}
        .grid-cols-2 {{ grid-template-columns: repeat(2, 1fr); }}
        .grid-cols-3 {{ grid-template-columns: repeat(3, 1fr); }}
        .grid-cols-4 {{ grid-template-columns: repeat(4, 1fr); }}

        .canvas-container {{
            width: 100%;
            {transitionStyle}
        }}

        .canvas-grid {{
            display: grid;
            gap: 1.5rem;
        }}

        .canvas-grid.grid-cols-1 {{ grid-template-columns: repeat(1, 1fr); }}
        .canvas-grid.grid-cols-2 {{ grid-template-columns: repeat(2, 1fr); }}
        .canvas-grid.grid-cols-3 {{ grid-template-columns: repeat(3, 1fr); }}
        .canvas-grid.grid-cols-4 {{ grid-template-columns: repeat(4, 1fr); }}

        .canvas-element {{
            background: linear-gradient(to bottom, #ffffff 0%, #f8fafc 100%);
            padding: 1.25rem;
            border-radius: var(--border-radius);
            border: 1px solid #e2e8f0;
            {transitionStyle}
            overflow: hidden;
        }}

        .canvas-element:hover {{
            border-color: #cbd5e1;
            {(theme.EnableAnimations ? "transform: translateY(-2px); box-shadow: 0 4px 12px rgba(0, 0, 0, 0.08);" : "")}
        }}

        @media (max-width: 1280px) {{
            .grid-cols-4 {{ grid-template-columns: repeat(3, 1fr); }}
            .canvas-grid.grid-cols-4 {{ grid-template-columns: repeat(3, 1fr); }}
        }}

        @media (max-width: 1024px) {{
            .grid-cols-4 {{ grid-template-columns: repeat(2, 1fr); }}
            .grid-cols-3 {{ grid-template-columns: repeat(2, 1fr); }}
            .canvas-grid.grid-cols-4 {{ grid-template-columns: repeat(2, 1fr); }}
            .canvas-grid.grid-cols-3 {{ grid-template-columns: repeat(2, 1fr); }}
        }}

        @media (max-width: 768px) {{
            .grid {{ grid-template-columns: 1fr !important; }}
            .canvas-grid {{ grid-template-columns: 1fr !important; }}
            .canvas-container {{ width: 100% !important; }}
        }}

        .element {{
            background: linear-gradient(to bottom, #ffffff 0%, #f8fafc 100%);
            padding: 1.25rem;
            border-radius: var(--border-radius);
            border: 1px solid #e2e8f0;
            {transitionStyle}
            overflow: hidden;
        }}

        .element:hover {{
            border-color: #cbd5e1;
            {(theme.EnableAnimations ? "transform: translateY(-2px); box-shadow: 0 4px 12px rgba(0, 0, 0, 0.08);" : "")}
        }}

        .number-tile {{
            text-align: center;
            padding: 1.75rem 1.25rem;
            position: relative;
        }}

        {(theme.EnableGradients ? @"
        .number-tile::before {{
            content: '';
            position: absolute;
            top: 0;
            left: 0;
            right: 0;
            height: 4px;
            background: linear-gradient(90deg, var(--primary-color), var(--accent-color));
            border-radius: var(--border-radius) var(--border-radius) 0 0;
        }}" : "")}

        .tile-title {{
            font-size: 0.8125rem;
            font-weight: 600;
            color: var(--secondary-color);
            text-transform: uppercase;
            letter-spacing: 0.075em;
            margin-bottom: 0.75rem;
        }}

        .tile-value {{
            font-size: clamp(2rem, 5vw, 2.75rem);
            font-weight: 700;
            color: var(--primary-color);
            margin-bottom: 0.5rem;
            line-height: 1.1;
            letter-spacing: -0.02em;
        }}

        .tile-subtitle {{
            font-size: 0.875rem;
            font-weight: 500;
            color: var(--secondary-color);
            margin-top: 0.5rem;
        }}

        .tile-tooltip {{
            position: absolute;
            top: -0.75rem;
            right: -0.5rem;
            z-index: 10;
        }}

        .tooltip-icon {{
            display: inline-flex;
            align-items: center;
            justify-content: center;
            width: 1.25rem;
            height: 1.25rem;
            border-radius: 50%;
            background-color: var(--secondary-color);
            color: var(--background-color);
            font-size: 0.75rem;
            font-weight: 600;
            font-style: normal;
            cursor: help;
            {transitionStyle}
        }}

        .tooltip-icon:hover {{
            background-color: var(--primary-color);
            transform: scale(1.1);
        }}

        .tooltip-content {{
            position: absolute;
            top: calc(100% + 0.5rem);
            right: 0;
            min-width: 200px;
            max-width: 300px;
            padding: 0.75rem;
            background-color: var(--text-color);
            color: var(--background-color);
            font-size: 0.8125rem;
            font-weight: 400;
            line-height: 1.5;
            border-radius: var(--border-radius-sm);
            box-shadow: 0 4px 12px rgba(0, 0, 0, 0.15);
            opacity: 0;
            visibility: hidden;
            {transitionStyle}
            pointer-events: none;
            white-space: normal;
            text-align: left;
        }}

        .tooltip-content::before {{
            content: '';
            position: absolute;
            bottom: 100%;
            right: 0.5rem;
            width: 0;
            height: 0;
            border-left: 6px solid transparent;
            border-right: 6px solid transparent;
            border-bottom: 6px solid var(--text-color);
        }}

        .tooltip-icon:hover + .tooltip-content {{
            opacity: 1;
            visibility: visible;
        }}

        .free-text {{
            line-height: 1.7;
            color: var(--text-color);
        }}

        .free-text p {{
            margin-bottom: 1rem;
        }}

        .free-text p:last-child {{
            margin-bottom: 0;
        }}

        .table-container {{
            overflow-x: auto;
            border-radius: var(--border-radius-sm);
        }}

        .table-container h3 {{
            font-size: 1.125rem;
            font-weight: 600;
            color: var(--text-color);
            margin-bottom: 1.25rem;
        }}

        table {{
            width: 100%;
            border-collapse: collapse;
            background-color: var(--background-color);
            font-size: 0.9375rem;
        }}

        th, td {{
            padding: 0.875rem 1rem;
            text-align: left;
            border-bottom: 1px solid #e2e8f0;
        }}

        th {{
            background: linear-gradient(to bottom, #f8fafc 0%, #f1f5f9 100%);
            font-weight: 600;
            color: var(--text-color);
            text-transform: uppercase;
            font-size: 0.8125rem;
            letter-spacing: 0.05em;
            border-bottom: 2px solid #cbd5e1;
        }}

        tbody tr {{
            {transitionStyle}
        }}

        tbody tr:nth-child(even) {{
            background-color: #fafbfc;
        }}

        tbody tr:hover {{
            background-color: #f0f9ff;
            {(theme.EnableAnimations ? "transform: scale(1.005);" : "")}
        }}

        tbody tr:last-child td {{
            border-bottom: none;
        }}

        .chart-container {{
            padding: 1.25rem;
            position: relative;
        }}

        .chart-container h3 {{
            font-size: 1.125rem;
            font-weight: 600;
            color: var(--text-color);
            margin-bottom: 1.5rem;
            padding-bottom: 0.5rem;
            border-bottom: 2px solid #f1f5f9;
        }}

        .chart-container canvas {{
            max-height: 350px;
            {transitionStyle}
        }}

        .chart-tooltip {{
            position: absolute;
            top: -0.75rem;
            right: -0.5rem;
            z-index: 10;
        }}

        .chart-tooltip .tooltip-icon {{
            display: inline-flex;
            align-items: center;
            justify-content: center;
            width: 1.25rem;
            height: 1.25rem;
            border-radius: 50%;
            background-color: var(--secondary-color);
            color: var(--background-color);
            font-size: 0.75rem;
            font-weight: 600;
            font-style: normal;
            cursor: help;
            {transitionStyle}
        }}

        .chart-tooltip .tooltip-icon:hover {{
            background-color: var(--primary-color);
            transform: scale(1.1);
        }}

        .chart-tooltip .tooltip-content {{
            position: absolute;
            top: calc(100% + 0.5rem);
            right: 0;
            min-width: 200px;
            max-width: 300px;
            padding: 0.75rem;
            background-color: var(--text-color);
            color: var(--background-color);
            font-size: 0.8125rem;
            font-weight: 400;
            line-height: 1.5;
            border-radius: var(--border-radius-sm);
            box-shadow: 0 4px 12px rgba(0, 0, 0, 0.15);
            opacity: 0;
            visibility: hidden;
            {transitionStyle}
            pointer-events: none;
            white-space: normal;
            text-align: left;
        }}

        .chart-tooltip .tooltip-content::before {{
            content: '';
            position: absolute;
            bottom: 100%;
            right: 0.5rem;
            width: 0;
            height: 0;
            border-left: 6px solid transparent;
            border-right: 6px solid transparent;
            border-bottom: 6px solid var(--text-color);
        }}

        .chart-tooltip .tooltip-icon:hover + .tooltip-content {{
            opacity: 1;
            visibility: visible;
        }}

        .report-footer {{
            background-color: var(--background-color);
            padding: 1.75rem;
            border-radius: var(--border-radius-lg);
            {shadowStyle}
            text-align: center;
            color: var(--secondary-color);
            font-size: 0.875rem;
            {transitionStyle}
        }}

        .report-footer p {{
            margin-bottom: 0.5rem;
        }}

        .report-footer p:last-child {{
            margin-bottom: 0;
        }}

        .timestamp {{
            margin-top: 0.5rem;
            font-style: italic;
            opacity: 0.8;
        }}

        @media print {{
            body {{
                padding: 0;
                background-color: white;
            }}

            .section, .report-header, .report-footer {{
                box-shadow: none;
                page-break-inside: avoid;
            }}
        }}
        ";
    }

    private string GetShadowStyle(string intensity)
    {
        return intensity switch
        {
            "none" => "box-shadow: none;",
            "subtle" => "box-shadow: 0 1px 2px rgba(0, 0, 0, 0.05);",
            "medium" => "box-shadow: 0 1px 3px rgba(0, 0, 0, 0.1), 0 1px 2px rgba(0, 0, 0, 0.06);",
            "strong" => "box-shadow: 0 4px 6px rgba(0, 0, 0, 0.1), 0 2px 4px rgba(0, 0, 0, 0.06);",
            _ => "box-shadow: 0 1px 3px rgba(0, 0, 0, 0.1), 0 1px 2px rgba(0, 0, 0, 0.06);"
        };
    }

    private string GetHoverShadowStyle(string intensity)
    {
        return intensity switch
        {
            "none" => "box-shadow: none;",
            "subtle" => "box-shadow: 0 2px 4px rgba(0, 0, 0, 0.08);",
            "medium" => "box-shadow: 0 4px 6px rgba(0, 0, 0, 0.12), 0 2px 4px rgba(0, 0, 0, 0.08);",
            "strong" => "box-shadow: 0 10px 15px rgba(0, 0, 0, 0.15), 0 4px 6px rgba(0, 0, 0, 0.1);",
            _ => "box-shadow: 0 4px 6px rgba(0, 0, 0, 0.12), 0 2px 4px rgba(0, 0, 0, 0.08);"
        };
    }

    private string[] GetChartColors()
    {
        if (_currentTheme?.ChartColors != null && _currentTheme.ChartColors.Length > 0)
        {
            return _currentTheme.ChartColors;
        }

        // Default color palette - modern and accessible
        return new[]
        {
            "rgba(37, 99, 235, 0.7)",   // Blue
            "rgba(100, 116, 139, 0.7)",  // Slate
            "rgba(239, 68, 68, 0.7)",    // Red
            "rgba(34, 197, 94, 0.7)",    // Green
            "rgba(251, 191, 36, 0.7)",   // Amber
            "rgba(168, 85, 247, 0.7)"    // Purple
        };
    }

    private string HexToRgba(string hex, double opacity = 0.7)
    {
        hex = hex.TrimStart('#');
        if (hex.Length == 6)
        {
            var r = Convert.ToInt32(hex.Substring(0, 2), 16);
            var g = Convert.ToInt32(hex.Substring(2, 2), 16);
            var b = Convert.ToInt32(hex.Substring(4, 2), 16);
            return $"rgba({r}, {g}, {b}, {opacity})";
        }
        return $"rgba(37, 99, 235, {opacity})"; // Fallback
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
