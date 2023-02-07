using AngleSharp.Html.Dom;
using AngleSharp.Html.Parser;
using ExHentaiDownloader3.Helpers;
using ExHentaiDownloader3.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ExHentaiDownloader3.Core.Exhentai
{
    class BookPage
    {
        private string _rootUrl;
        private List<BigImageInfoVM> _imageInfos;
        private string _rawHtml;
        private int _currentPage;//start from 0
        private int _pageSize;
        private int _imageCount;
        private CookieCollection _cookies = ConfigManager.Instance.Config.CookieCollection;


        public string BookName { get; private set; }
        /// <summary>
        /// https://exhentai.org/g/2457292/4cbc83af05/
        /// </summary>
        public string RootUrl { get => _rootUrl; private set => _rootUrl = value; }
        public List<BigImageInfoVM> ImageInfos { get => _imageInfos; private set => _imageInfos = value; }
        public int PageSize { get => _pageSize; set => _pageSize = value; }
        public int ImageCount { get => _imageCount; set => _imageCount = value; }

        public BookPage(string rootUrl, int imageCount, string bookName)
        {
            _rootUrl = rootUrl;
            ImageCount = imageCount;
            BookName = bookName;
        }

        public async Task Load(int currentPage)
        {
            _currentPage = currentPage;

            await LoadHtmlAsync();
            HtmlParser parser = new HtmlParser();
            IHtmlDocument document = await parser.ParseDocumentAsync(_rawHtml);

            if (PageSize <= 0)
                LoadPageSize(document);
            LoadBigImageInfo(document);
        }

        private async Task LoadHtmlAsync()
        {
            string url = _currentPage == 0 ? RootUrl : string.Format(UrlHelper.CONST_EXHENTAI_BOOK_PAGE_FORMAT, RootUrl, _currentPage);
            _rawHtml = await HttpHelper.GetHtmlWithCookiesAsync(url, _cookies);
        }

        private void LoadPageSize(IHtmlDocument document)
        {
            PageSize = document.QuerySelectorAll(".gdtl").Count();
        }

        private void LoadBigImageInfo(IHtmlDocument document)
        {
            ImageInfos = new List<BigImageInfoVM>();

            var imageInfoDoms = document.QuerySelectorAll(".gdtl");
            int count = imageInfoDoms.Count();
            for (int i = 0; i < count; i++)
            {
                var dom = imageInfoDoms[i];
                ImageInfos.Add(new BigImageInfoVM()
                {
                    Index = i + PageSize * _currentPage,
                    BookName = BookName,
                    DetailPageUrl = dom.QuerySelector("a").GetAttribute("href"),
                    ThumbUrl = dom.QuerySelector("img").GetAttribute("src")
                });
            }
        }
    }
}
