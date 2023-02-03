using CommunityToolkit.Mvvm.Input;
using ExHentaiDownloader3.Core;
using ExHentaiDownloader3.Views;
using System.Collections.ObjectModel;

namespace ExHentaiDownloader3.ViewModels
{
    public class HomeVM : BindableBase
    {
        private string _searchText = string.Empty;

        public string SearchText { get => _searchText; set => SetProperty(ref _searchText, value); }
        public ObservableCollection<string> HistorySearchItems { get => HistoryManager.Instance.History; }

        public RelayCommand<string> HistroyItemQueryCommand { get; private set; }
        public RelayCommand QueryCommand { get; private set; }
        public RelayCommand ClearCommand { get; private set; }

        public HomeVM()
        {
            HistroyItemQueryCommand = new RelayCommand<string>(HistroyItemQueryCommandHandler);
            QueryCommand = new RelayCommand(DoQuery);
            ClearCommand = new RelayCommand(ClearCommandHandler);
        }

        private void HistroyItemQueryCommandHandler(string queryText)
        {
            SearchText = queryText;

            DoQuery();
        }

        public void DoQuery()
        {
            var tab = TabFactory.CreateSearchResultTab(SearchText);
            MainWindow.Instance.VM.NewTab(tab);

            HistoryManager.Instance.AddItem(SearchText);

            SearchText = string.Empty;
        }

        private void ClearCommandHandler()
        {
            HistoryManager.Instance.Clear();
        }
    }
}
