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
        private string _nlImageUrl;
        private CookieCollection _cookies = ConfigManager.Instance.Config.CookieCollection;

        public string ImageUrl { get => _imageUrl; set => _imageUrl = value; }
        public string NlImageUrl { get => _nlImageUrl; set => _nlImageUrl = value; }

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
            var imgDom = document.QuerySelector("#img");
            _imageUrl = imgDom.GetAttribute("src");
            string onerror = imgDom.GetAttribute("onerror");
            if (!string.IsNullOrWhiteSpace(onerror))
            {
                int startIndex = onerror.IndexOf("nl('");
                int endIndex = onerror.IndexOf("')");
                if (startIndex > 0 && endIndex > 0 && startIndex < endIndex)
                {
                    string nl = onerror.Substring(startIndex + 4, endIndex - startIndex - 4);
                    if (!string.IsNullOrWhiteSpace(nl))
                    {
                        string nlUrl = $"{_url}?nl={nl}";
                        BigImagePage nlPage = new BigImagePage(nlUrl);
                        await nlPage.Load();
                        NlImageUrl = nlPage.ImageUrl;
                    }
                }
            }
        }
    }
}
