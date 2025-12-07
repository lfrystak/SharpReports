# SharpReports Architecture

## Overview

SharpReports is designed following SOLID principles with a clean, extensible architecture that separates concerns between report building, element representation, and rendering.

## Project Structure

```
SharpReports/
├── Core/                           # Core report building infrastructure
│   ├── IReportElement.cs          # Base interface for all elements
│   ├── Report.cs                  # Report data model
│   ├── ReportBuilder.cs           # Fluent API builder
│   └── ReportSection.cs           # Section container
│
├── Elements/                       # Report element implementations
│   ├── ReportElementBase.cs       # Base class for elements
│   ├── NumberTile.cs              # Metric display
│   ├── DateTile.cs                # Date/DateTime display
│   ├── FreeText.cs                # Text content
│   ├── Table.cs                   # Tabular data
│   └── Charts/                    # Chart implementations
│       ├── ChartBase.cs           # Base class for charts
│       ├── BarChart.cs
│       ├── StackedBarChart.cs
│       ├── LineChart.cs
│       └── PieChart.cs
│
├── Rendering/                      # Output format renderers
│   ├── IRenderer.cs               # Renderer interface
│   ├── HtmlRenderer.cs            # HTML + Chart.js output
│   ├── JsonRenderer.cs            # JSON output
│   └── Theme.cs                   # Styling configuration
│
└── Extensions/                     # Fluent API extensions
    └── ReportSectionExtensions.cs # Section builder extensions
```

## Design Patterns

### 1. Builder Pattern
The `ReportBuilder` class implements the builder pattern with a fluent API:

```csharp
ReportBuilder.WithTitle("Report")
    .AddSection("Section", s => { })
    .WithFooter("Footer")
    .GenerateHtml();
```

### 2. Strategy Pattern
Different renderers implement `IRenderer` interface:
- `HtmlRenderer` - Generates HTML with Chart.js
- `JsonRenderer` - Generates structured JSON
- Future: `MarkdownRenderer`, `PdfRenderer`, etc.

### 3. Composite Pattern
Reports contain sections, sections contain elements:
```
Report
  └─ Section[]
      └─ IReportElement[]
          ├─ NumberTile
          ├─ Chart
          └─ Table
```

## Core Components

### IReportElement
Base interface for all report elements. Provides:
- `Id` - Unique identifier
- `ElementType` - Element type name

All elements inherit from `ReportElementBase` which implements this interface.

### Report
The main data model containing:
- `Title` - Report title
- `Sections` - List of sections
- `Footer` - Optional footer text
- `LogoUrl` - Optional logo URL
- `GeneratedAt` - Timestamp

### ReportBuilder
Fluent API builder that:
- Constructs reports step-by-step
- Provides convenience methods for generating output
- Supports both sync and async operations

### ReportSection
Container for elements with:
- `Title` - Section title
- `Columns` - Layout column count (1-4)
- `Elements` - List of report elements

## Element System

### Base Elements

**NumberTile**
- Displays a single metric with optional formatting
- Supports .NET format strings (C, N, P, etc.)
- Optional subtitle for context

**DateTile**
- Displays DateTime or DateOnly values with formatting
- Supports custom date format strings
- Optional subtitle for context
- Handles both date and datetime types

**FreeText**
- Plain text or HTML content
- Useful for descriptions and notes

**Table**
- Row-based or column-based data input
- Automatic column detection
- Flexible data structure

### Chart Elements

All charts inherit from `ChartBase` and accept `IDictionary` types:

**BarChart**
- Vertical or horizontal bars
- Accepts `IDictionary<string, double>` or `IDictionary<string, int>`
- Simple label → value mapping

**StackedBarChart**
- Multiple series stacked
- Accepts double or int data types
- Supports both simple and complex data structures

**LineChart**
- Single or multiple series
- Accepts `IDictionary` types for flexible input
- Optional point markers

**PieChart**
- Standard pie or donut chart
- Percentage-based visualization

## Rendering System

### IRenderer Interface
```csharp
public interface IRenderer
{
    string Render(Report report, Theme? theme = null);
}
```

### HtmlRenderer
- Generates self-contained HTML
- Integrates Chart.js from CDN
- CSS grid-based responsive layout
- Theme support via CSS variables

### JsonRenderer
- Mirrors report structure
- Includes all element data
- Formatted for readability
- Useful for data export

## Extension Points

### 1. Custom Elements
Create custom elements by implementing `IReportElement`:

```csharp
public class CustomElement : ReportElementBase
{
    public override string ElementType => "CustomElement";

    // Add properties and logic
}
```

### 2. Custom Renderers
Add new output formats by implementing `IRenderer`:

```csharp
public class MarkdownRenderer : IRenderer
{
    public string Render(Report report, Theme? theme = null)
    {
        // Generate markdown
    }
}
```

### 3. Extension Methods
Add fluent API methods via extension methods:

```csharp
public static class CustomExtensions
{
    public static ReportSection AddCustomElement(
        this ReportSection section, ...)
    {
        return section.AddElement(new CustomElement(...));
    }
}
```

## Data Flow

1. **Build Phase**
   ```
   ReportBuilder → Report → Section[] → IReportElement[]
   ```

2. **Render Phase**
   ```
   Report → IRenderer → Output String (HTML/JSON)
   ```

3. **Save Phase**
   ```
   Output String → File.WriteAllTextAsync → File on disk
   ```

## Key Design Decisions

### 1. Immutable vs Mutable
- `Report`, `Section`, and elements are **mutable** for simplicity
- Builder pattern allows for flexible construction
- Could be refactored to immutable if needed

### 2. Data Structures
- Charts accept `IDictionary<>` interfaces (not concrete Dictionary)
- Support for both `double` and `int` data types
- Tables support both row-based and column-based input
- Flexible enough for most scenarios

### 3. IDictionary vs Dictionary
**Decision:** Accept IDictionary interfaces instead of concrete Dictionary types
**Rationale:**
- More flexible - works with Dictionary, SortedDictionary, custom implementations
- Follows "Program to interfaces" principle
- Internally converts to Dictionary for storage
- Allows users to pass various dictionary types without casting

### 4. Rendering Strategy
- HTML uses Chart.js via CDN (no bundling needed)
- JSON mirrors the object structure
- Separation allows for easy addition of new formats

### 5. Theming
- CSS variables for easy customization
- Theme object is simple and focused
- Easy to extend with more properties

### 6. Async Support
- File I/O operations are async
- Rendering is sync (CPU-bound)
- Async wrappers provided for convenience

## SOLID Principles Applied

### Single Responsibility
- Each element type has one job
- Renderers only handle output generation
- Builder only constructs reports

### Open/Closed
- Easy to add new elements (extend `ReportElementBase`)
- Easy to add new renderers (implement `IRenderer`)
- No need to modify existing code

### Liskov Substitution
- All elements can be used interchangeably via `IReportElement`
- All renderers can be swapped via `IRenderer`

### Interface Segregation
- `IReportElement` is minimal
- `IRenderer` has single method
- Extensions provide rich API without polluting interfaces

### Dependency Inversion
- Renderers depend on `Report` abstraction
- Builder works with `IReportElement` interface
- Easy to mock and test

## Performance Considerations

### Memory
- Reports are kept in memory during construction
- Rendering streams output to `StringBuilder`
- Suitable for reports with thousands of data points

### CPU
- Rendering is CPU-bound (string building)
- Chart.js does client-side rendering
- No heavy computations in the library

### I/O
- File operations are async
- No buffering needed for typical report sizes
- Could add streaming for very large outputs

## Testing Strategy

### Unit Tests (Future)
- Test each element type independently
- Test renderers with mock data
- Test builder API

### Integration Tests (Future)
- End-to-end report generation
- Validate HTML structure
- Validate JSON format

### Example Tests
- `BasicExample.cs` demonstrates real usage
- `TestApp` provides comprehensive sample

## Future Enhancements

### Potential Features
1. **More chart types**: Scatter, radar, area charts
2. **More output formats**: Markdown, PDF, Excel
3. **Conditional formatting**: Color-code values based on thresholds
4. **Data aggregation**: Built-in grouping and calculations
5. **Interactive features**: Filters, sorting for HTML output
6. **Template system**: Pre-built report templates
7. **Streaming**: Support for very large datasets

### Backward Compatibility
- Current API is designed to be stable
- New features should be additive
- Breaking changes would require major version bump

## Dependencies

### Runtime
- **.NET 10.0** - Target framework
- **System.Text.Json** - Built-in JSON serialization
- **Chart.js** (CDN) - Client-side charting for HTML

### No External NuGet Packages
- Minimal dependencies by design
- Easy to audit and maintain
- Fast installation and build times

## Conclusion

SharpReports provides a clean, extensible architecture for report generation. The design follows industry best practices and allows for easy customization and extension without modifying core code.
