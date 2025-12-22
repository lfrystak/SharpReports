# SharpReports

A lightweight .NET library for generating beautiful HTML and JSON reports with charts, tables, and data tiles.

## Features

- Multiple output formats (HTML, JSON)
- Interactive charts powered by Chart.js (Bar, Line, Pie, Stacked Bar)
- **Canvas element** for flexible mixed layouts
- **Custom column widths** using intuitive unit-based ratios
- Data tables and metric tiles
- **Tooltips** for tiles and charts
- Date and number formatting
- Customizable themes with modern design
- Fluent API design
- Zero dependencies (Chart.js loaded via CDN for HTML output)
- .NET 10.0 target framework

## Installation

```bash
dotnet add package SharpReports
```

## Quick Start

```csharp
using SharpReports.Core;
using SharpReports.Extensions;

var salesData = new Dictionary<string, double>
{
    ["Q1"] = 45000,
    ["Q2"] = 52000,
    ["Q3"] = 48000,
    ["Q4"] = 61000
};

var report = ReportBuilder.WithTitle("Sales Report 2024")
    .AddSection("Key Metrics", section => section
        .SetColumns(4)
        .AddNumberTile("Total Revenue", 206000, "C0")
        .AddNumberTile("Orders", 1247, "N0")
        .AddNumberTile("Avg Order", 165.20, "C2")
        .AddNumberTile("Growth", 0.23, "P1", "↑ vs 2023"))

    .AddSection("Quarterly Performance", section => section
        .SetColumns(2)
        .AddBarChart("Revenue by Quarter", salesData)
        .AddPieChart("Distribution", salesData, isDonut: true))

    .WithFooter("Generated with SharpReports")
    .Build();

// Generate outputs
await report.SaveHtmlAsync("report.html");
await report.SaveJsonAsync("report.json");
```

## Output Formats

### HTML
Interactive reports with Chart.js visualizations, responsive design, and custom theming.

### JSON
Structured data output for API integration and data processing.

## Key Components

### Data Tiles
Display key metrics with formatting and optional tooltips:
```csharp
.AddNumberTile("Revenue", 50000, "C0")        // Currency
.AddNumberTile("Users", 1234, "N0")           // Number
.AddNumberTile("Rate", 0.156, "P1")           // Percentage
.AddNumberTile("Score", 85.5, "N1", "Points", "Calculated from user ratings")  // With tooltip
.AddDateTile("Launch Date", DateTime.Now, "yyyy-MM-dd")
```

### Charts
Multiple chart types with flexible data input and optional tooltips:
```csharp
// Supports Dictionary, SortedDictionary, or any IDictionary
.AddBarChart("Sales", data)
.AddBarChart("Revenue", data, tooltip: "Hover for details")
.AddLineChart("Trend", data)
.AddPieChart("Distribution", data, isDonut: true)
.AddStackedBarChart("Comparison", nestedData, tooltip: "Breakdown by category")
```

### Tables
Structured data display:
```csharp
var tableData = new List<Dictionary<string, object>>
{
    new() { ["Name"] = "Alice", ["Sales"] = 125000 },
    new() { ["Name"] = "Bob", ["Sales"] = 98000 }
};

.AddTable("Top Performers", tableData)
```

### Canvas - Flexible Layouts
Mix tiles and charts in custom layouts with flexible column widths:
```csharp
// Create a section with custom column widths (1:2 ratio = 33%/67%)
.AddSection("Mixed Layout", section => section
    .SetColumnWidths(1, 2)  // First column 33%, second column 67%
    .AddCanvas(2, canvas => canvas  // Canvas with 2 internal columns
        .AddNumberTile("Metric 1", 100, "N0")
        .AddBarChart("Chart 1", data)
        .AddNumberTile("Metric 2", 200, "N0")
        .AddPieChart("Chart 2", pieData))
    .AddLineChart("Trend", trendData))  // Takes the 67% column

// Equal width columns (same as SetColumns)
.AddSection("Dashboard", section => section
    .SetColumnWidths(1, 1, 1)  // Three equal 33.33% columns
    .AddCanvas(1, canvas => canvas.AddNumberTile("Total", 500))
    .AddCanvas(1, canvas => canvas.AddNumberTile("Active", 342))
    .AddCanvas(1, canvas => canvas.AddNumberTile("Pending", 158)))
```

### Custom Themes
Customize the appearance with modern design options:
```csharp
var theme = new Theme
{
    PrimaryColor = "#10b981",
    SecondaryColor = "#6366f1",
    BackgroundColor = "#ffffff",
    TextColor = "#111827",
    FontFamily = "Inter, sans-serif",
    BorderRadius = "12px",          // Rounded corners
    ShadowIntensity = 0.1,           // Subtle shadows
    EnableAnimations = true,         // Hover effects
    EnableGradients = true           // Gradient backgrounds
};

var html = report.GenerateHtml(theme);
```

## Documentation

- [Getting Started Guide](docs/getting-started.md) - Detailed tutorial and examples
- [API Reference](docs/api-reference.md) - Complete API documentation
- [Sample Application](SharpReports.Sample/) - Full working example
- [Code Examples](Examples/) - Code snippets and patterns

## Requirements

- .NET 10.0 or later
- No external dependencies for core library
- Chart.js (loaded via CDN for HTML output)

## License

MIT License - see LICENSE file for details

## Contributing

Contributions welcome! Please open an issue or submit a pull request.
