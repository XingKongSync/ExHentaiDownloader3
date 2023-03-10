using Microsoft.UI.Xaml.Controls;
using ExHentaiDownloader3.ViewModels;

namespace ExHentaiDownloader3.Views
{
    public sealed partial class HomeView : UserControl
    {
        public HomeVM ViewModel { get => DataContext as HomeVM; set => DataContext = value; }

        public HomeView()
        {
            this.InitializeComponent();
            ViewModel = new HomeVM();
        }

        private void abSearch_QuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
        {
            ViewModel.DoQuery();
        }
    }
}
