using AngleSharp.Html.Dom;
using AngleSharp.Html.Parser;
using ExHentaiDownloader3.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ExHentaiDownloader3.Core.Exhentai
{
    class BigImagePage
    {
        private string _url;
        private string _rawHtml;
        private string _imageUrl;
        private CookieCollection _cookies = ConfigManager.Instance.Config.CookieCollection;

        public string ImageUrl { get => _imageUrl; set => _imageUrl = value; }

        public BigImagePage(string url)
        {
            _url = url;
        }

        public async Task Load()
        {
            await LoadHtmlAsync();
        }
        private async Task LoadHtmlAsync()
        {
            _rawHtml = await HttpHelper.GetHtmlWithCookiesAsync(_url, _cookies);

            IHtmlDocument document = await new HtmlParser().ParseDocumentAsync(_rawHtml);
            _imageUrl = document.QuerySelector("#img").GetAttribute("src");
        }
    }
}
