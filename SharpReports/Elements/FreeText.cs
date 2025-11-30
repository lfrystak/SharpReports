namespace SharpReports.Elements;

/// <summary>
/// Displays free-form text content
/// </summary>
public class FreeText : ReportElementBase
{
    public override string ElementType => "FreeText";

    /// <summary>
    /// Gets the text content
    /// </summary>
    public string Content { get; }

    /// <summary>
    /// Gets whether the content is HTML (default: false, treated as plain text)
    /// </summary>
    public bool IsHtml { get; }

    public FreeText(string content, bool isHtml = false)
    {
        Content = content ?? throw new ArgumentNullException(nameof(content));
        IsHtml = isHtml;
    }
}
