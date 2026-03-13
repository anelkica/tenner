using Avalonia.Labs.Gif;
using System.Collections.ObjectModel;
using Tenner.Avalonia.Models;

namespace Tenner.Avalonia.ViewModels
{
    public partial class MainWindowViewModel : ViewModelBase
    {
        public ObservableCollection<GifItem> Gifs { get; } = [];
    }
}
