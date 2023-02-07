using ExHentaiDownloader3.Core;
using ExHentaiDownloader3.Core.Exhentai;
using Microsoft.UI.Xaml.Media.Imaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExHentaiDownloader3.ViewModels
{
    partial class BigImageInfoVM
    {
        private int _index = 0;
        private string _bookName = string.Empty;
        private bool _isLoadingBigImage = false;
        private BitmapImage _bigImageSource;
        private bool _LoadBigFailed = false;
        private string _bigImageUrl;

        public int Index { get => _index; set => SetProperty(ref _index, value); }
        public string BookName { get => _bookName; set => SetProperty(ref _bookName, value); }

        public bool IsLoadingBigImage { get => _isLoadingBigImage; set => SetProperty(ref _isLoadingBigImage, value); }
        public bool LoadBigFailed { get => _LoadBigFailed; set => SetProperty(ref _LoadBigFailed, value); }
        
        public BitmapImage BigImageSource
        {
            get
            {
                if (_bigImageSource is not null) return _bigImageSource;
                if (IsLoadingBigImage) return _bigImageSource;

                UpdateBigSource();

                return _bigImageSource;
            }
            set => SetProperty(ref _bigImageSource, value); 
        }


        private async void UpdateBigSource()
        {
            LoadBigFailed = false;
            IsLoadingBigImage = true;
            BitmapImage tmp = null;
            try
            {
                if (string.IsNullOrWhiteSpace(_bigImageUrl))
                {
                    BigImagePage bip = new BigImagePage(DetailPageUrl);
                    await bip.Load();

                    _bigImageUrl = bip.ImageUrl;
                }
                string imagePath = await DownloadManager.Instance.DownloadBigImage(BookName, _bigImageUrl, Index);
                tmp = new BitmapImage(new Uri(imagePath));
            }
            catch (Exception ex)
            {
                IsLoadingBigImage = false;
                LoadBigFailed = true;
                await MainWindow.Instance.ShowMessage("Error", ex.Message);
            }
            BigImageSource = tmp;
            IsLoadingBigImage= false;
        }
    }
}
