using SharpReports.Core;

namespace SharpReports.Rendering;

/// <summary>
/// Interface for report renderers
/// </summary>
public interface IRenderer
{
    /// <summary>
    /// Renders a report to a string output
    /// </summary>
    string Render(Report report, Theme? theme = null);
}
