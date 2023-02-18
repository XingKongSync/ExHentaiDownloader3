using Newtonsoft.Json;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Net;

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
        private CookieCollection _cookieCollection;
        private string _cookies = string.Empty;
        private bool _enableSound;
        private string _libraryFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), @"ExHentai");
        private bool _useMosaic = false;
        private ObservableCollection<string> _history = new ObservableCollection<string>();

        public string Cookies 
        {
            get => _cookies;
            set
            {
                if (SetProperty(ref _cookies, value))
                {
                    _cookieCollection = null;
                }
            }
        }
        public bool EnableSound { get => _enableSound; set => SetProperty(ref _enableSound, value); }
        public string LibraryFolder { get => _libraryFolder; set => SetProperty(ref _libraryFolder, value); }
        public bool UseMosaic { get => _useMosaic; set => SetProperty(ref _useMosaic, value); }
        public ObservableCollection<string> History { get => _history; set => SetProperty(ref _history, value); }

        [JsonIgnore]
        public CookieCollection CookieCollection 
        {
            get
            {
                if (_cookieCollection == null && !string.IsNullOrEmpty(_cookies))
                {
                    try
                    {
                        CookieEntity[] cookieItems = JsonConvert.DeserializeObject<CookieEntity[]>(_cookies);
                        _cookieCollection = CookieEntity.GetCookieCollection(cookieItems);
                        return _cookieCollection;
                    }
                    catch (Exception)
                    {
                    }
                }
                return _cookieCollection;
            }
        }
    }

    public interface IConfig
    {
        string Cookies { get; set; }
        bool EnableSound { get; set; }
        string LibraryFolder { get; set; }
        bool UseMosaic { get; set; }
    }
}
