using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExHentaiDownloader3.Core
{
    public class LibraryManager : BindableBase
    {
        private static Lazy<LibraryManager> _libraryManager = new Lazy<LibraryManager>(() => new LibraryManager());

        public static LibraryManager Instance { get => _libraryManager.Value; }

    }
}
