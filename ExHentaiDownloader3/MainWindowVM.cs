using ExHentaiDownloader3.ViewModels;
using ExHentaiDownloader3.Views;
using System;
using System.Collections.ObjectModel;
using System.Globalization;

namespace ExHentaiDownloader3
{
    public class MainWindowVM : BindableBase
    {
        private TabVM _selectedTab;

        public string AppTitleText { get; set; } = "ExhentaiDownloader";
        public ObservableCollection<TabVM> Tabs { get; private set; } = new ObservableCollection<TabVM>();
        public ObservableCollection<TabVM> FooterTabs { get; private set; } = new ObservableCollection<TabVM>();
        public TabVM SelectedTab { get => _selectedTab; set => SetProperty(ref _selectedTab, value); }

        public TabVM DownloadTab { get; private set; }

        public MainWindowVM() 
        {
            InitPages();
        }

        private void InitPages()
        {
            Tabs.Add(TabFactory.CreateHomeTab());

            DownloadTab = TabFactory.CreateDownloadTaskTab();

            FooterTabs.Add(DownloadTab);
            FooterTabs.Add(TabFactory.CreateLibraryTab());
            FooterTabs.Add(TabFactory.CreateSettingsTab());

            SelectedTab = Tabs[0];
        }

        public void NewTab(TabVM tab)
        {
            Tabs.Add(tab);
            SelectedTab = tab;
        }

        public void CloseTab(TabVM tab)
        {
            if (SelectedTab == tab)
            {
                int index = Tabs.IndexOf(tab);
                int temp = index - 1;
                if (temp >= 0 && temp <= Tabs.Count - 1)
                {
                    SelectedTab = Tabs[temp];
                }
                else
                {
                    SelectedTab = Tabs[0];
                }
            }
            Tabs.Remove(tab);
            if (SelectedTab == null)
            {
                SelectedTab = Tabs[0];
            }
        }
    }
}
