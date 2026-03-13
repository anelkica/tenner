namespace Tenner.Avalonia.Models;

public record GifItem(string Url, double Width, double Height)
{
    public double AspectRatio => Height / Width;
};