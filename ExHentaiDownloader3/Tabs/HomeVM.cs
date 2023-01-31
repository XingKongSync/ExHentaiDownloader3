using CommunityToolkit.Mvvm.Input;
using ExHentaiDownloader3.Navigation;
using Microsoft.UI.Xaml.Documents;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ExHentaiDownloader3.Tabs
{
    public class HomeVM : BindableBase
    {
        private string _searchText = string.Empty;
        private ObservableCollection<string> _historySearchItems = new ObservableCollection<string>();

        public string SearchText { get => _searchText; set => SetProperty(ref _searchText, value); }
        public ObservableCollection<string> HistorySearchItems { get => _historySearchItems; set => SetProperty(ref _historySearchItems, value); }

        public RelayCommand<string> HistroyItemQueryCommand { get; private set; }

        public HomeVM()
        {
            for (int i = 0; i < 10; i++)
            {
                _historySearchItems.Add($"item{i}");
            }

            HistroyItemQueryCommand = new RelayCommand<string>(HistroyItemQueryCommandHandler);
        }

        private void HistroyItemQueryCommandHandler(string queryText)
        {
            SearchText = queryText;

            DoQuery();
        }

        public void DoQuery()
        {
            NavigationRootPage.Instance.OpenNewPage(new NavigationPageModel()
            {
                Title = $"Search: {SearchText}",
                Icon = Microsoft.UI.Xaml.Controls.Symbol.Pictures,
                PageType = typeof(SearchResultView),
                Args = new Parameters.SearchResultNavigateParameter()
                {
                    ViewModel = new SearchResultVM()
                    {
                        Title = SearchText
                    }
                }
            });
        }
    }
}
