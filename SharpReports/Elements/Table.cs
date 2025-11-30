namespace SharpReports.Elements;

/// <summary>
/// Displays tabular data
/// </summary>
public class Table : ReportElementBase
{
    public override string ElementType => "Table";

    /// <summary>
    /// Gets the table title
    /// </summary>
    public string Title { get; }

    /// <summary>
    /// Gets the table data as rows
    /// </summary>
    public IEnumerable<Dictionary<string, object>> Rows { get; }

    /// <summary>
    /// Gets the column headers (in order)
    /// </summary>
    public List<string> Columns { get; }

    public Table(string title, IEnumerable<Dictionary<string, object>> rows)
    {
        Title = title ?? throw new ArgumentNullException(nameof(title));
        Rows = rows ?? throw new ArgumentNullException(nameof(rows));

        // Extract column names from first row
        var firstRow = rows.FirstOrDefault();
        Columns = firstRow?.Keys.ToList() ?? new List<string>();
    }

    public Table(string title, IEnumerable<Dictionary<string, object>> rows, List<string> columns)
    {
        Title = title ?? throw new ArgumentNullException(nameof(title));
        Rows = rows ?? throw new ArgumentNullException(nameof(rows));
        Columns = columns ?? throw new ArgumentNullException(nameof(columns));
    }

    /// <summary>
    /// Alternative constructor using column-based data
    /// </summary>
    public static Table FromColumns(string title, Dictionary<string, IEnumerable<object>> columns)
    {
        if (columns == null || !columns.Any())
            throw new ArgumentException("Columns cannot be null or empty", nameof(columns));

        var columnNames = columns.Keys.ToList();
        var rowCount = columns.First().Value.Count();

        // Convert column-based to row-based
        var rows = new List<Dictionary<string, object>>();
        for (int i = 0; i < rowCount; i++)
        {
            var row = new Dictionary<string, object>();
            foreach (var col in columnNames)
            {
                row[col] = columns[col].ElementAt(i);
            }
            rows.Add(row);
        }

        return new Table(title, rows, columnNames);
    }
}
