﻿using ExHentaiDownloader3.ViewModels;
using ExHentaiDownloader3.Views;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExHentaiDownloader3
{
    public class MainWindowVM : BindableBase
    {
        private TabVM _selectedTab;

        public string AppTitleText { get; set; } = "ExhentaiDownloader";
        public ObservableCollection<TabVM> Tabs { get; private set; } = new ObservableCollection<TabVM>();
        public TabVM SelectedTab { get => _selectedTab; set => SetProperty(ref _selectedTab, value); }

        public MainWindowVM() 
        {
            InitPages();
        }

        private void InitPages()
        {
            Tabs.Add(new TabVM() { Icon = Symbol.Home, Title = "Home", View = new HomeView() });

            SelectedTab = Tabs[0];
        }

        public void NewTab(TabVM tab)
        {
            Tabs.Add(tab);
            SelectedTab = tab;
        }
    }
}
