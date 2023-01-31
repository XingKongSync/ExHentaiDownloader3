using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExHentaiDownloader3.ViewModels
{
    public class SearchResultVM : BindableBase
    {
        private string _url;
        private string _title;
        private int _count;
        private string _firstUrl;
        private string _lastUrl;
        private string _preUrl;
        private string _nextUrl;

        public string Url { get => _url; set => SetProperty(ref _url, value); }
        public string Title { get => _title; set => SetProperty(ref _title, value); }
        public int Count { get => _count; set => SetProperty(ref _count, value); }
        public string FirstUrl { get => _firstUrl; set => SetProperty(ref _firstUrl, value); }
        public string LastUrl { get => _lastUrl; set => SetProperty(ref _lastUrl, value); }
        public string PreUrl { get => _preUrl; set => SetProperty(ref _preUrl, value); }
        public string NextUrl { get => _nextUrl; set => SetProperty(ref _nextUrl, value); }
    }
}
