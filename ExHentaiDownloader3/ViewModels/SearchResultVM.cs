using CommunityToolkit.Mvvm.Input;
using ExHentaiDownloader3.Core;
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
        
        public string FirstUrl 
        {
            get => _firstUrl; 
            set
            {
                if (SetProperty(ref _firstUrl, value))
                    FirstCommand.NotifyCanExecuteChanged();
            }
        }

        public string LastUrl 
        {
            get => _lastUrl;
            set
            {
                if (SetProperty(ref _lastUrl, value))
                    LastComand.NotifyCanExecuteChanged();
            }
        }

        public string PreUrl
        {
            get => _preUrl;
            set
            {
                if (SetProperty(ref _preUrl, value))
                    PreCommand.NotifyCanExecuteChanged();
            }
        }

        public string NextUrl
        {
            get => _nextUrl;
            set
            {
                if (SetProperty(ref _nextUrl, value))
                    NextCommand.NotifyCanExecuteChanged();
            }
        }
        public ObservableCollection<BookInfoVM> Books { get => _books; set => SetProperty(ref _books, value); }
        public BookInfoVM SelectedBook { get => _selectedBook; set => SetProperty(ref _selectedBook, value); }
        public RelayCommand<BookInfoVM> OpenBookCommand { get; private set; }
        public RelayCommand<BookInfoVM> OpenBookInBackgroundCommand { get; private set; }
        public RelayCommand<BookInfoVM> DownloadCommand { get; private set; }
        public RelayCommand FirstCommand { get; private set; }
        public RelayCommand LastComand { get; private set; }
        public RelayCommand PreCommand { get; private set; }
        public RelayCommand NextCommand { get; private set; }

        public ConfigManager ConfigManager { get => ConfigManager.Instance; }

        public bool IsLoading { get => _isLoading; set => SetProperty(ref _isLoading, value); }
        
        public TabVM MyTab { get; private set; }

        public SearchResultVM(TabVM tabVM)
        {
            MyTab = tabVM;
            OpenBookCommand = new RelayCommand<BookInfoVM>(OpenBookCommandHandler);
            OpenBookInBackgroundCommand = new RelayCommand<BookInfoVM>(OpenBookInBackgroundCommandHandler);
            DownloadCommand = new RelayCommand<BookInfoVM>(DownloadCommandHandler);
            FirstCommand = new RelayCommand(FirstCommandHandler, ()=> !string.IsNullOrWhiteSpace(FirstUrl));
            LastComand = new RelayCommand(LastComandHandler, ()=> !string.IsNullOrWhiteSpace(LastUrl));
            PreCommand = new RelayCommand(PreCommandHandler, ()=> !string.IsNullOrWhiteSpace(PreUrl));
            NextCommand = new RelayCommand(NextCommandHandler, ()=> !string.IsNullOrWhiteSpace(NextUrl));

            DispatcherQueue.GetForCurrentThread().TryEnqueue(Load);
        }

        private void DownloadCommandHandler(BookInfoVM obj)
        {
            DownloadTaskManager.Instance.CreateNewTask(obj);
            MainWindow.Instance.VM.SelectedTab = MainWindow.Instance.VM.DownloadTab;
        }

        private void FirstCommandHandler()
        {
            if (string.IsNullOrWhiteSpace(FirstUrl))
                return;
            Url = FirstUrl;
            DispatcherQueue.GetForCurrentThread().TryEnqueue(Load);
        }

        private void LastComandHandler()
        {
            if (string.IsNullOrWhiteSpace(LastUrl))
                return;
            Url = LastUrl;
            DispatcherQueue.GetForCurrentThread().TryEnqueue(Load);
        }

        private void PreCommandHandler()
        {
            if (string.IsNullOrWhiteSpace(PreUrl))
                return;
            Url = PreUrl;
            DispatcherQueue.GetForCurrentThread().TryEnqueue(Load);
        }

        private void NextCommandHandler()
        {
            if (string.IsNullOrWhiteSpace(NextUrl))
                return;
            Url = NextUrl;
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
            MyTab.IsExpanded = true;
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
