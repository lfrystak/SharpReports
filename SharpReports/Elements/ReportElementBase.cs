using SharpReports.Core;

namespace SharpReports.Elements;

/// <summary>
/// Base class for report elements providing common functionality
/// </summary>
public abstract class ReportElementBase : IReportElement
{
    /// <inheritdoc/>
    public string Id { get; }

    /// <inheritdoc/>
    public abstract string ElementType { get; }

    protected ReportElementBase()
    {
        Id = Guid.NewGuid().ToString("N");
    }

    protected ReportElementBase(string id)
    {
        Id = id ?? throw new ArgumentNullException(nameof(id));
    }
}
