using Avalonia;
using Avalonia.Controls;
using Avalonia.Labs.Gif;
using Avalonia.Media;
using Avalonia.VisualTree;
using System;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace Tenner.Avalonia.Controls;

public partial class GifCard : UserControl
{
    private static readonly HttpClient _http = new();

    public static readonly StyledProperty<string?> UrlProperty =
        AvaloniaProperty.Register<GifCard, string?>(nameof(Url));

    public static readonly StyledProperty<double> AspectRatioProperty =
        AvaloniaProperty.Register<GifCard, double>(nameof(AspectRatio), defaultValue: 1.0);

    public string? Url
    {
        get => GetValue(UrlProperty);
        set => SetValue(UrlProperty, value);
    }

    public double AspectRatio
    {
        get => GetValue(AspectRatioProperty);
        set => SetValue(AspectRatioProperty, value);
    }

    public GifCard()
    {
        InitializeComponent();
    }

    protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
    {
        base.OnPropertyChanged(change);
        if (change.Property == UrlProperty && !string.IsNullOrEmpty(Url))
            _ = LoadGifAsync(Url);
        if (change.Property == AspectRatioProperty)
            InvalidateMeasure();
    }

    protected override void OnSizeChanged(SizeChangedEventArgs e)
    {
        base.OnSizeChanged(e);
        //UpdateHeight(e.NewSize.Width);

        //(Parent as Control)?.InvalidateMeasure(); // tell parent to re-measure
    }

    protected override Size MeasureOverride(Size availableSize)
    {
        var width = double.IsInfinity(availableSize.Width) ? Bounds.Width : availableSize.Width;
        var height = width * AspectRatio;
        return new Size(width, height > 0 ? height : availableSize.Height);
    }

    private void UpdateHeight(double width)
    {
        if (width > 0)
            Card.Height = width * AspectRatio;

        Debug.WriteLine($"{Card.Height} | {AspectRatio}");
    }

    private async Task LoadGifAsync(string url)
    {
        try
        {
            byte[] bytes = await _http.GetByteArrayAsync(url);
            Gif.Source = new MemoryStream(bytes);
        }
        catch (Exception e)
        {
            Debug.WriteLine($"Failed to load GIF: {e.Message}");
        }
    }
}