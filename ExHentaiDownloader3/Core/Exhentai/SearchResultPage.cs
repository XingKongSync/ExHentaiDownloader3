using AngleSharp.Dom;
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
    class SearchResultPage
    {
        private string _url;
        private string _rawHtml;
        private IHtmlDocument _document;
        private List<BookInfoVM> _bookInfos;
        private int _count;
        private string _firstUrl;
        private string _lastUrl;
        private string _preUrl;
        private string _nextUrl;

        private CookieCollection _cookies = ConfigManager.Instance.Config.CookieCollection;

        public List<BookInfoVM> BookInfos { get => _bookInfos; set => _bookInfos = value; }
        public int Count { get => _count; set => _count = value; }
        public string FirstUrl { get => _firstUrl; set => _firstUrl = value; }
        public string LastUrl { get => _lastUrl; set => _lastUrl = value; }
        public string PreUrl { get => _preUrl; set => _preUrl = value; }
        public string NextUrl { get => _nextUrl; set => _nextUrl = value; }

        public async Task LoadPage(string url)
        {
            _url = url;

            await TryLoadHtmlAsync();
            HtmlParser parser = new HtmlParser();
            _document = await parser.ParseDocumentAsync(_rawHtml);

            TryLoadCount(_document);
            TryLoadAllPagerUrl(_document);

            TryLoadBookInfo(_document);
        }

        private async Task TryLoadHtmlAsync()
        {
            try
            {
                _ = await HttpHelper.GetHtmlWithCookiesAsync(UrlHelper.CONST_EXHENTAI_SET_PAGE_FORMAT, _cookies);
            }
            catch (Exception ex)
            {
            }
            try
            {
                _rawHtml = await HttpHelper.GetHtmlWithCookiesAsync(_url, _cookies);
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void TryLoadCount(IHtmlDocument document)
        {
            try
            {
                string rawCount = _document.QuerySelector(".searchtext > p").TextContent;
                Count = GetInt(rawCount);
            }
            catch (Exception)
            {
            }
        }

        private void TryLoadAllPagerUrl(IHtmlDocument document)
        {
            TryLoadPageUrl(document, "ufirst", ref _firstUrl);
            TryLoadPageUrl(document, "uprev", ref _preUrl);
            TryLoadPageUrl(document, "unext", ref _nextUrl);
            TryLoadPageUrl(document, "ulast", ref _lastUrl);
        }

        private void TryLoadPageUrl(IHtmlDocument document, string id, ref string localField)
        {
            var element = document.QuerySelector($"#{id}");
            if (element?.TagName?.Equals("A") == true)
            {
                localField = element.GetAttribute("href");
            }
        }

        private void TryLoadBookInfo(IHtmlDocument document)
        {
            //var bookInfoDoms = document.All.Where(m => m.LocalName == "tr" && "1".Equals(m.GetAttribute("data-new")));
            var bookInfoDoms = document.QuerySelector("table.gltc").QuerySelectorAll("tr").ToList();
            if (bookInfoDoms.Count > 0)
            {
                bookInfoDoms.RemoveAt(0);
            }

            BookInfos = new List<BookInfoVM>();
            foreach (var dom in bookInfoDoms)
            {
                try
                {
                    //Title
                    BookInfoVM vm = new BookInfoVM();
                    vm.Title = dom.QuerySelector(".glink").TextContent;
                    vm.Url = dom.QuerySelector("a").GetAttribute("href");
                    vm.PageCount = GetInt(dom.QuerySelector(".glhide").TextContent);
                    vm.Tags = LoadTags(dom);
                    vm.ThumbUrl = dom.QuerySelector("img")?.GetAttribute("data-src") ?? dom.QuerySelector("img")?.GetAttribute("src");
                    BookInfos.Add(vm);
                }
                catch (Exception)
                {
                }
            }
        }

        private List<string> LoadTags(IElement bookInfoDom)
        {
            var tags = new List<string>();

            var temp = bookInfoDom?.QuerySelectorAll(".gt");
            if (temp == null)
                return tags;
            foreach (var tagDom in temp)
            {
                string tag = tagDom.GetAttribute("title");
                if (!string.IsNullOrWhiteSpace(tag))
                {
                    tags.Add(tag);
                }
            }
            return tags;
        }

        private int GetInt(string text)
        {
            StringBuilder sb = new StringBuilder();
            foreach (char c in text)
            {
                if (c >= '0' && c <= '9')
                {
                    sb.Append(c);
                }
            }
            return int.Parse(sb.ToString());
        }
    }
}
