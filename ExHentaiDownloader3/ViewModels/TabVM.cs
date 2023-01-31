using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExHentaiDownloader3.ViewModels
{
    public class TabVM : BindableBase
    {
        private Symbol _icon;
        private string _title;
        private UIElement _view;
        private ObservableCollection<TabVM> _children;

        public Symbol Icon { get => _icon; set => SetProperty(ref _icon, value); }
        public string Title { get => _title; set => SetProperty(ref _title, value); }
        public UIElement View { get => _view; set => SetProperty(ref _view, value); }
        public ObservableCollection<TabVM> Children { get => _children; set => SetProperty(ref _children, value); }
    }
}
