# SharpReports Quick Reference

## Basic Report Structure

```csharp
using SharpReports.Core;
using SharpReports.Extensions;

var report = ReportBuilder.WithTitle("Report Title")
    .AddSection("Section Name", section => { })
    .WithFooter("Footer text")
    .WithLogo("logo-url")
    .GenerateHtml();
```

## Elements

### Number Tile
```csharp
section.AddNumberTile(title, value, format?, subtitle?, tooltip?)

// Examples:
.AddNumberTile("Revenue", 50000, "C0")
.AddNumberTile("Count", 1234, "N0")
.AddNumberTile("Rate", 0.156, "P1", "↑ vs last month")
.AddNumberTile("Score", 85.5, "N1", "Points", "Calculated from user ratings")  // With tooltip
```

### Date Tile
```csharp
section.AddDateTile(title, dateValue, format?, subtitle?, tooltip?)

// Examples:
.AddDateTile("Launch Date", new DateTime(2024, 6, 15), "yyyy-MM-dd")
.AddDateTile("Deadline", DateOnly.Parse("2024-12-31"), "dd MMM yyyy", "End of year")
.AddDateTile("Report Date", DateTime.Now, "MMMM dd, yyyy HH:mm", tooltip: "Automatically updated")
```

Supports both `DateTime` and `DateOnly` types.

### Bar Chart
```csharp
section.AddBarChart(title, data, isHorizontal?, tooltip?)

// Accepts IDictionary<string, double> or IDictionary<string, int>
.AddBarChart("Sales", new Dictionary<string, double> { ["Q1"] = 45000 })
.AddBarChart("Counts", new Dictionary<string, int> { ["Items"] = 100 })
.AddBarChart("Revenue", salesData, tooltip: "Quarterly revenue breakdown")
```

### Stacked Bar Chart
```csharp
section.AddStackedBarChart(title, data, isHorizontal?, tooltip?)

// Accepts double or int data types
// Multi-series (double):
.AddStackedBarChart("Sales", new Dictionary<string, Dictionary<string, double>>
{
    ["Q1"] = new() { ["Product A"] = 12000, ["Product B"] = 8000 }
}, tooltip: "Product comparison by quarter")

// Multi-series (int):
.AddStackedBarChart("Tasks", new Dictionary<string, Dictionary<string, int>>
{
    ["Sprint 1"] = new() { ["Done"] = 15, ["In Progress"] = 8 }
})

// Simple single-series:
.AddStackedBarChart("Categories", new Dictionary<string, int>
{
    ["Category A"] = 100,
    ["Category B"] = 200
})
```

### Line Chart
```csharp
section.AddLineChart(title, data, showPoints?, tooltip?)

// Multi-series:
.AddLineChart("Trends", new Dictionary<string, Dictionary<string, double>>
{
    ["Series 1"] = new() { ["Jan"] = 100, ["Feb"] = 150 },
    ["Series 2"] = new() { ["Jan"] = 80, ["Feb"] = 120 }
}, showPoints: true, tooltip: "Monthly trends comparison")

// Single series:
.AddLineChart("Trend", new Dictionary<string, double>
{
    ["Jan"] = 100,
    ["Feb"] = 150
})
```

### Pie Chart
```csharp
section.AddPieChart(title, data, isDonut?, tooltip?)

// Accepts IDictionary<string, double> or IDictionary<string, int>
.AddPieChart("Distribution", new Dictionary<string, double>
{
    ["Category A"] = 35.5,
    ["Category B"] = 28.3
}, isDonut: true, tooltip: "Percentage breakdown by category")

.AddPieChart("Votes", new Dictionary<string, int>
{
    ["Option A"] = 42,
    ["Option B"] = 38
})
```

### Table
```csharp
section.AddTable(title, rows)

// From rows:
.AddTable("Data", new List<Dictionary<string, object>>
{
    new() { ["Name"] = "Alice", ["Value"] = 100 },
    new() { ["Name"] = "Bob", ["Value"] = 200 }
})

// From columns:
.AddTableFromColumns("Data", new Dictionary<string, IEnumerable<object>>
{
    ["Name"] = new[] { "Alice", "Bob" },
    ["Value"] = new object[] { 100, 200 }
})
```

### Free Text
```csharp
section.AddText(content, isHtml?)

// Examples:
.AddText("Plain text content")
.AddText("<p>HTML <strong>content</strong></p>", isHtml: true)
```

### Canvas
Mix different element types in flexible layouts:
```csharp
section.AddCanvas(columns, configure)

// Examples:
.AddCanvas(2, canvas => canvas  // 2 internal columns, elements auto-flow
    .AddNumberTile("Metric 1", 100, "N0")
    .AddBarChart("Chart 1", data)
    .AddNumberTile("Metric 2", 200, "N0")
    .AddPieChart("Chart 2", pieData))

.AddCanvas(3, canvas => canvas  // 3 columns for compact layout
    .AddNumberTile("Total", 500)
    .AddNumberTile("Active", 342)
    .AddNumberTile("Pending", 158))
```

Canvas supports all element types (tiles, charts, tables, text) and automatically flows elements left-to-right across its columns. The Canvas itself always fills 100% of its section column width.

## Layout

### Set Columns
```csharp
section.SetColumns(columnCount)

// Example:
.AddSection("Dashboard", section => section
    .SetColumns(3)  // Creates 3 equal-width columns
    .AddNumberTile(...)
    .AddNumberTile(...)
    .AddNumberTile(...))
```

### Set Column Widths
Use unit-based ratios for custom column widths:
```csharp
section.SetColumnWidths(params int[] widths)

// Examples:
.SetColumnWidths(1, 2)      // 33.33% / 66.67% (1:2 ratio)
.SetColumnWidths(1, 1)      // 50% / 50% (same as SetColumns(2))
.SetColumnWidths(1, 1, 1)   // 33.33% / 33.33% / 33.33% (equal thirds)
.SetColumnWidths(2, 3)      // 40% / 60% (2:3 ratio)
.SetColumnWidths(1, 2, 1)   // 25% / 50% / 25% (sidebar-content-sidebar)

// Usage:
.AddSection("Mixed Layout", section => section
    .SetColumnWidths(1, 2)  // Left: 33.33%, Right: 66.67%
    .AddCanvas(2, canvas => canvas
        .AddNumberTile("Metric 1", 100)
        .AddNumberTile("Metric 2", 200))
    .AddBarChart("Main Chart", data))
```

The total units are summed automatically, and percentages are calculated proportionally. This is much more intuitive than specifying exact percentages!

## Output Methods

### Generate HTML
```csharp
// As string
var html = report.GenerateHtml();
var html = report.GenerateHtml(customTheme);

// Async
var html = await report.GenerateHtmlAsync();
var html = await report.GenerateHtmlAsync(customTheme);

// Save to file
await report.SaveHtmlAsync("report.html");
await report.SaveHtmlAsync("report.html", customTheme);
```

### Generate JSON
```csharp
// As string
var json = report.GenerateJson();

// Async
var json = await report.GenerateJsonAsync();

// Save to file
await report.SaveJsonAsync("report.json");
```

## Theming

```csharp
var theme = new Theme
{
    PrimaryColor = "#2563eb",        // Default: #2563eb (blue)
    SecondaryColor = "#64748b",      // Default: #64748b (gray)
    BackgroundColor = "#ffffff",     // Default: #ffffff (white)
    TextColor = "#1e293b",           // Default: #1e293b (dark)
    FontFamily = "Arial, sans-serif", // Default: system fonts
    BorderRadius = "12px",           // Default: "8px" - rounded corners
    ShadowIntensity = 0.1,           // Default: 0.08 - shadow strength (0-1)
    EnableAnimations = true,         // Default: true - hover effects
    EnableGradients = true           // Default: true - gradient backgrounds
};

var html = report.GenerateHtml(theme);
```

Theme properties:
- **PrimaryColor**: Main accent color for headers and important elements
- **SecondaryColor**: Secondary accent color
- **BackgroundColor**: Page background color
- **TextColor**: Text color throughout the report
- **FontFamily**: Font stack for all text
- **BorderRadius**: Corner rounding for elements (e.g., "8px", "12px")
- **ShadowIntensity**: Depth of shadows (0 = no shadow, 1 = heavy shadow)
- **EnableAnimations**: Enable/disable hover and transition effects
- **EnableGradients**: Enable/disable gradient backgrounds on elements

## Number Format Strings

| Format | Example Input | Output |
|--------|--------------|--------|
| `"C0"` | 1234.56 | $1,235 |
| `"C2"` | 1234.56 | $1,234.56 |
| `"N0"` | 1234.56 | 1,235 |
| `"N2"` | 1234.56 | 1,234.56 |
| `"P0"` | 0.1234 | 12% |
| `"P1"` | 0.1234 | 12.3% |
| `"P2"` | 0.1234 | 12.34% |
| `"F2"` | 1234.56 | 1234.56 |

## Complete Example

```csharp
var report = ReportBuilder.WithTitle("Q2 Sales Report")
    .WithLogo("https://example.com/logo.png")

    // Metrics section - 4 columns
    .AddSection("Key Metrics", s => s
        .SetColumns(4)
        .AddNumberTile("Revenue", 432000, "C0")
        .AddNumberTile("Orders", 1247, "N0")
        .AddNumberTile("Conversion", 0.198, "P1", "↑ vs Q1")
        .AddNumberTile("Rating", 4.7, "N1"))

    // Charts section - 2 columns
    .AddSection("Performance", s => s
        .SetColumns(2)
        .AddBarChart("Sales by Region", regionData)
        .AddLineChart("Monthly Trend", monthlyData))

    // Table section - full width
    .AddSection("Details", s => s
        .AddTable("Top Customers", customerData))

    .WithFooter("Confidential - Q2 2024")
    .Build();

// Generate both formats
await report.SaveHtmlAsync("report.html");
await report.SaveJsonAsync("report.json");
```

## Common Patterns

### Dashboard with Metrics and Charts
```csharp
ReportBuilder.WithTitle("Dashboard")
    .AddSection("Overview", s => s.SetColumns(4)
        .AddNumberTile("Users", 1500)
        .AddNumberTile("Revenue", 50000, "C0")
        .AddNumberTile("Growth", 0.15, "P1")
        .AddNumberTile("Rating", 4.5))
    .AddSection("Analytics", s => s.SetColumns(2)
        .AddBarChart("Traffic", trafficData)
        .AddPieChart("Sources", sourceData))
```

### Comparison Report
```csharp
ReportBuilder.WithTitle("Year over Year")
    .AddSection("Comparison", s => s.SetColumns(2)
        .AddStackedBarChart("2024 vs 2023", comparisonData)
        .AddLineChart("Trends", trendData))
```

### Data Export
```csharp
ReportBuilder.WithTitle("Data Export")
    .AddSection("Full Dataset", s => s
        .AddTable("All Records", records))
    .GenerateJson()  // Perfect for data export
```
