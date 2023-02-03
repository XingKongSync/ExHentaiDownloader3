using CommunityToolkit.Mvvm.Input;
using ExHentaiDownloader3.Core;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage.Pickers;

namespace ExHentaiDownloader3.ViewModels
{
    public class SettingsVM : BindableBase, IConfig
    {
        public RelayCommand ChooseDownloadFolderCommand { get; private set; }
        public RelayCommand SaveCommand { get; private set; }
        public RelayCommand CancelCommand { get; private set; }

        private bool _hasModified = false;
        public bool HasModified { get => _hasModified; set => SetProperty(ref _hasModified, value); }

        #region Config Properties

        public string Cookies { get => GetCacheProperty<string>(); set => SetCacheProperty(value); }

        public bool EnableSound { get => GetCacheProperty<bool>(); set => SetCacheProperty(value); }
        
        public string LibraryFolder { get => GetCacheProperty<string>(); set => SetCacheProperty(value); }

        #endregion

        public SettingsVM() 
        {
            ChooseDownloadFolderCommand = new RelayCommand(ChooseDownloadFolderCommandHandler);
            SaveCommand = new RelayCommand(Submit);
            CancelCommand = new RelayCommand(Discard);
        }

        #region Cache Property Utils

        private Dictionary<string, object> _propertyCache = new Dictionary<string, object>();

        private void SetCacheProperty<T>(T value, [CallerMemberName] string propertyName = null)
        {
            T old = GetCacheProperty<T>(propertyName);

            if (old.Equals(value))
                return;

            _propertyCache[propertyName] = value;
            RaisePropertyChanged(propertyName);

            HasModified = true;
        }

        private T GetCacheProperty<T>([CallerMemberName] string propertyName = null)
        {
            if (_propertyCache.TryGetValue(propertyName, out object v))
            {
                return (T)v;
            }

            var property = typeof(ConfigEnitty).GetProperty(propertyName);
            return (T)property.GetValue(ConfigManager.Instance.Config);
        }

        private void Discard()
        {
            _propertyCache.Clear();

            HasModified = false;
        }

        private void Submit()
        {
            foreach (var pair in _propertyCache)
            {
                string propertyName = pair.Key;
                object value = pair.Value;

                var propertyInfo = typeof(ConfigEnitty).GetProperty(propertyName);
                propertyInfo.SetValue(ConfigManager.Instance.Config, value);
            }
            Discard();

            ConfigManager.Instance.Save();
        }

        #endregion

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
