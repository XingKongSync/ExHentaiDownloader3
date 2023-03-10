using Microsoft.UI.Xaml.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExHentaiDownloader3.Converters
{
    public class LanguageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (parameter is not string l10n)
                l10n = parameter?.ToString();
            if (l10n is null)
                l10n = value?.ToString();

            if (string.IsNullOrEmpty(l10n))
                return default;

            try
            {
                if (typeof(Properties.Resources).GetProperty(l10n).GetValue(null) is string text)
                {
                    return text;
                }
            }
            catch (Exception)
            {
            }
            return parameter;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
