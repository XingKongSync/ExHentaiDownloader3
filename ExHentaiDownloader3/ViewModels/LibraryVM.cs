using CommunityToolkit.Mvvm.Input;
using ExHentaiDownloader3.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExHentaiDownloader3.ViewModels
{
    public enum LibraryMode
    {
        Library,
        LibraryItem,
        LibraryItemImage
    }

    public class LibraryVM : BindableBase
    {
        private List<LibraryItem> _libraryItemsSource;
        private LibraryItem _selectedLibraryItem;
        private LibraryMode _displayMode = LibraryMode.Library;
        private LibraryItemImage _selectedItemImage;
        
        private int _selectedItemImageIndex = -1;

        public List<LibraryItem> LibraryItemsSource { get => _libraryItemsSource; set => SetProperty(ref _libraryItemsSource, value); }
        public LibraryItem SelectedLibraryItem
        {
            get => _selectedLibraryItem;
            set
            {
                if (SetProperty(ref _selectedLibraryItem, value))
                {
                    if (value != null)
                    {
                        DisplayMode = LibraryMode.LibraryItem;
                    }
                }
            }
        }

        public LibraryItemImage SelectedItemImage
        {
            get => _selectedItemImage;
            set
            {
                if (SetProperty(ref _selectedItemImage, value))
                {
                    if (value != null)
                    {
                        DisplayMode = LibraryMode.LibraryItemImage;
                    }
                }
            }
        }

        public int SelectedItemImageIndex
        {
            get => _selectedItemImageIndex;
            set
            {
                if (SetProperty(ref _selectedItemImageIndex, value))
                {
                    PreImageCommand.NotifyCanExecuteChanged();
                    NextImageCommand.NotifyCanExecuteChanged();
                }
            }
        }

        public LibraryMode DisplayMode { get => _displayMode; set => SetProperty(ref _displayMode, value); }

        public RelayCommand RefreshCommand { get; private set; }
        public RelayCommand BackToLibraryCommand { get; private set; }
        public RelayCommand BackToLibraryItemCommand { get; private set; }
        public RelayCommand PreImageCommand { get; private set; }
        public RelayCommand NextImageCommand { get; private set; }


        public ConfigManager ConfigManager { get => ConfigManager.Instance; }
        public LibraryVM()
        {
            RefreshCommand = new RelayCommand(RefreshCommandHandler);
            BackToLibraryCommand = new RelayCommand(BackToLibraryCommandHandler);
            BackToLibraryItemCommand = new RelayCommand(BackToLibraryItemCommandHandler);
            PreImageCommand = new RelayCommand(PreImageCommandHandler, () => SelectedItemImageIndex > 0);
            NextImageCommand = new RelayCommand(NextImageCommandHandler, () => SelectedItemImageIndex < SelectedLibraryItem?.Images?.Count - 1);
        }
        private void RefreshCommandHandler()
        {
            LibraryManager.Instance.Refresh();
            LibraryItemsSource = LibraryManager.Instance.LibraryItems;
        }

        private void BackToLibraryItemCommandHandler()
        {
            SelectedItemImage = null;
            DisplayMode = LibraryMode.LibraryItem;
        }

        private void BackToLibraryCommandHandler()
        {
            SelectedLibraryItem = null;
            DisplayMode = LibraryMode.Library;
        }


        private void PreImageCommandHandler()
        {
            SelectedItemImageIndex = (SelectedItemImageIndex - 1) % SelectedLibraryItem?.Images?.Count ?? 1;
        }

        private void NextImageCommandHandler()
        {
            SelectedItemImageIndex = (SelectedItemImageIndex + 1) % SelectedLibraryItem?.Images?.Count ?? 1;
        }
    }

}
