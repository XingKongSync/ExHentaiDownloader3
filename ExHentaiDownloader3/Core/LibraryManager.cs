using ExHentaiDownloader3.Helpers;
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

        private LibraryManager() { }

        public void CopyToLibrary(string bookName, string fullFilePath)
        {
            bookName = FileNameHelper.CleanFileName(bookName);
            string fullBookPath = Path.Combine(ConfigManager.Instance.Config.LibraryFolder, bookName);

            Directory.CreateDirectory(fullBookPath);

            string fileName = Path.GetFileName(fullBookPath);
            string dst = Path.Combine(fullBookPath, fileName);

            File.Copy(fullFilePath, dst, true);
        }
    }
}
