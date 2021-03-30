# Sparkline

This is a lightweight .NET WinForms Control for creating Sparkline charts with simple style customization options

![Samples](https://i.imgur.com/8yXnv0I.png)

## Usage

Add a reference to the Sparkline control into your toolbox and draw the chart in your form. Showing your data is just a matter of populating the `Values` property of the object and calling the `Refresh` method.

```csharp
sparkline1.Values = new List<float>() { 97, 90, 91, 55, 69, 65 };
sparkline1.Refresh();
```

Make sure to always call the `Refresh()` method after updating the values from a list.

## Customization properties

- `Values` - A list of floats containing your series values. Make sure to call `Refresh()` after updating the list
- `YMin`/`YMax` - Set the Y axis range. Only works if auto ranging is disabled (see below)
- `AutoMin`/`AutoMax` - Automatically calculates the Y axis range
- `Filled` - Fills the area below the line series
- `FillColor` - Fill color, if `Filled` is set to true
- `LineStyle` - A `Pen` object defining the series style, such as color, width, pattern, etc.
- `LineColor` - Exposes color of the internal `Pen` object
- `LineWidth` - Exposes width of the internal `Pen` object
