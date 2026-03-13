using Avalonia;
using Avalonia.Controls;
using Avalonia.Labs.Gif;

namespace Tenner.Avalonia.Controls;

public partial class GifCard : UserControl
{
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
        if (change.Property == UrlProperty && Url is not null)
            Gif.Source = Url;
    }

    protected override void OnSizeChanged(SizeChangedEventArgs e)
    {
        base.OnSizeChanged(e);
        UpdateHeight(e.NewSize.Width);
    }

    private void UpdateHeight(double width)
    {
        if (width > 0)
            Card.Height = width * AspectRatio;
    }
}