using SharpReports.Core;
using SharpReports.Extensions;
using SharpReports.Rendering;

namespace SharpReports.Examples;

/// <summary>
/// Basic example demonstrating SharpReports usage
/// </summary>
public class BasicExample
{
    public static async Task GenerateSampleReport()
    {
        // Sample data
        var salesByRegion = new Dictionary<string, double>
        {
            ["North"] = 45000,
            ["South"] = 38000,
            ["East"] = 52000,
            ["West"] = 41000
        };

        var productSales = new Dictionary<string, IDictionary<string, double>>
        {
            ["Q1"] = new Dictionary<string, double> { ["Product A"] = 12000, ["Product B"] = 8000, ["Product C"] = 5000 },
            ["Q2"] = new Dictionary<string, double> { ["Product A"] = 15000, ["Product B"] = 9000, ["Product C"] = 6000 },
            ["Q3"] = new Dictionary<string, double> { ["Product A"] = 14000, ["Product B"] = 11000, ["Product C"] = 7000 },
            ["Q4"] = new Dictionary<string, double> { ["Product A"] = 16000, ["Product B"] = 10000, ["Product C"] = 8000 }
        };

        var monthlyTrend = new Dictionary<string, double>
        {
            ["Jan"] = 15000,
            ["Feb"] = 18000,
            ["Mar"] = 22000,
            ["Apr"] = 19000,
            ["May"] = 25000,
            ["Jun"] = 28000
        };

        var marketShare = new Dictionary<string, double>
        {
            ["Our Product"] = 35,
            ["Competitor A"] = 28,
            ["Competitor B"] = 20,
            ["Others"] = 17
        };

        var tableData = new List<Dictionary<string, object>>
        {
            new() { ["Employee"] = "Alice Johnson", ["Sales"] = 125000, ["Commission"] = 6250 },
            new() { ["Employee"] = "Bob Smith", ["Sales"] = 98000, ["Commission"] = 4900 },
            new() { ["Employee"] = "Carol Williams", ["Sales"] = 142000, ["Commission"] = 7100 },
            new() { ["Employee"] = "David Brown", ["Sales"] = 87000, ["Commission"] = 4350 }
        };

        // Build the report using fluent API
        var report = ReportBuilder.WithTitle("Q2 2024 Sales Report")
            .AddSection("Executive Summary", section => section
                .SetColumns(3)
                .AddNumberTile("Total Revenue", 176000, "C0", tooltip: "Total revenue from all regions for Q2 2024")
                .AddNumberTile("Total Orders", 1247, "N0", tooltip: "Number of orders received during the quarter")
                .AddNumberTile("Avg Order Value", 141.14, "C2", "↑ 12% vs Q1", "Average value per order compared to previous quarter"))

            .AddSection("Important Dates", section => section
                .SetColumns(3)
                .AddDateTile("Report Period Start", new DateTime(2024, 4, 1), "yyyy-MM-dd")
                .AddDateTile("Report Period End", new DateTime(2024, 6, 30), "yyyy-MM-dd")
                .AddDateTile("Next Review", DateOnly.FromDateTime(new DateTime(2024, 10, 15)), "dd MMM yyyy", "Q3 review date"))

            .AddSection("Sales by Region", section => section
                .SetColumns(2)
                .AddBarChart("Regional Performance", salesByRegion, tooltip: "Revenue by geographic region for Q2 2024")
                .AddPieChart("Market Distribution", salesByRegion, tooltip: "Percentage breakdown of sales across regions"))

            .AddSection("Product Performance", section => section
                .SetColumns(2)
                .AddStackedBarChart("Quarterly Product Sales", productSales, tooltip: "Product performance comparison across quarters")
                .AddLineChart("Monthly Revenue Trend", monthlyTrend, tooltip: "Month-over-month revenue progression"))

            .AddSection("Market Analysis", section => section
                .SetColumns(2)
                .AddPieChart("Market Share (%)", marketShare, isDonut: true, tooltip: "Our market share compared to competitors")
                .AddText("The market shows strong growth potential with our product " +
                         "maintaining a leading position. Focus on expanding in the " +
                         "South region where we see the most opportunity."))

            .AddSection("Canvas Example - Flexible Layout", section => section
                .SetColumnWidths(1, 2)  // 1:2 ratio = 33.33% / 66.67%
                .AddCanvas(2, canvas => canvas
                    .AddNumberTile("Active Customers", 342, "N0", tooltip: "Currently active customer accounts")
                    .AddDateTile("Last Updated", DateTime.Now, "yyyy-MM-dd HH:mm")
                    .AddNumberTile("New This Month", 28, "N0")
                    .AddBarChart("Quick Stats", new Dictionary<string, int> { ["Won"] = 15, ["Lost"] = 3, ["Pending"] = 8 }))
                .AddLineChart("Customer Growth", monthlyTrend, tooltip: "Customer acquisition trend over time"))

            .AddSection("Top Performers", section => section
                .AddTable("Sales Team Performance", tableData))

            .WithFooter("Confidential - Q2 2024 Sales Report")
            .Build();

        // Generate HTML
        var html = new HtmlRenderer().Render(report);
        await File.WriteAllTextAsync("sales-report.html", html);
        Console.WriteLine("HTML report generated: sales-report.html");

        // Generate JSON
        var json = new JsonRenderer().Render(report);
        await File.WriteAllTextAsync("sales-report.json", json);
        Console.WriteLine("JSON report generated: sales-report.json");

        // Alternative: use builder methods directly
        var htmlDirect = ReportBuilder.WithTitle("Quick Report")
            .AddSection("Summary", s => s
                .AddNumberTile("Users", 1500)
                .AddNumberTile("Revenue", 50000, "C0"))
            .GenerateHtml();

        // Save with custom theme (including new theme properties)
        var customTheme = new Theme
        {
            PrimaryColor = "#10b981",
            SecondaryColor = "#6366f1",
            BackgroundColor = "#ffffff",
            TextColor = "#111827",
            BorderRadius = "12px",
            ShadowIntensity = 0.1,
            EnableAnimations = true,
            EnableGradients = true
        };

        await ReportBuilder.WithTitle("Custom Themed Report")
            .AddSection("Data", s => s.AddNumberTile("Metric", 100))
            .SaveHtmlAsync("custom-report.html", customTheme);
        Console.WriteLine("Custom themed report generated: custom-report.html");
    }
}
