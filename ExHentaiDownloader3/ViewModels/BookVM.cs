using CommunityToolkit.Mvvm.Input;
using ExHentaiDownloader3.Core;
using ExHentaiDownloader3.Core.Exhentai;
using Microsoft.UI.Dispatching;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExHentaiDownloader3.ViewModels
{
    public class BookVM : BindableBase
    {
        private BookInfoVM _bookInfo;

        private int _currentPage;
        private int _pageCount;
        private List<BigImageInfoVM> _pages;
        private BigImageInfoVM _selectedPage;
        private int _selectedPageIndex = -1;

        private bool _isLoading = false;
        private BookPage _loader;

        public ConfigManager ConfigManager { get => ConfigManager.Instance; }

        public BookInfoVM BookInfo { get => _bookInfo; set => SetProperty(ref _bookInfo, value); }

        public int CurrentPage
        {
            get => _currentPage;
            set
            {
                if (SetProperty(ref _currentPage, value))
                {
                    PreCommand.NotifyCanExecuteChanged();
                    NextCommand.NotifyCanExecuteChanged();
                }
            }
        }

        public int PageCount
        {
            get => _pageCount;
            set
            {
                if (SetProperty(ref _pageCount, value))
                {
                    PreCommand.NotifyCanExecuteChanged();
                    NextCommand.NotifyCanExecuteChanged();
                }
            }
        }
        public List<BigImageInfoVM> Pages { get => _pages; set => SetProperty(ref _pages, value); }
        public BigImageInfoVM SelectedPage { get => _selectedPage; set => SetProperty(ref _selectedPage, value); }
        public int SelectedPageIndex
        {
            get => _selectedPageIndex;
            set
            {
                if (SetProperty(ref _selectedPageIndex, value))
                {
                    PreImageCommand.NotifyCanExecuteChanged();
                    NextImageCommand.NotifyCanExecuteChanged();

                    //预加载图片
                    PreloadNextImage();
                }
            }
        }

        public RelayCommand PreCommand { get; private set; }
        public RelayCommand NextCommand { get; private set; }

        public RelayCommand ImageBackCommand { get; private set; }
        public RelayCommand PreImageCommand { get; private set; }
        public RelayCommand NextImageCommand { get; private set; }

        public bool IsLoading { get => _isLoading; set => SetProperty(ref _isLoading, value); }

        public BookVM(BookInfoVM bookInfo)
        {
            _bookInfo = bookInfo;

            PreCommand = new RelayCommand(PreCommandHandler, () => CurrentPage > 0);
            NextCommand = new RelayCommand(NextCommantHandler, () => CurrentPage < PageCount - 1);
            ImageBackCommand = new RelayCommand(() => SelectedPage = null);

            PreImageCommand = new RelayCommand(PreImageCommandHandler, () => SelectedPageIndex > 0);
            NextImageCommand = new RelayCommand(NextImageCommandHandler, () => SelectedPageIndex < Pages?.Count - 1);

            DispatcherQueue.GetForCurrentThread().TryEnqueue(Load);
        }

        private void PreCommandHandler()
        {
            CurrentPage = (CurrentPage - 1) % PageCount;

            DispatcherQueue.GetForCurrentThread().TryEnqueue(Load);
        }

        private void NextCommantHandler()
        {
            CurrentPage = (CurrentPage + 1) % PageCount;

            DispatcherQueue.GetForCurrentThread().TryEnqueue(Load);
        }

        private void PreImageCommandHandler()
        {
            SelectedPageIndex = (SelectedPageIndex - 1) % Pages?.Count ?? 1;
        }

        private void NextImageCommandHandler()
        {
            SelectedPageIndex = (SelectedPageIndex + 1) % Pages?.Count ?? 1;
        }

        private async void Load()
        {
            IsLoading = true;

            Pages = null;

            try
            {
                 if (_loader is null)
                    _loader = new BookPage(_bookInfo.Url, _bookInfo.PageCount, _bookInfo.Title);
                await _loader.Load(CurrentPage);

                if (PageCount <= 0)
                {
                    int tmp = _loader.ImageCount / _loader.PageSize;
                    int tmp2 = _loader.ImageCount % _loader.PageSize;

                    PageCount = tmp2 > 0 ? tmp + 1 : tmp;
                }

                Pages = _loader.ImageInfos;
            }
            catch (Exception ex)
            {
                _ = MainWindow.Instance.ShowMessage("Error", ex.Message);
            }


            IsLoading = false;
        }

        private void PreloadNextImage()
        {
            TouchImage(SelectedPageIndex + 1);
            TouchImage(SelectedPageIndex - 1);
        }

        private void TouchImage(int index)
        {
            int count = Pages?.Count ?? 0;
            if (count <= 0) return;

            index = index % count;
            if (index >= 0 && index < count)
            {
                BigImageInfoVM vm = Pages[index];
                _ = vm.BigImageSource;
            }

        }
    }
}
