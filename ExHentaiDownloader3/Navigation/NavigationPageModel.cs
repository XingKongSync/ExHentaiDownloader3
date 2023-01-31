using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExHentaiDownloader3.Navigation
{
    public class NavigationPageModel : BindableBase
    {
        private Symbol _icon;
        private string _title;
        private Type _pageType;
        private object _args;

        public Symbol Icon { get => _icon; set => SetProperty(ref _icon, value); }
        public string Title { get => _title; set => SetProperty(ref _title, value); }
        public Type PageType { get => _pageType; set => SetProperty(ref _pageType, value); }
        public object Args { get => _args; set => SetProperty(ref _args, value); }
    }
}
