using SharpReports.Rendering;

namespace SharpReports.Core;

/// <summary>
/// Fluent API builder for creating reports
/// </summary>
public class ReportBuilder
{
    private readonly Report _report;

    public ReportBuilder(string title)
    {
        _report = new Report(title);
    }

    /// <summary>
    /// Sets the report title
    /// </summary>
    public static ReportBuilder WithTitle(string title)
    {
        return new ReportBuilder(title);
    }

    /// <summary>
    /// Adds a section to the report with a configuration action
    /// </summary>
    public ReportBuilder AddSection(string title, Action<ReportSection> configure)
    {
        var section = new ReportSection(title);
        configure?.Invoke(section);
        _report.Sections.Add(section);
        return this;
    }

    /// <summary>
    /// Adds a section to the report
    /// </summary>
    public ReportBuilder AddSection(ReportSection section)
    {
        _report.Sections.Add(section ?? throw new ArgumentNullException(nameof(section)));
        return this;
    }

    /// <summary>
    /// Sets the footer text
    /// </summary>
    public ReportBuilder WithFooter(string footer)
    {
        _report.Footer = footer;
        return this;
    }

    /// <summary>
    /// Sets the logo URL
    /// </summary>
    public ReportBuilder WithLogo(string logoUrl)
    {
        _report.LogoUrl = logoUrl;
        return this;
    }

    /// <summary>
    /// Builds and returns the report
    /// </summary>
    public Report Build()
    {
        return _report;
    }

    /// <summary>
    /// Generates HTML output for the report
    /// </summary>
    public string GenerateHtml(Theme? theme = null)
    {
        var renderer = new HtmlRenderer();
        return renderer.Render(_report, theme);
    }

    /// <summary>
    /// Generates HTML output asynchronously
    /// </summary>
    public Task<string> GenerateHtmlAsync(Theme? theme = null)
    {
        return Task.Run(() => GenerateHtml(theme));
    }

    /// <summary>
    /// Generates JSON output for the report
    /// </summary>
    public string GenerateJson()
    {
        var renderer = new JsonRenderer();
        return renderer.Render(_report);
    }

    /// <summary>
    /// Generates JSON output asynchronously
    /// </summary>
    public Task<string> GenerateJsonAsync()
    {
        return Task.Run(() => GenerateJson());
    }

    /// <summary>
    /// Saves the report as HTML to a file
    /// </summary>
    public async Task SaveHtmlAsync(string filePath, Theme? theme = null)
    {
        var html = await GenerateHtmlAsync(theme);
        await File.WriteAllTextAsync(filePath, html);
    }

    /// <summary>
    /// Saves the report as JSON to a file
    /// </summary>
    public async Task SaveJsonAsync(string filePath)
    {
        var json = await GenerateJsonAsync();
        await File.WriteAllTextAsync(filePath, json);
    }
}
