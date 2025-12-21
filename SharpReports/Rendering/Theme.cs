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
    /// Accent color for highlights and call-to-actions (hex format)
    /// </summary>
    public string AccentColor { get; set; } = "#10b981";

    /// <summary>
    /// Border radius for elements (CSS value, e.g., "0.5rem", "8px")
    /// </summary>
    public string BorderRadius { get; set; } = "0.5rem";

    /// <summary>
    /// Chart color palette (array of hex colors). If empty, uses default palette.
    /// </summary>
    public string[] ChartColors { get; set; } = Array.Empty<string>();

    /// <summary>
    /// Shadow intensity level: "none", "subtle", "medium", "strong"
    /// </summary>
    public string ShadowIntensity { get; set; } = "medium";

    /// <summary>
    /// Enable smooth transitions and animations
    /// </summary>
    public bool EnableAnimations { get; set; } = true;

    /// <summary>
    /// Enable gradient effects on tiles and headers
    /// </summary>
    public bool EnableGradients { get; set; }

    /// <summary>
    /// Default theme
    /// </summary>
    public static Theme Default => new();
}
