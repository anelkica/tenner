using Avalonia.Labs.Gif;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Tenner.Avalonia.Models;
using Tenner.Avalonia.Services;
using Tenner.Core.Models;

namespace Tenner.Avalonia.ViewModels
{
    public partial class MainWindowViewModel : ViewModelBase
    {
        private readonly ITennerClient _client;

        public ObservableCollection<GifResult> Gifs { get; } = [];
        public bool IsLoading { get; set; }

        public MainWindowViewModel(ITennerClient client)
        {
            _client = client;

            _ = LoadFeaturedAsync();
        }

        public async Task LoadFeaturedAsync()
        {
            IsLoading = true;
            TenorResponse? response = await _client.GetFeaturedAsync();

            if (response is null) return;

            Gifs.Clear();
            foreach (GifResult? gif in response.Results)
                Gifs.Add(gif);

            IsLoading = false;
        }
    }
}
