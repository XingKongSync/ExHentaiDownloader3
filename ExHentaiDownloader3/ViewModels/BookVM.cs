using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExHentaiDownloader3.ViewModels
{
    public class BookVM : BindableBase
    {
        private BookInfoVM _bookInfo;

        public BookInfoVM BookInfo { get => _bookInfo; set => SetProperty(ref _bookInfo, value); }
    }
}
