using Avalonia;
using Avalonia.Animation;
using Avalonia.Animation.Easings;
using Avalonia.Controls;
using Avalonia.Media.Transformation;
using Avalonia.Styling;
using System;
using System.Linq;

namespace Tenner.Avalonia.Controls;

public class MasonryPanel : Panel
{
    public static readonly StyledProperty<int> ColumnsProperty =
        AvaloniaProperty.Register<MasonryPanel, int>(nameof(Columns), defaultValue: 3);

    public static readonly StyledProperty<double> SpacingProperty =
        AvaloniaProperty.Register<MasonryPanel, double>(nameof(Spacing), defaultValue: 8);

    public int Columns
    {
        get => GetValue(ColumnsProperty);
        set => SetValue(ColumnsProperty, value);
    }

    public double Spacing
    {
        get => GetValue(SpacingProperty);
        set => SetValue(SpacingProperty, value);
    }

    private (int column, double y)[] _layout = [];

    static MasonryPanel()
    {
        ColumnsProperty.Changed.AddClassHandler<MasonryPanel>((p, _) => p.InvalidateMeasure());
        SpacingProperty.Changed.AddClassHandler<MasonryPanel>((p, _) => p.InvalidateMeasure());
    }

    protected override void OnSizeChanged(SizeChangedEventArgs e)
    {
        base.OnSizeChanged(e);

        Columns = e.NewSize.Width switch
        {
            < 400 => 1,
            < 650 => 2,
            _ => 3
        };
    }

    protected override Size MeasureOverride(Size availableSize)
    {
        if (Children.Count == 0) return default;
        if (double.IsInfinity(availableSize.Width)) return default;

        int cols = Math.Max(1, Columns);
        double spacing = Spacing;
        double colWidth = (availableSize.Width - spacing * (cols - 1)) / cols;
        double[] colHeights = new double[cols];

        if (_layout.Length != Children.Count)
            _layout = new (int, double)[Children.Count];

        for (int i = 0; i < Children.Count; i++)
        {
            Children[i].Measure(new Size(colWidth, double.PositiveInfinity));
            int shortest = GetShortestColumn(colHeights);
            _layout[i] = (shortest, colHeights[shortest]);
            colHeights[shortest] += Children[i].DesiredSize.Height + spacing;
        }

        return new Size(availableSize.Width, colHeights.Max() - spacing);
    }

    protected override Size ArrangeOverride(Size finalSize)
    {
        if (Children.Count == 0) return finalSize;
        if (_layout.Length != Children.Count) return finalSize;

        int cols = Math.Max(1, Columns);
        double colWidth = (finalSize.Width - Spacing * (cols - 1)) / cols;

        for (int i = 0; i < Children.Count; i++)
        {
            var (col, y) = _layout[i];
            double x = col * (colWidth + Spacing);
            Children[i].Arrange(new Rect(x, y, colWidth, Children[i].DesiredSize.Height));
        }

        return finalSize;
    }

    private static int GetShortestColumn(double[] colHeights)
    {
        int shortest = 0;
        for (int i = 1; i < colHeights.Length; i++)
            if (colHeights[i] < colHeights[shortest])
                shortest = i;
        return shortest;
    }
}