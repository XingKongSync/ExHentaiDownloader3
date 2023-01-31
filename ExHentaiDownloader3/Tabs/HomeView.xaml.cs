using Microsoft.UI.Xaml.Controls;
namespace ExHentaiDownloader3.Tabs
{
    public sealed partial class HomeView : Page
    {
        public HomeVM ViewModel { get => DataContext as HomeVM; set => DataContext = value; }

        public HomeView()
        {
            this.InitializeComponent();
            ViewModel = new HomeVM();
        }

    }
}
