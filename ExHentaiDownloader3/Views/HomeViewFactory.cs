using ExHentaiDownloader3.ViewModels;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExHentaiDownloader3.Views
{
    public static class HomeViewFactory
    {
        public static TabVM CreateTab()
        {
            return new TabVM() 
            {
                Icon = Symbol.Home, 
                Title = "Home", 
                View = new HomeView() ,
            };
        }
    }
}
