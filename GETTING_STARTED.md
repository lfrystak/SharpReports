# Getting Started with SharpReports

Welcome to SharpReports! This guide will help you get up and running quickly.

## Installation

Add SharpReports to your project:

```bash
dotnet add package SharpReports
```

Or add it to your `.csproj` file:

```xml
<ItemGroup>
  <PackageReference Include="SharpReports" Version="1.0.0" />
</ItemGroup>
```

## Your First Report

Here's a simple example to create your first report:

```csharp
using SharpReports.Core;
using SharpReports.Extensions;

// Create a simple report
var html = ReportBuilder.WithTitle("My First Report")
    .AddSection("Summary", section => section
        .AddNumberTile("Total Users", 1500)
        .AddNumberTile("Active Sessions", 342))
    .GenerateHtml();

// Save to file
await File.WriteAllTextAsync("my-report.html", html);
```

Open `my-report.html` in your browser, and you'll see a beautiful formatted report!

## Common Patterns

### 1. Adding Multiple Metrics

```csharp
var report = ReportBuilder.WithTitle("Dashboard")
    .AddSection("Key Metrics", section => section
        .SetColumns(4)  // 4-column layout
        .AddNumberTile("Revenue", 50000, "C0")      // Currency format
        .AddNumberTile("Orders", 1234, "N0")        // Number format
        .AddNumberTile("Conversion", 0.156, "P1")   // Percentage format
        .AddNumberTile("Rating", 4.7, "N1"))        // Decimal format
    .GenerateHtml();
```

### 2. Adding Date Information

```csharp
var report = ReportBuilder.WithTitle("Project Status")
    .AddSection("Timeline", section => section
        .SetColumns(3)
        .AddDateTile("Start Date", new DateTime(2024, 1, 1), "yyyy-MM-dd")
        .AddDateTile("Current Date", DateTime.Now, "MMMM dd, yyyy")
        .AddDateTile("Deadline", DateOnly.FromDateTime(DateTime.Now.AddMonths(3)), "dd MMM yyyy", "3 months from now"))
    .GenerateHtml();
```

### 3. Adding Charts

```csharp
// Works with double values
var salesData = new Dictionary<string, double>
{
    ["January"] = 45000,
    ["February"] = 52000,
    ["March"] = 48000
};

// Also works with integers!
var orderCounts = new Dictionary<string, int>
{
    ["January"] = 450,
    ["February"] = 520,
    ["March"] = 480
};

var report = ReportBuilder.WithTitle("Sales Report")
    .AddSection("Monthly Sales", section => section
        .AddBarChart("Revenue by Month", salesData)
        .AddBarChart("Order Counts", orderCounts)
        .AddLineChart("Trend", salesData))
    .GenerateHtml();
```

All chart methods accept `IDictionary` types, so you can use Dictionary, SortedDictionary, or custom implementations.

### 4. Adding Tables

```csharp
var employeeData = new List<Dictionary<string, object>>
{
    new() { ["Name"] = "Alice", ["Department"] = "Sales", ["Revenue"] = 125000 },
    new() { ["Name"] = "Bob", ["Department"] = "Marketing", ["Revenue"] = 98000 }
};

var report = ReportBuilder.WithTitle("Employee Report")
    .AddSection("Top Performers", section => section
        .AddTable("Q1 Performance", employeeData))
    .GenerateHtml();
```

### 5. Multiple Sections with Different Layouts

```csharp
var report = ReportBuilder.WithTitle("Comprehensive Report")
    .AddSection("Overview", s => s
        .SetColumns(3)
        .AddNumberTile("Metric 1", 100)
        .AddNumberTile("Metric 2", 200)
        .AddNumberTile("Metric 3", 300))

    .AddSection("Charts", s => s
        .SetColumns(2)
        .AddBarChart("Chart 1", data1)
        .AddPieChart("Chart 2", data2))

    .AddSection("Details", s => s
        .SetColumns(1)
        .AddTable("Full Data", tableData))

    .WithFooter("Generated: " + DateTime.Now.ToString("yyyy-MM-dd"))
    .GenerateHtml();
```

## Saving Reports

### HTML Output
```csharp
// Generate as string
var html = report.GenerateHtml();

// Generate async
var html = await report.GenerateHtmlAsync();

// Save directly to file
await report.SaveHtmlAsync("report.html");

// With custom theme
var theme = new Theme { PrimaryColor = "#059669" };
await report.SaveHtmlAsync("report.html", theme);
```

### JSON Output
```csharp
// Generate as string
var json = report.GenerateJson();

// Generate async
var json = await report.GenerateJsonAsync();

// Save to file
await report.SaveJsonAsync("report.json");
```

## Customizing Appearance

```csharp
var customTheme = new Theme
{
    PrimaryColor = "#10b981",        // Emerald green
    SecondaryColor = "#6366f1",      // Indigo
    BackgroundColor = "#ffffff",     // White
    TextColor = "#111827",           // Dark gray
    FontFamily = "Georgia, serif"    // Custom font
};

var html = report.GenerateHtml(customTheme);
```

## Adding a Logo

```csharp
var report = ReportBuilder.WithTitle("Company Report")
    .WithLogo("https://example.com/logo.png")
    .AddSection(...)
    .GenerateHtml();
```

## Number Formatting

SharpReports uses standard .NET format strings:

- **Currency**: `"C0"` → $1,234, `"C2"` → $1,234.56
- **Number**: `"N0"` → 1,234, `"N2"` → 1,234.56
- **Percentage**: `"P0"` → 12%, `"P1"` → 12.3%
- **Decimal**: `"F2"` → 1234.56

```csharp
section.AddNumberTile("Price", 1234.56, "C2")      // $1,234.56
section.AddNumberTile("Count", 1234, "N0")         // 1,234
section.AddNumberTile("Rate", 0.123, "P1")         // 12.3%
```

## Real-World Example: API Data Processing

```csharp
// Fetch data from your API
var apiData = await FetchDataFromApi();

// Process and generate report
var report = ReportBuilder.WithTitle("API Analytics Report")
    .AddSection("API Performance", section => section
        .SetColumns(4)
        .AddNumberTile("Total Requests", apiData.TotalRequests, "N0")
        .AddNumberTile("Avg Response Time", apiData.AvgResponseMs, "N0", "milliseconds")
        .AddNumberTile("Success Rate", apiData.SuccessRate, "P1")
        .AddNumberTile("Error Rate", apiData.ErrorRate, "P2"))

    .AddSection("Request Distribution", section => section
        .SetColumns(2)
        .AddBarChart("Requests by Endpoint", apiData.RequestsByEndpoint)
        .AddPieChart("Status Codes", apiData.StatusCodeDistribution))

    .AddSection("Top Errors", section => section
        .AddTable("Error Log", apiData.TopErrors))

    .WithFooter($"Report generated: {DateTime.Now:yyyy-MM-dd HH:mm:ss}")
    .Build();

// Save both formats
await report.SaveHtmlAsync("api-report.html");
await report.SaveJsonAsync("api-report.json");
```

## Next Steps

- Check out the [README.md](README.md) for complete API documentation
- Explore the [Examples](Examples/) directory for more samples
- Run the TestApp to see a comprehensive example

## Tips

1. **Use appropriate columns**: 3-4 columns work well for metrics, 1-2 for charts
2. **Format numbers**: Always specify format strings for better presentation
3. **Add context**: Use subtitles on number tiles to add context
4. **Section organization**: Group related data into sections
5. **Responsive design**: Reports automatically adapt to mobile devices
6. **Save both formats**: HTML for viewing, JSON for data processing

## Support

For issues, questions, or contributions, please visit the [GitHub repository](https://github.com/yourusername/sharpreports).
