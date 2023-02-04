using CommunityToolkit.Mvvm.Input;
using ExHentaiDownloader3.Helpers;
using ExHentaiDownloader3.ViewModels;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExHentaiDownloader3.Views
{
    public static class TabFactory
    {
        public static TabVM CreateHomeTab()
        {
            return new TabVM()
            {
                Icon = Symbol.Home,
                Title = "Home",
                View = new HomeView(),
            };
        }

        public static TabVM CreateSearchResultTab(string searchText)
        {
            string url;
            string title;
            if (string.IsNullOrWhiteSpace(searchText))
            {
                url = UrlHelper.CONST_EXHENTAI_ROOT;
                title = "Latest Content";
            }
            else
            {
                url = string.Format(UrlHelper.CONST_EXHENTAI_SEARCH, Uri.EscapeDataString(searchText));
                title = $"Search: {searchText}";
            }

            TabVM tabVM = new TabVM
            {
                Icon = Microsoft.UI.Xaml.Controls.Symbol.Pictures,
                Title = title
            };
            SearchResultVM vm = new SearchResultVM(tabVM) 
            {
                Title = title,
                Url = url
            };
            tabVM.View = new SearchResultView() { ViewModel = vm };

            tabVM.CloseCommand = new RelayCommand(() => MainWindow.Instance.VM.CloseTab(tabVM));

            return tabVM;
        }

        public static TabVM CreateSettingsTab()
        {
            return new TabVM()
            {
                Icon = Symbol.Setting,
                Title = "Setting",
                View = new SettingsView()
            };
        }

        public static TabVM CreateBookTab(BookInfoVM info, TabVM parent)
        {
            return new TabVM()
            {
                Icon = Symbol.BrowsePhotos,
                Title = info.Title,
                Parent = parent,
                View = new BookView() { ViewModel = new BookVM() { BookInfo = info } }
            };
        }

        public static TabVM CreateLibraryTab()
        {
            return new TabVM()
            {
                Icon = Symbol.Library,
                Title = "Library",
                View = new LibraryView()
            };
        }
    }
}
