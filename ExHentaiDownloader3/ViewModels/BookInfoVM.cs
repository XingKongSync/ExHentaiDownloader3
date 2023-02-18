using ExHentaiDownloader3.Core;
using Microsoft.UI.Xaml.Media.Imaging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExHentaiDownloader3.ViewModels
{
    public class BookInfoVM : BindableBase
    {
        private string _url = string.Empty;
        private string _title = string.Empty;
        private List<string> _tags = new List<string>();
        private int _pageCount = 0;
        private string _thumbUrl = string.Empty;
        private bool _isLoadingThumb = false;
        private BitmapImage _thumbSource;
        private bool _loadFailed = false;

        public string Url { get => _url; set => SetProperty(ref _url, value); }
        public string Title { get => _title; set => SetProperty(ref _title, value); }
        public List<string> Tags { get => _tags; set => SetProperty(ref _tags, value); }
        public int PageCount { get => _pageCount; set => SetProperty(ref _pageCount, value); }
        public string ThumbUrl { get => _thumbUrl; set => SetProperty(ref _thumbUrl, value); }
        [JsonIgnore]
        public bool LoadFailed { get => _loadFailed; private set => SetProperty(ref _loadFailed, value); }

        [JsonIgnore]
        public bool IsLoadingThumb 
        {
            get => _isLoadingThumb;
            set
            { 
                if (SetProperty(ref _isLoadingThumb, value))
                    RaisePropertyChanged(nameof(LoadFailed));
            }
        }

        [JsonIgnore]
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
