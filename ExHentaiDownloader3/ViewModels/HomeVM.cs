using CommunityToolkit.Mvvm.Input;
using ExHentaiDownloader3.Views;
using System.Collections.ObjectModel;

namespace ExHentaiDownloader3.ViewModels
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
            var result = SearchResultViewFactory.Create(SearchText);
            MainWindow.Instance.VM.NewTab(new TabVM()
            {
                Icon = Microsoft.UI.Xaml.Controls.Symbol.Pictures,
                Title = result.Title,
                View = result.View
            });
        }
    }
}
