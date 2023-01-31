using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExHentaiDownloader3.Views
{
    public static class SearchResultViewFactory
    {
        public class Result
        {
            public string Title { get; set; }
            public SearchResultView View { get; set; }
        }

        public static Result Create(string searchText)
        {
            searchText = $"Search: {searchText}";

            return new Result { Title = searchText, View = new SearchResultView() { DataContext = new ViewModels.SearchResultVM() { Title = searchText } } };
        }
    }

}
