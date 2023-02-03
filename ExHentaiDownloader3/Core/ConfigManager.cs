using Newtonsoft.Json;
using System;
using System.Collections.ObjectModel;
using System.IO;

namespace ExHentaiDownloader3.Core
{
    public class ConfigManager : BindableBase
    {
        private readonly string CONST_CONFIG_FILE_FULL_PATH = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), @"ExHentaiDownloader3\Config.json");
        private static readonly Lazy<ConfigManager> _instance = new Lazy<ConfigManager>(() => new ConfigManager());
        private ConfigEnitty _config;

        public static ConfigManager Instance { get { return _instance.Value; } }

        public ConfigEnitty Config { get => _config; set => SetProperty(ref _config, value); }

        private ConfigManager()
        {
            MainWindow.Instance.Closed += MainWindow_Closed;
            Load();
            if (_config is null)
            {
                _config = new ConfigEnitty();
            }
        }

        private void MainWindow_Closed(object sender, Microsoft.UI.Xaml.WindowEventArgs args)
        {
            if (HistoryManager.Instance.NeedSave)
            {
                Save();
            }
        }

        private void Load()
        {
            try
            {
                if (File.Exists(CONST_CONFIG_FILE_FULL_PATH))
                {
                    string configJson = File.ReadAllText(CONST_CONFIG_FILE_FULL_PATH);
                    _config = JsonConvert.DeserializeObject<ConfigEnitty>(configJson);
                }
            }
            catch (Exception)
            {
            }
        }

        public void Save()
        {
            try
            {
                Directory.CreateDirectory(Path.GetDirectoryName(CONST_CONFIG_FILE_FULL_PATH));
                File.WriteAllText(CONST_CONFIG_FILE_FULL_PATH, JsonConvert.SerializeObject(Config));
            }
            catch (Exception)
            {
            }
        }

    }

    public class ConfigEnitty : BindableBase, IConfig
    {
        private string _cookies = string.Empty;
        private bool _enableSound;
        private string _downloadFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), @"ExHentaiDownloader3\Library");
        private ObservableCollection<string> _history = new ObservableCollection<string>();

        public string Cookies { get => _cookies; set => SetProperty(ref _cookies, value); }
        public bool EnableSound { get => _enableSound; set => SetProperty(ref _enableSound, value); }
        public string LibraryFolder { get => _downloadFolder; set => SetProperty(ref _downloadFolder, value); }
        public ObservableCollection<string> History { get => _history; set => SetProperty(ref _history, value); }
    }

    public interface IConfig
    {
        string Cookies { get; set; }
        bool EnableSound { get; set; }
        string LibraryFolder { get; set; }
    }
}
