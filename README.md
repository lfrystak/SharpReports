# SharpReports

A lightweight, fluent C# library for generating beautiful HTML and JSON reports with charts, tables, and metrics.

## Features

- **Fluent API** - Intuitive, chainable methods for report building
- **Multiple Formats** - Generate HTML (with Chart.js) and JSON outputs
- **Rich Elements** - Number tiles, date tiles, charts, tables, and free text
- **Chart Types** - Bar, stacked bar, line, and pie charts
- **Responsive Layout** - Grid-based sections with configurable columns
- **Theming** - Customizable colors and fonts via CSS variables
- **Async Support** - Async methods for file I/O operations
- **Flexible Input** - Accepts IDictionary types and supports both int and double data
- **Zero Dependencies** - Minimal external dependencies (Chart.js via CDN for HTML)

## Installation

```bash
dotnet add package SharpReports
```

## Quick Start

```csharp
using SharpReports.Core;
using SharpReports.Extensions;

var report = ReportBuilder.WithTitle("Sales Report")
    .AddSection("Summary", section => section
        .SetColumns(3)
        .AddNumberTile("Revenue", 176000, "C0")
        .AddNumberTile("Orders", 1247)
        .AddNumberTile("Avg Order", 141.14, "C2"))
    .AddSection("Charts", section => section
        .SetColumns(2)
        .AddBarChart("Sales by Region", new Dictionary<string, double>
        {
            ["North"] = 45000,
            ["South"] = 38000,
            ["East"] = 52000
        })
        .AddPieChart("Market Share", new Dictionary<string, double>
        {
            ["Product A"] = 35,
            ["Product B"] = 28,
            ["Others"] = 37
        }))
    .WithFooter("Generated on " + DateTime.Now.ToString("yyyy-MM-dd"))
    .GenerateHtml();

await File.WriteAllTextAsync("report.html", report);
```

## Elements

### Number Tiles

Display key metrics with optional formatting and subtitles:

```csharp
section.AddNumberTile("Revenue", 176000, "C0")
section.AddNumberTile("Growth Rate", 0.15, "P1", "↑ vs last quarter")
```

### Date Tiles

Display dates with optional formatting and subtitles:

```csharp
section.AddDateTile("Launch Date", new DateTime(2024, 6, 15), "yyyy-MM-dd")
section.AddDateTile("Deadline", DateOnly.FromDateTime(DateTime.Now.AddDays(30)), "dd MMM yyyy", "30 days from now")
```

Supports both `DateTime` and `DateOnly` types with standard .NET date format strings.

### Charts

**Bar Chart**
```csharp
// Works with double values
section.AddBarChart("Sales by Region", new Dictionary<string, double>
{
    ["North"] = 45000,
    ["South"] = 38000
}, isHorizontal: false)

// Also works with integers
section.AddBarChart("Order Counts", new Dictionary<string, int>
{
    ["North"] = 450,
    ["South"] = 380
})
```

All chart methods accept `IDictionary<,>` types, allowing flexibility with Dictionary, SortedDictionary, or custom implementations.

**Stacked Bar Chart**
```csharp
// Multi-series with double values
section.AddStackedBarChart("Product Sales", new Dictionary<string, Dictionary<string, double>>
{
    ["Q1"] = new() { ["Product A"] = 12000, ["Product B"] = 8000 },
    ["Q2"] = new() { ["Product A"] = 15000, ["Product B"] = 9000 }
})

// Also supports integer data
section.AddStackedBarChart("Task Status", new Dictionary<string, Dictionary<string, int>>
{
    ["Sprint 1"] = new() { ["Completed"] = 15, ["In Progress"] = 8 },
    ["Sprint 2"] = new() { ["Completed"] = 20, ["In Progress"] = 5 }
})

// Simple single-series version
section.AddStackedBarChart("Categories", new Dictionary<string, int>
{
    ["Category A"] = 100,
    ["Category B"] = 200
})
```

**Line Chart**
```csharp
section.AddLineChart("Trend", new Dictionary<string, double>
{
    ["Jan"] = 15000,
    ["Feb"] = 18000,
    ["Mar"] = 22000
}, showPoints: true)
```

**Pie Chart**
```csharp
// With double values
section.AddPieChart("Distribution", new Dictionary<string, double>
{
    ["Category A"] = 35.5,
    ["Category B"] = 28.3
}, isDonut: true)

// With integer values
section.AddPieChart("Votes", new Dictionary<string, int>
{
    ["Option A"] = 42,
    ["Option B"] = 38
})
```

### Tables

**From Rows**
```csharp
var rows = new List<Dictionary<string, object>>
{
    new() { ["Name"] = "Alice", ["Sales"] = 125000 },
    new() { ["Name"] = "Bob", ["Sales"] = 98000 }
};
section.AddTable("Top Performers", rows)
```

**From Columns**
```csharp
section.AddTableFromColumns("Data", new Dictionary<string, IEnumerable<object>>
{
    ["Name"] = new[] { "Alice", "Bob" },
    ["Sales"] = new object[] { 125000, 98000 }
})
```

### Free Text

```csharp
section.AddText("This is a plain text description")
section.AddText("<p>This is <strong>HTML</strong> content</p>", isHtml: true)
```

## Layout

Control section layouts with columns:

```csharp
.AddSection("Dashboard", section => section
    .SetColumns(3)  // 3-column grid layout
    .AddNumberTile("Metric 1", 100)
    .AddNumberTile("Metric 2", 200)
    .AddNumberTile("Metric 3", 300))
```

Elements flow left-to-right, wrapping to new rows automatically.

## Output Formats

### HTML
```csharp
// Generate HTML string
var html = report.GenerateHtml();

// Generate with custom theme
var theme = new Theme
{
    PrimaryColor = "#10b981",
    SecondaryColor = "#6366f1"
};
var html = report.GenerateHtml(theme);

// Save to file
await report.SaveHtmlAsync("report.html", theme);
```

### JSON
```csharp
// Generate JSON string
var json = report.GenerateJson();

// Save to file
await report.SaveJsonAsync("report.json");
```

## Theming

Customize report appearance:

```csharp
var theme = new Theme
{
    PrimaryColor = "#2563eb",      // Headers, highlights
    SecondaryColor = "#64748b",    // Subtitles, secondary text
    BackgroundColor = "#ffffff",   // Card backgrounds
    TextColor = "#1e293b",         // Body text
    FontFamily = "Arial, sans-serif"
};

var html = report.GenerateHtml(theme);
```

## Advanced Usage

### Custom Logo
```csharp
var report = ReportBuilder.WithTitle("Report")
    .WithLogo("https://example.com/logo.png")
    .AddSection(...)
    .Build();
```

### Async Generation
```csharp
var html = await report.GenerateHtmlAsync();
var json = await report.GenerateJsonAsync();
```

### Building Reports Programmatically
```csharp
var builder = ReportBuilder.WithTitle("Dynamic Report");

foreach (var region in regions)
{
    builder.AddSection(region.Name, s =>
        s.AddBarChart("Sales", region.SalesData));
}

var report = builder.Build();
```

## Architecture

SharpReports follows SOLID principles:

- **Core**: `Report`, `ReportSection`, `ReportBuilder`, `IReportElement`
- **Elements**: `NumberTile`, `FreeText`, `Table`, chart types
- **Rendering**: `IRenderer`, `HtmlRenderer`, `JsonRenderer`
- **Extensions**: Fluent extension methods for `ReportSection`

### Extensibility

Add custom elements by implementing `IReportElement`:

```csharp
public class CustomElement : ReportElementBase
{
    public override string ElementType => "CustomElement";
    // Add your properties and logic
}
```

Then extend the renderers to handle your custom type.

## Examples

See the [Examples](./Examples) directory for:
- `BasicExample.cs` - Complete sample report with all element types
- More examples coming soon

## Requirements

- .NET 10.0 or later
- Chart.js (loaded via CDN in HTML output)

## License

To be determined

## Contributing

Contributions welcome! Please open issues or submit pull requests.

## Documentation

### XML Documentation
All public APIs include XML documentation for IntelliSense support.

### Sample Output
Run the examples to see generated HTML and JSON reports.

## Roadmap

Future enhancements may include:
- Additional chart types (scatter, radar, etc.)
- More output formats (Markdown, PDF)
- Enhanced table features (sorting, filtering)
- Conditional formatting
- Data export functionality
