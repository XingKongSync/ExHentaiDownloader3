using CommunityToolkit.Mvvm.Input;
using ExHentaiDownloader3.Core.Exhentai;
using ExHentaiDownloader3.Views;
using Microsoft.UI.Dispatching;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        private ObservableCollection<BookInfoVM> _books;
        private BookInfoVM _selectedBook;

        private bool _isLoading = false;
        private SearchResultPage _loader = new SearchResultPage();

        public string Url { get => _url; set => SetProperty(ref _url, value); }
        public string Title { get => _title; set => SetProperty(ref _title, value); }
        public int Count { get => _count; set => SetProperty(ref _count, value); }
        public string FirstUrl { get => _firstUrl; set => SetProperty(ref _firstUrl, value); }
        public string LastUrl { get => _lastUrl; set => SetProperty(ref _lastUrl, value); }
        public string PreUrl { get => _preUrl; set => SetProperty(ref _preUrl, value); }
        public string NextUrl { get => _nextUrl; set => SetProperty(ref _nextUrl, value); }
        public ObservableCollection<BookInfoVM> Books { get => _books; set => SetProperty(ref _books, value); }
        public BookInfoVM SelectedBook { get => _selectedBook; set => SetProperty(ref _selectedBook, value); }
        public RelayCommand<BookInfoVM> OpenBookCommand { get; private set; }
        public RelayCommand<BookInfoVM> OpenBookInBackgroundCommand { get; private set; }
        
        public bool IsLoading { get => _isLoading; set => SetProperty(ref _isLoading, value); }
        
        public TabVM MyTab { get; private set; }

        public SearchResultVM(TabVM tabVM)
        {
            MyTab = tabVM;
            OpenBookCommand = new RelayCommand<BookInfoVM>(OpenBookCommandHandler);
            OpenBookInBackgroundCommand = new RelayCommand<BookInfoVM>(OpenBookInBackgroundCommandHandler);

            DispatcherQueue.GetForCurrentThread().TryEnqueue(Load);
        }

        private void OpenBookCommandHandler(BookInfoVM info)
        {
            var bookVM = TabFactory.CreateBookTab(info, MyTab);
            MyTab.Children.Add(bookVM);

            MyTab.IsExpanded = true;
            MainWindow.Instance.VM.SelectedTab = bookVM;
        }

        private void OpenBookInBackgroundCommandHandler(BookInfoVM info)
        {
            var bookVM = TabFactory.CreateBookTab(info, MyTab);
            if (MyTab.Children is null)
            {
                MyTab.Children = new ObservableCollection<TabVM>();
            }
            MyTab.Children.Add(bookVM);
        }

        private async void Load()
        {
            IsLoading = true;

            try
            {
                await _loader.LoadPage(Url);

                Count = _loader.Count;
                FirstUrl = _loader.FirstUrl;
                LastUrl = _loader.LastUrl;
                PreUrl = _loader.PreUrl;
                NextUrl = _loader.NextUrl;
                Books = new ObservableCollection<BookInfoVM>(_loader.BookInfos);
            }
            catch (Exception ex)
            {
                _ = MainWindow.Instance.ShowMessage("Error", ex.Message);
            }

            IsLoading = false;
        }
    }
}
