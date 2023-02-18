using CommunityToolkit.Mvvm.Input;
using ExHentaiDownloader3.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExHentaiDownloader3.ViewModels
{
    public class DownloadTaskVM : BindableBase
    {
        public DownloadTaskManager Manager { get => DownloadTaskManager.Instance; }
        public ConfigManager ConfigManager { get => ConfigManager.Instance; }

        public RelayCommand<DownloadTask> CancelTaskCommand { get; private set; }

        public DownloadTaskVM()
        {
            CancelTaskCommand = new RelayCommand<DownloadTask>(CancelTaskCommandHandler);
        }

        private void CancelTaskCommandHandler(DownloadTask task)
        {
            Manager.DeleteTask(task);
        }
    }
}
