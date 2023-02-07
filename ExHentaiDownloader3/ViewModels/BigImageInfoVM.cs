using ExHentaiDownloader3.Core;
using Microsoft.UI.Xaml.Media.Imaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExHentaiDownloader3.ViewModels
{
    public class BigImageInfoVM : BindableBase
    {
        private string _thumbUrl;
        private string _detailPageUrl;

        private bool _isLoadingThumb = false;
        private BitmapImage _thumbSource;
        private bool _loadFailed = false;

        public string ThumbUrl { get => _thumbUrl; set => SetProperty(ref _thumbUrl, value); }
        public string DetailPageUrl { get => _detailPageUrl; set => SetProperty(ref _detailPageUrl, value); }
        public bool LoadFailed { get => _loadFailed; private set => SetProperty(ref _loadFailed, value); }

        public bool IsLoadingThumb
        {
            get => _isLoadingThumb;
            set
            {
                if (SetProperty(ref _isLoadingThumb, value))
                    RaisePropertyChanged(nameof(LoadFailed));
            }
        }

        public BitmapImage ThumbSource
        {
            get
            {
                if (_thumbSource is not null)
                    return _thumbSource;
                if (IsLoadingThumb)
                    return _thumbSource;

                UpdateThumbSource();
                return _thumbSource;
            }
            set => SetProperty(ref _thumbSource, value);
        }

        private async void UpdateThumbSource()
        {
            IsLoadingThumb = true;
            BitmapImage tmp = null;
            try
            {
                string imagePath = await DownloadManager.Instance.DownloadThumb(ThumbUrl);
                tmp = new BitmapImage(new Uri(imagePath));
            }
            catch (Exception)
            {
                LoadFailed = true;
            }
            IsLoadingThumb = false;

            ThumbSource = tmp;
        }
    }
}
