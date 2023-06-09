﻿using CommunityToolkit.Mvvm.Input;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Media.Playback;

namespace ExHentaiDownloader3.ViewModels
{
    public class TabVM : BindableBase
    {
        private Symbol _icon;
        private string _title;
        private UIElement _view;
        private TabVM _parent;
        private ObservableCollection<TabVM> _children = new ObservableCollection<TabVM>();
        private RelayCommand _closeCommand;
        private bool _isExpanded = true;

        public Symbol Icon { get => _icon; set => SetProperty(ref _icon, value); }
        public string Title { get => _title; set => SetProperty(ref _title, value); }
        public UIElement View { get => _view; set => SetProperty(ref _view, value); }
        public ObservableCollection<TabVM> Children { get => _children; set => SetProperty(ref _children, value); }
        public TabVM Parent { get => _parent; set => SetProperty(ref _parent, value); }
        public RelayCommand CloseCommand { get => _closeCommand; set =>SetProperty(ref _closeCommand, value); }
        public bool IsExpanded { get => _isExpanded; set => SetProperty(ref _isExpanded, value); }

        public TabVM()
        {
            CloseCommand = new RelayCommand(CloseCommandHandler, () => Parent?.Children is not null);
        }

        private void CloseCommandHandler()
        {
            if (Parent?.Children is not null)
            {
                Parent.Children.Remove(this);
            }
            if (MainWindow.Instance.VM.SelectedTab == null)
            {
                MainWindow.Instance.VM.SelectedTab = Parent ?? MainWindow.Instance.VM.Tabs[0];
            }
        }
    }
}
