using SharpReports.Core;
using SharpReports.Extensions;
using SharpReports.Rendering;

Console.WriteLine("SharpReports Test Application");
Console.WriteLine("============================\n");

// Sample data - simulating data extracted from an API
var salesByRegion = new Dictionary<string, double>
{
    ["North America"] = 125000,
    ["Europe"] = 98000,
    ["Asia Pacific"] = 142000,
    ["Latin America"] = 67000
};

var monthlyRevenue = new Dictionary<string, double>
{
    ["January"] = 45000,
    ["February"] = 52000,
    ["March"] = 48000,
    ["April"] = 55000,
    ["May"] = 61000,
    ["June"] = 58000
};

var productMix = new Dictionary<string, double>
{
    ["Enterprise"] = 45,
    ["Professional"] = 30,
    ["Starter"] = 25
};

var quarterlyGrowth = new Dictionary<string, Dictionary<string, double>>
{
    ["Q1"] = new() { ["Revenue"] = 145000, ["Costs"] = 95000 },
    ["Q2"] = new() { ["Revenue"] = 174000, ["Costs"] = 102000 },
};

var topCustomers = new List<Dictionary<string, object>>
{
    new() { ["Customer"] = "Acme Corp", ["Revenue"] = 45000, ["Status"] = "Active" },
    new() { ["Customer"] = "TechStart Inc", ["Revenue"] = 38000, ["Status"] = "Active" },
    new() { ["Customer"] = "Global Solutions", ["Revenue"] = 35000, ["Status"] = "Active" },
    new() { ["Customer"] = "Innovation Labs", ["Revenue"] = 28000, ["Status"] = "Pending" }
};

Console.WriteLine("Building report...");

// Build the report using the fluent API
var report = ReportBuilder.WithTitle("Q2 2024 Business Performance Report")
    .WithLogo("https://via.placeholder.com/150x50/2563eb/ffffff?text=MyCompany")
    .AddSection("Key Metrics", section => section
        .SetColumns(4)
        .AddNumberTile("Total Revenue", 432000, "C0")
        .AddNumberTile("New Customers", 47, "N0")
        .AddNumberTile("Growth Rate", 0.198, "P1", "↑ vs Q1")
        .AddNumberTile("Customer Satisfaction", 4.7, "N1", "out of 5.0"))

    .AddSection("Timeline", section => section
        .SetColumns(3)
        .AddDateTile("Quarter Start", new DateTime(2024, 4, 1), "MMMM dd, yyyy")
        .AddDateTile("Quarter End", new DateTime(2024, 6, 30), "MMMM dd, yyyy")
        .AddDateTile("Report Generated", DateTime.Now, "yyyy-MM-dd HH:mm", "Current time"))

    .AddSection("Revenue Analysis", section => section
        .SetColumns(2)
        .AddBarChart("Revenue by Region", salesByRegion)
        .AddLineChart("Monthly Revenue Trend", monthlyRevenue))

    .AddSection("Product & Growth", section => section
        .SetColumns(2)
        .AddPieChart("Product Mix (%)", productMix, isDonut: true)
        .AddStackedBarChart("Quarterly Performance", quarterlyGrowth))

    .AddSection("Top Customers", section => section
        .AddTable("Q2 Top Revenue Contributors", topCustomers))

    .AddSection("Summary", section => section
        .AddText(@"Q2 2024 showed strong performance across all regions with total revenue of $432,000,
representing a 19.8% increase over Q1. Asia Pacific continues to be our strongest market,
while Latin America presents significant growth opportunities.

The Enterprise product tier dominates our revenue mix at 45%, indicating strong traction
in the high-value segment. Customer satisfaction remains high at 4.7/5.0.

Key focus areas for Q3:
• Expand sales team in Latin America
• Launch new features for Professional tier
• Increase customer retention initiatives"))

    .WithFooter("Confidential - Internal Use Only")
    .Build();

Console.WriteLine("Report built successfully!");

// Generate HTML output
Console.WriteLine("\nGenerating HTML report...");
var html = new HtmlRenderer().Render(report);
var htmlPath = Path.Combine(Directory.GetCurrentDirectory(), "business-report.html");
await File.WriteAllTextAsync(htmlPath, html);
Console.WriteLine($"✓ HTML report saved to: {htmlPath}");

// Generate JSON output
Console.WriteLine("\nGenerating JSON report...");
var json = new JsonRenderer().Render(report);
var jsonPath = Path.Combine(Directory.GetCurrentDirectory(), "business-report.json");
await File.WriteAllTextAsync(jsonPath, json);
Console.WriteLine($"✓ JSON report saved to: {jsonPath}");

// Generate with custom theme
Console.WriteLine("\nGenerating custom-themed HTML report...");
var customTheme = new Theme
{
    PrimaryColor = "#059669",      // Emerald green
    SecondaryColor = "#6366f1",    // Indigo
    BackgroundColor = "#ffffff",
    TextColor = "#111827",
    FontFamily = "Georgia, serif"
};

var customHtml = new HtmlRenderer().Render(report, customTheme);
var customPath = Path.Combine(Directory.GetCurrentDirectory(), "business-report-custom.html");
await File.WriteAllTextAsync(customPath, customHtml);
Console.WriteLine($"✓ Custom themed report saved to: {customPath}");

Console.WriteLine("\n✓ All reports generated successfully!");
Console.WriteLine("\nOpen the HTML files in a browser to view the reports.");
