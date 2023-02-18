using ExHentaiDownloader3.Helpers;
using ExHentaiDownloader3.ViewModels;
using Microsoft.UI.Xaml.Media.Imaging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExHentaiDownloader3.Core
{
    public class LibraryManager : BindableBase
    {
        private static Lazy<LibraryManager> _libraryManager = new Lazy<LibraryManager>(() => new LibraryManager());

        public static LibraryManager Instance { get => _libraryManager.Value; }

        private DateTime _lastLibraryTime;
        private List<LibraryItem> _libraryItems;

        public List<LibraryItem> LibraryItems { get => _libraryItems; set => SetProperty(ref _libraryItems, value); }

        private LibraryManager() { }

        public string GetCleanFullBookPath(string bookName)
        {
            bookName = FileNameHelper.CleanFileName(bookName);
            string fullBookPath = Path.Combine(ConfigManager.Instance.Config.LibraryFolder, bookName);
            return fullBookPath;
        }

        public void CreateLibraryInfoFile(BookInfoVM info)
        {
            string fullBookPath = GetCleanFullBookPath(info.Title);
            Directory.CreateDirectory(fullBookPath);

            string infoPath = Path.Combine(fullBookPath, "info.json");
            string infoJson = JsonConvert.SerializeObject(info, Formatting.Indented);
            File.WriteAllText(infoPath, infoJson);

            string thumbTmpPath = DownloadManager.Instance.DownloadThumb(info.ThumbUrl).Result;
            string thumbExt = Path.GetExtension(thumbTmpPath);
            string thumbPath = Path.Combine(fullBookPath, $"Folder{thumbExt}");
            if (!File.Exists(thumbPath))
                File.Copy(thumbTmpPath, thumbPath, true);
        }

        public void CopyToLibrary(string bookName, string fullFilePath)
        {
            string fullBookPath = GetCleanFullBookPath(bookName);

            Directory.CreateDirectory(fullBookPath);

            string fileName = Path.GetFileName(fullFilePath);
            string dst = Path.Combine(fullBookPath, fileName);

            if (!File.Exists(dst))
                File.Copy(fullFilePath, dst, true);
        }

        public void Refresh()
        {
            try
            {
                List<LibraryItem> list = new List<LibraryItem>();
                string[] subFolders = Directory.GetDirectories(ConfigManager.Instance.Config.LibraryFolder);
                foreach (string folder in subFolders)
                {
                    list.Add(new LibraryItem(folder));
                }
                _lastLibraryTime = DateTime.Now;
                LibraryItems = list;
            }
            catch (Exception)
            {
            }
        }
    }

    public class LibraryItem : BindableBase
    {
        private string _folderFullPath;

        //private DateTime _updateTime;
        //private string _title;
        //private string _thumbPath;
        private WeakReference<List<LibraryItemImage>> _images;
        private WeakReference<BitmapImage> _thumbImageSource;

        public DateTime UpdateTime { get; set; }
        public string Title { get; set; }
        public string ThumbPath { get; set; }

        public List<LibraryItemImage> Images
        {
            get
            {
                if (_images?.TryGetTarget(out var result) == true)
                {
                    return result;
                }

                var list = LoadImages();
                _images = new WeakReference<List<LibraryItemImage>>(list);
                return list;
            }
        }

        public BitmapImage ThumbImageSource
        {
            get
            {
                if (_thumbImageSource?.TryGetTarget(out var result) == true)
                {
                    return result;
                }
                if (string.IsNullOrWhiteSpace(ThumbPath))
                    return null;

                BitmapImage source = new BitmapImage(new Uri(ThumbPath));
                _thumbImageSource = new WeakReference<BitmapImage>(source);
                return source;
            }
        }

        public LibraryItem(string folderFullPath)
        {
            _folderFullPath = folderFullPath;

            Title = Path.GetFileName(folderFullPath);

            var dirInfo = new DirectoryInfo(folderFullPath);
            ThumbPath = dirInfo.GetFiles("Folder.*")?.FirstOrDefault()?.FullName;
        }

        private List<LibraryItemImage> LoadImages()
        {
            List<LibraryItemImage> list = new List<LibraryItemImage>();
            DirectoryInfo dirInfo = new DirectoryInfo(_folderFullPath);
            var files = dirInfo.GetFiles();
            if (files != null)
            {
                foreach (var file in files)
                {
                    string ext = file.Extension?.ToLower();
                    if (string.IsNullOrWhiteSpace(ext)
                        || !(ext.Equals(".jpg") || ext.Equals(".png")))
                    {
                        continue;
                    }
                    if (file.Name?.ToLower()?.StartsWith("folder") != false)
                        continue;
                    list.Add(new LibraryItemImage(file.FullName));
                }
            }
            return list;
        }
    }

    public class LibraryItemImage : BindableBase
    {
        private string _imagePath;
        private BitmapImage _imageSource;

        public string ImagePath { get => _imagePath; set => _imagePath = value; }
        public BitmapImage ImageSource
        {
            get
            {
                if (_imageSource == null && !string.IsNullOrWhiteSpace(_imagePath))
                {
                    try
                    {
                        BitmapImage source = new BitmapImage(new Uri(_imagePath));
                        _imageSource = source;
                    }
                    catch (Exception)
                    {
                    }
                }
                return _imageSource;
            }
            private set => SetProperty(ref _imageSource, value);
        }

        public LibraryItemImage(string imagePath)
        {
            _imagePath = imagePath;
        }
    }
}
