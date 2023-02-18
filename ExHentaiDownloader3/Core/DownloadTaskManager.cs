using ExHentaiDownloader3.Core.Exhentai;
using ExHentaiDownloader3.ViewModels;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Media.Imaging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.Globalization.DateTimeFormatting;

namespace ExHentaiDownloader3.Core
{
    public class DownloadTaskManager : BindableBase
    {
        private static Lazy<DownloadTaskManager> _instance = new Lazy<DownloadTaskManager>(()=> new DownloadTaskManager());

        public static DownloadTaskManager Instance { get { return _instance.Value; } }

        public ObservableCollection<DownloadTask> Tasks { get; private set; } = new ObservableCollection<DownloadTask>();

        private SemaphoreSlim _semaphore = new SemaphoreSlim(5);

        private DownloadTaskManager() { }

        public void CreateNewTask(BookInfoVM info)
        {
            if (Tasks.Any(t=> t?.Url?.Equals(info?.Url) == true))
            {
                return;
            }

            var task = new DownloadTask(info, _semaphore);
            Tasks.Add(task);
            task.Start();
        }

        public void DeleteTask(DownloadTask task)
        {
            task.Stop();
            Tasks.Remove(task);
        }

    }

    public class DownloadTask : BindableBase
    {
        private BookInfoVM _info;
        private bool _isLoadingThumb = false;
        private BitmapImage _thumbSource;
        private bool _thumbLoadFailed = false;

        private SemaphoreSlim _semaphore = new SemaphoreSlim(5);

        private int _currentCount = 0;
        private int _errorCount = 0;
        private TaskStatus _status = TaskStatus.Waiting;

        private Task _downlaodTask;
        private CancellationTokenSource _cancelTokenSource; 

        public string Title { get => _info.Title; }
        public string Url { get => _info.Url; }
        public string ThumbUrl { get => _info.ThumbUrl; }
        public int ImageCount { get => _info.PageCount; }
        public int CurrentCount { get => _currentCount; set => SetProperty(ref _currentCount, value); }
        public TaskStatus Status { get => _status; set => SetProperty(ref _status, value); }
        public int ErrorCount { get => _errorCount; set => SetProperty(ref _errorCount, value); }

        public bool ThumbLoadFailed { get => _thumbLoadFailed; private set => SetProperty(ref _thumbLoadFailed, value); }

        public bool IsLoadingThumb
        {
            get => _isLoadingThumb;
            set => SetProperty(ref _isLoadingThumb, value);
        }

        public BitmapImage ThumbSource
        {
            get
            {
                if (_thumbSource is not null)
                    return _thumbSource;
                if (IsLoadingThumb)
                    return _thumbSource;

                UpdateThumbSource();
                return _thumbSource;
            }
            set => SetProperty(ref _thumbSource, value);
        }

        public DownloadTask(BookInfoVM info, SemaphoreSlim semaphore)
        {
            _info = info;
            _semaphore = semaphore;
        }

        public void Start()
        {
            if (_downlaodTask is not null && !_downlaodTask.IsCompleted)
                return;

            _cancelTokenSource = new CancellationTokenSource();
            _downlaodTask = Task.Run(DownloadTaskHandler, _cancelTokenSource.Token);
        }

        public void Stop()
        {
            if (_downlaodTask is null || _downlaodTask.IsCompleted)
                return;
            _cancelTokenSource?.Cancel();
        }

        private async void UpdateThumbSource()
        {
            IsLoadingThumb = true;
            BitmapImage tmp = null;
            try
            {
                string imagePath = await DownloadManager.Instance.DownloadThumb(ThumbUrl);
                tmp = new BitmapImage(new Uri(imagePath));
            }
            catch (Exception)
            {
                ThumbLoadFailed = true;
            }
            IsLoadingThumb = false;

            ThumbSource = tmp;
        }

        private void CreateInfoFile()
        {
            try
            {

            }
            catch (Exception)
            {
            }
        }

        private void IncreaseCurrentCount()
        {
            lock (this)
            {
                CurrentCount++;
            }
        }

        private void IncreaseErrorCount()
        {
            lock (this)
            {
                ErrorCount++;
            }
        }

        private void DownloadTaskHandler()
        {
            CancellationToken? token = _cancelTokenSource?.Token;
            if (token == null)
                return;

            bool waitSuccess = false;
            CurrentCount = 0;
            try
            {
                Status = TaskStatus.Waiting;
                _semaphore.Wait((CancellationToken)token);
                waitSuccess = true;

                LibraryManager.Instance.CreateLibraryInfoFile(_info);
                token.Value.ThrowIfCancellationRequested();

                Status = TaskStatus.Downloading;

                Parallel.ForEach(GetBigImageInfos(), new ParallelOptions() { MaxDegreeOfParallelism = 5 }, info =>
                {
                    token.Value.ThrowIfCancellationRequested();

                    BigImagePage bp = new BigImagePage(info.DetailPageUrl);
                    bp.Load().Wait();

                    token.Value.ThrowIfCancellationRequested();
                    try
                    {
                        string imagePath = DownloadManager.Instance.DownloadBigImage(Title, bp.ImageUrl, bp.NlImageUrl, info.Index).Result;
                        LibraryManager.Instance.CopyToLibrary(Title, imagePath);
                        IncreaseCurrentCount();
                    }
                    catch (Exception)
                    {
                        IncreaseErrorCount();
                    }
                });

                Status = TaskStatus.Finished;
            }
            catch (OperationCanceledException) 
            {
                Status = TaskStatus.Canceled;
            }
            catch (Exception)
            {
                Status = TaskStatus.Error;
            }
            finally
            {
                if (waitSuccess)
                    _semaphore.Release();
            }
        }

        private IEnumerable<BigImageInfoVM> GetBigImageInfos()
        {
            int pageSize;
            int pageCount;
            int currentPage = 0;
            BookPage bookPage = new BookPage(Url, ImageCount, Title);
            bookPage.Load(currentPage).Wait();
            pageSize = bookPage.PageSize;
            if (pageSize == 0)
                yield break;
            
            pageCount = bookPage.ImageCount / pageSize;
            pageCount += (bookPage.ImageCount % pageSize == 0 ? 0 : 1);

            for (int i = 0; i < pageCount; i++)
            {
                try
                {
                    if (i != 0)
                    {
                        bookPage.Load(i).Wait();
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"[DownloadTaskManager][GetBigImageInfos]Error: {ex.Message}");
                    continue;
                }
              
                if (bookPage.ImageInfos != null)
                {
                    foreach (var item in bookPage.ImageInfos)
                    {
                        yield return item;
                    }
                }
            }
        }
    }

    public enum TaskStatus
    {
        Waiting,
        Downloading,
        Finished,
        Canceled,
        //Suspend,
        Error
    }
}
