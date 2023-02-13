// Copyright (c) Microsoft Corporation and Contributors.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System.Windows.Input;
using ExHentaiDownloader3.Core;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace ExHentaiDownloader3.Controls
{
    public sealed partial class ImageViewer : UserControl
    {


        public ImageSource ImageSource
        {
            get { return (ImageSource)GetValue(ImageSourceProperty); }
            set { SetValue(ImageSourceProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ImageSource.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ImageSourceProperty =
            DependencyProperty.Register("ImageSource", typeof(ImageSource), typeof(ImageViewer), new PropertyMetadata(null));


        public ICommand BackCommand
        {
            get { return (ICommand)GetValue(BackCommandProperty); }
            set { SetValue(BackCommandProperty, value); }
        }

        // Using a DependencyProperty as the backing store for BackCommand.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty BackCommandProperty =
            DependencyProperty.Register("BackCommand", typeof(ICommand), typeof(ImageViewer), new PropertyMetadata(null));


        public ConfigManager ConfigManager { get => ConfigManager.Instance; }

        public ImageViewer()
        {
            this.InitializeComponent();
        }

        private void btFullScreen_Click(object sender, RoutedEventArgs e)
        {
            zoom.ZoomMode = ZoomMode.Uniform;
        }
    }
}
