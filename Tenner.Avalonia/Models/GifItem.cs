using System.IO;
using Tenner.Core.Models;

namespace Tenner.Avalonia.Models;

public record GifItem(string Url, double AspectRatio, GifResult Source);