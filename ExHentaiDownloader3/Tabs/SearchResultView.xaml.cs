using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;

namespace ExHentaiDownloader3.Tabs
{
    public sealed partial class SearchResultView : Page
    {
        public SearchResultVM ViewModel { get => DataContext as SearchResultVM; set => DataContext = value; }
        
        public SearchResultView()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            if (ViewModel is null && e.Parameter is Parameters.SearchResultNavigateParameter param && param.ViewModel is not null)
            {
                ViewModel = param.ViewModel;
                return;
            }
            if (ViewModel is null)
            {
                ViewModel = new SearchResultVM();
            }
        }
    }
}
