using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExHentaiDownloader3.ViewModels
{
    public class BookInfoVM : BindableBase
    {
        private string _title = $"Title{new Random().Next(10)}";
        private List<string> _tags = new List<string> { "tag1", "tag2", "tag3"};
        private int _pageCount = new Random().Next(100);

        public string Title { get => _title; set => SetProperty(ref _title, value); }
        public List<string> Tags { get => _tags; set => SetProperty(ref _tags, value); }
        public int PageCount { get => _pageCount; set => SetProperty(ref _pageCount, value); }
    }
}
