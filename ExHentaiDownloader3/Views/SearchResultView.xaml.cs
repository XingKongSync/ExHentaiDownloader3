using Microsoft.UI.Xaml.Controls;
using ExHentaiDownloader3.ViewModels;
using System;
using Microsoft.UI.Xaml;

namespace ExHentaiDownloader3.Views
{
    public sealed partial class SearchResultView : UserControl
    {
        public SearchResultVM ViewModel { get => DataContext as SearchResultVM; set => DataContext = value; }

        public SearchResultView()
        {
            this.InitializeComponent();
        }

        private void Border_PointerReleased(object sender, Microsoft.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            var pointer = e.Pointer;
            if (pointer.PointerDeviceType == Microsoft.UI.Input.PointerDeviceType.Mouse && e.GetCurrentPoint(sender as UIElement).Properties.IsRightButtonPressed)
            {
                return;
            }
            DispatcherQueue.TryEnqueue(() => _ = termsOfUseContentDialog.ShowAsync());
        }
    }
}
