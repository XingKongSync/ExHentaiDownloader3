using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using ExHentaiDownloader3.ViewModels;

namespace ExHentaiDownloader3.Views
{
    public sealed partial class BookView : UserControl
    {
        public BookVM ViewModel { get => DataContext as BookVM; set => DataContext = value; }

        public BookView()
        {
            this.InitializeComponent();
        }

    }
}
