using ExHentaiDownloader3.ViewModels;
using Microsoft.UI.Xaml.Controls;

namespace ExHentaiDownloader3.Views
{
    public sealed partial class DownloadTaskView : UserControl
    {
        public DownloadTaskVM ViewModel { get => DataContext as DownloadTaskVM; set => DataContext = value; }

        public DownloadTaskView()
        {
            this.InitializeComponent();
        }
    }
}
