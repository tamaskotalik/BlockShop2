using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace BlockShop2
{

    public class VC_FT_To_int : IValueConverter
    {
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int ret = 0;
            var b = int.TryParse(value.ToString().Replace("Ft", "").Replace(" ", ""), out ret);
            return ret;
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value.ToString() + " Ft";
        }
    }
    public class VC_Percent_To_float : IValueConverter
    {
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            float ret = 0;
            var b = float.TryParse(value.ToString().Replace("%", "").Replace(" ", ""), out ret);
            return ret;
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value.ToString() + " %";
        }
    }

}
