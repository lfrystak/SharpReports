namespace SharpReports.Rendering;

/// <summary>
/// Theme configuration for report styling
/// </summary>
public class Theme
{
    /// <summary>
    /// Primary color (hex format)
    /// </summary>
    public string PrimaryColor { get; set; } = "#2563eb";

    /// <summary>
    /// Secondary color (hex format)
    /// </summary>
    public string SecondaryColor { get; set; } = "#64748b";

    /// <summary>
    /// Background color (hex format)
    /// </summary>
    public string BackgroundColor { get; set; } = "#ffffff";

    /// <summary>
    /// Text color (hex format)
    /// </summary>
    public string TextColor { get; set; } = "#1e293b";

    /// <summary>
    /// Font family
    /// </summary>
    public string FontFamily { get; set; } = "-apple-system, BlinkMacSystemFont, 'Segoe UI', Roboto, 'Helvetica Neue', Arial, sans-serif";

    /// <summary>
    /// Default theme
    /// </summary>
    public static Theme Default => new();
}
