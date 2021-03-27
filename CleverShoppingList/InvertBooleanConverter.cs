using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace CleverShoppingList
{
    //[ValueConversion(typeof(bool), typeof(bool))]
    class InvertBooleanConverter :IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter,
            System.Globalization.CultureInfo culture)
        {
            if (targetType != typeof(bool))
                throw new InvalidOperationException("Target must be a boolean");

            return !(bool)value;
        }

        public object ConvertBack(object value, Type targetType, object parameter,
            System.Globalization.CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
