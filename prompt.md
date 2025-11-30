Let's vibe code something cool!

I want to code a C# library: SharpReports. It will be a C# library that can be used to configure and generate reports.

A bit of background: I am currently extracting some data from my web application using the API. As I am extracting and processing the data, I'd like to immedeatly pass it on to a report generator that will allow me presenting the results. I am primarilily interested in HTML reports to show to customers, but for the sake of comparison of reports, JSON should be supporter too. As I am working on some data mining and aggregation in a separate app and I am struggling with presenting the results nicely and easily. So I am thinking of creating a library that will be able to generate a report.

I’d like to generate reports in different formats, but the primary focus is on nice looking html reports as a single file. I might then want to generate other formats: JSON, markdown, etc.

Below is the specification for the app. We are not yet writing any code. Let me know if the specification makes sense and if you have any questions before we start.

To be clear: I have already prepared the C# project called SharpReports

# C# Library Specification Template

## 1. Library Overview
- **Library Name**: SharpReports
- **Core Purpose**: The library should allow building (i.e., configuring) and generating reports.
- **Target Users**: C# library used in other C# projects.
- **Target Framework**: .NET 10

## 2. Report Types & Formats
What types of reports and output formats?
- **Output Formats**: Start with HTML and JSON. Other formats might follow, so the library must be flexible and extensible.
    - For JSON output
        - Mirror the report structure (sections, elements, etc.)
        - Just output the raw data in a structured format
        - Include metadata (title, generation date)
        - Doesn't actually try to show any charts, etc. This is just presented as raw data.
- **Report Styles**: Building a report should be flexible. A user will build their report as needed, adding different elements like titles, section, columns, charts, etc.
- **Complexity Level**: The reports will be configured by the user. The user will arrange the report into sections, columns, "data dislpays"(charts, number tiles, etc.)

## 3. Core Features
List the main capabilities in priority order:
1. Configure and generate reports based on input data. The input data will be usual types: int, double, IEnumerable, IDictionary, etc.
2. Chart/graph generation
3. Required desing elements:
    - Bar chart
    - Stacked bar chart
        - Dictionary<string, int>
    - Line chart
    - Pie chart
    - Number "tile"
        - Number + title. keep it simple for v1
    - Free text
    - Table
        - IEnumerable<Dictionary<string, object>> (list of rows, each row is key-value pairs)?
        - Dictionary<string, IEnumerable<object>> (column name → column values)?
4. The library is extensible to add new data displays, etc.
5. The library complies with coding best practices (SOLID, etc.)
6. Each data display can be changed individually. Though they can share common, basic functionality (base class)

## 4. API Design & Usage
- **Programming Model**: Fluent API
- **Example Usage**: Below is example usage of how I see the library being used. Nothing I am showing is strictly set in stone, but this is the general idea.
    - In the below example, "Set Columns" only sets colums to the relevant section.
    - I don't know how to handle the different sections while using the fluent API
```csharp
  // Example of how you'd like to use it
  var report = new ReportBuilder()
      .WithTitle("Report title")
      .AddSection("Summary", section => section.SetColumns(3).AddNumberTile(...))
      .AddSection("Top level results", section => section.SetColumns(3).AddNumberTile(...))
      .AddFooter("Generated today")
      .GenerateHtml();
```

## 5. Data Input
- **Data Sources**: variables and objects in a C# application
- **Data Binding**:
    - Titles for data displays should be passes as strings when configuring the data displays.
    

## 6. Customization & Styling
- **Templates**:  Simple theme support (just CSS variables for colors/fonts)
- **Styling Options**: In the first iteration, the styling options can be limited. I am interesting in presenting data/results and I am not fussed in how it looks.
- **Branding**: Optionally allow adding logo to the upper right corner of the report.
- **Localization**: Not needed.

## 7. Advanced Features
- **Charts & Visualizations**: Bar charts, Stacked bar charts, line graphs, pie charts.
- **Pagination**: Not needed. The report should be one page.
- **Conditional Formatting**: Not needed in the first iteration.
- **Grouping & Aggregation**: Not needed. This can be handled by the calling aplication.
- **Images**: Not needed.

## 8. Technical Requirements
- **Dependencies**: Minimize dependencies. Discuss if using a popular libraries would be beneficial. Use Chart.js via CDN for HTML output.
- **Performance**: The ability to handle very large datasets is not needed.
- **Threading**: Async support would be nice if it makes sense. Take into account that reports will be saved into files.
- **Memory**: No specific requirements at this time.

## 9. Integration & Distribution
- **Package Format**: NuGet package
- **Licensing**: To be set later. Don't concern yourself with this.
- **Documentation**: XML docs, samples, wiki
