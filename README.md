# SharpReports

A lightweight .NET library for generating beautiful HTML and JSON reports with charts, tables, and data tiles.

## Features

- Multiple output formats (HTML, JSON)
- Interactive charts powered by Chart.js (Bar, Line, Pie, Stacked Bar)
- Data tables and metric tiles
- Date and number formatting
- Customizable themes
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
Display key metrics with formatting:
```csharp
.AddNumberTile("Revenue", 50000, "C0")        // Currency
.AddNumberTile("Users", 1234, "N0")           // Number
.AddNumberTile("Rate", 0.156, "P1")           // Percentage
.AddDateTile("Launch Date", DateTime.Now, "yyyy-MM-dd")
```

### Charts
Multiple chart types with flexible data input:
```csharp
// Supports Dictionary, SortedDictionary, or any IDictionary
.AddBarChart("Sales", data)
.AddLineChart("Trend", data)
.AddPieChart("Distribution", data, isDonut: true)
.AddStackedBarChart("Comparison", nestedData)
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

### Custom Themes
```csharp
var theme = new Theme
{
    PrimaryColor = "#10b981",
    SecondaryColor = "#6366f1",
    BackgroundColor = "#ffffff",
    TextColor = "#111827",
    FontFamily = "Inter, sans-serif"
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
