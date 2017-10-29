using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using ZalandoShop.Models;

namespace ZalandoShop.Converters
{
    class FacetConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            Facet facet = value as Facet;
            return facet;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
