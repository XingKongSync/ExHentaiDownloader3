using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage.Pickers;

namespace ExHentaiDownloader3.ViewModels
{
    public class SettingsVM : BindableBase
    {
        public RelayCommand ChooseDownloadFolderCommand { get; private set; }

        public SettingsVM() 
        {
            ChooseDownloadFolderCommand = new RelayCommand(ChooseDownloadFolderCommandHandler);
        }

        private async void ChooseDownloadFolderCommandHandler()
        {
            FolderPicker fp = new FolderPicker();
            WinRT.Interop.InitializeWithWindow.Initialize(fp, Process.GetCurrentProcess().MainWindowHandle);
            var folderInfo = await fp.PickSingleFolderAsync();
            if (folderInfo is not null)
            {
                //TODO: ...
            }
        }
    }
}
