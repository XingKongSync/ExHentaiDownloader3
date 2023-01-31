using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExHentaiDownloader3.Converters
{
    public class IconConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is Symbol sym)
            {
                return new SymbolIcon(sym);
            }
            //if (value is string str && Enum.TryParse(typeof(Symbol), str, out object tmp))
            //{
            //    return new SymbolIcon((Symbol)tmp);
            //}
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
