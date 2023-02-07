using ExHentaiDownloader3.Core;
using Microsoft.UI.Xaml.Media.Imaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExHentaiDownloader3.ViewModels
{
    public class BigImageVM : BindableBase
    {
        private string _imageUrl;

        private bool _isLoadingThumb = false;
        private BitmapImage _iamgeSource;
        private bool _loadFailed = false;

        public BitmapImage IamgeSource { get => _iamgeSource; set => SetProperty(ref _iamgeSource, value); }
        public bool LoadFailed { get => _loadFailed; set =>  SetProperty(ref _loadFailed, value); }

        public bool IsLoadingThumb
        {
            get => _isLoadingThumb;
            set
            {
                if (SetProperty(ref _isLoadingThumb, value))
                    RaisePropertyChanged(nameof(LoadFailed));
            }
        }

        public BitmapImage ImageSource
        {
            get
            {
                if (_iamgeSource is not null)
                    return _iamgeSource;
                if (IsLoadingThumb)
                    return _iamgeSource;

                UpdateThumbSource();
                return _iamgeSource;
            }
            set => SetProperty(ref _iamgeSource, value);
        }

        private async void UpdateThumbSource()
        {
            IsLoadingThumb = true;
            BitmapImage tmp = null;
            try
            {
                string imagePath = await DownloadManager.Instance.DownloadThumb(_imageUrl);
                tmp = new BitmapImage(new Uri(imagePath));
            }
            catch (Exception)
            {
                LoadFailed = true;
            }
            IsLoadingThumb = false;

            ImageSource = tmp;
        }
    }
}
