// Copyright (c) Microsoft Corporation and Contributors.
// Licensed under the MIT License.

using ExHentaiDownloader3.Tabs;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System.Collections.ObjectModel;

namespace ExHentaiDownloader3.Navigation
{
    public sealed partial class NavigationRootPage : Page
    {
        public static NavigationRootPage Instance { get; private set; }

        public MainWindow MainWindow { get; set; }

        public string AppTitleText { get; set; } = "ExhentaiDownloader";

        public ObservableCollection<NavigationPageModel> Tabs { get; private set; } = new ObservableCollection<NavigationPageModel>();

        public NavigationRootPage()
        {
            this.InitializeComponent();
            Instance = this;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            MainWindow.Title = AppTitleText;
            MainWindow.ExtendsContentIntoTitleBar = true;
            MainWindow.SetTitleBar(AppTitleBar);

            InitPages();
        }

        private void InitPages()
        {
            Tabs.Add(new NavigationPageModel() { Icon = Symbol.Home, Title = "Home", PageType = typeof(HomeView) });

            NavigationViewControl.SelectedItem = Tabs[0];
        }

        private void NavigationViewControl_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
        {
            if (args.SelectedItem is NavigationPageModel pageModel)
            {
                rootFrame.Navigate(pageModel.PageType, pageModel.Args);
            }
        }

        public void OpenNewPage(NavigationPageModel pageModel)
        {
            Tabs.Add(pageModel);
            NavigationViewControl.SelectedItem = pageModel;
        }
    }
}
