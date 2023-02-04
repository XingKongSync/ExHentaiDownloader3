using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ExHentaiDownloader3.Core
{
    public class DownloadManager
    {
        private static readonly Lazy<DownloadManager> _instance = new Lazy<DownloadManager>(() => new DownloadManager());

        private static readonly string CONST_THUMB_CACHE_PATH = Path.Combine(Path.GetTempPath(), @"ExhentaiDownloader3\Thumbs");

        private static SemaphoreSlim _semaphore = new SemaphoreSlim(10);

        public static DownloadManager Instance { get { return _instance.Value; } }

        private DownloadManager() { }

        public async Task<string> DownloadThumb(string url)
        {
            Directory.CreateDirectory(CONST_THUMB_CACHE_PATH);

            Exception lastError = null;

            await _semaphore.WaitAsync();
            for (int i = 0; i < 10; i++)
            {
                try
                {
                    _semaphore.Release();
                    return await DownloadIfNotExistAsync(url, CONST_THUMB_CACHE_PATH);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("Retry Count: " + i);
                    lastError = ex;
                }
                await Task.Delay(1000);
            }
            _semaphore.Release();
            throw new Exception("Cannot download thumb", lastError);
        }

        private static async Task<string> DownloadIfNotExistAsync(string url, string directory)
        {
            Uri uri = new Uri(url);
            string fileName = Path.GetFileName(uri.LocalPath);
            DirectoryInfo d = new DirectoryInfo(directory);
            string fileFullPathAndName = Path.Combine(d.FullName, fileName);
            if (!File.Exists(fileFullPathAndName))
            {
                //说明此文件不存在，则创建
                HttpClientHandler handler = new HttpClientHandler();
                if (handler.CookieContainer == null)
                {
                    handler.CookieContainer = new System.Net.CookieContainer();
                }
                var cookieCollection = ConfigManager.Instance.Config.CookieCollection;
                for (int i = 0; i < cookieCollection.Count; i++)
                {
                    handler.CookieContainer.Add(cookieCollection[i]);
                }

                HttpClient client = new HttpClient(handler);
                var stream = await client.GetStreamAsync(url);
                var fs = File.Create(fileFullPathAndName);
                using (stream)
                {
                    using (fs)
                    {
                        stream.CopyTo(fs);
                    }
                }
                //await (referUrl, url, Path.Combine(directory, fileName));
            }
            Debug.WriteLine("Cache hit: " + fileFullPathAndName);
            return fileFullPathAndName;
        }
    }
}
