using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace SkyView.Utils
{
    public class AnchorToControlConverter : IValueConverter
    {
        object IValueConverter.Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(value is System.Windows.Point)

            {
                if (parameter is int && (int)parameter == 100)
                {
                    return (System.Windows.Point)value + new Vector(100, 0);
                }
           
               else if (parameter is int && (int)parameter == -100)
                {
                    return (System.Windows.Point)value + new Vector(-100, 0);
                }
            }

            return null;
        }


        object IValueConverter.ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
