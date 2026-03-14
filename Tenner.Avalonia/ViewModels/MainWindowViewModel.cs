using Avalonia.Labs.Gif;
using System.Collections.Generic;
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

        public ObservableCollection<GifItem> Gifs { get; } = [];
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
            foreach (GifResult gif in response.Results)
            {
                MediaFormat? format = gif.MediaFormats.GetValueOrDefault("tinygif");
                if (format is null) return;

                // height / width
                double aspectRatio = format.Dims[1] / (double) format.Dims[0];
                Gifs.Add(new GifItem(format.Url, aspectRatio, gif));
            }

            IsLoading = false;
        }
    }
}
