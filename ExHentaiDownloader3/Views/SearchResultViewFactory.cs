using CommunityToolkit.Mvvm.Input;
using ExHentaiDownloader3.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExHentaiDownloader3.Views
{
    public static class SearchResultViewFactory
    {
        public static TabVM CreateTab(string searchText)
        {
            searchText = $"Search: {searchText}";

            SearchResultVM vm = new SearchResultVM() { Title = searchText };

            TabVM tabVM = new TabVM
            {
                Icon = Microsoft.UI.Xaml.Controls.Symbol.Pictures,
                Title = searchText, 
                View = new SearchResultView() 
                { 
                    ViewModel = vm
                },
            };
            tabVM.CloseCommand = new RelayCommand(()=> MainWindow.Instance.VM.CloseTab(tabVM));

            return tabVM;
        }
    }

}
