using System;
using System.Globalization;
using Microsoft.Maui.Controls;

namespace Informatics.MauiSurdeg.Converters 
{
    public class InvertBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool b)
                return !b; // Inverts the boolean value

            return true; // Default case
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool b)
                return !b;

            return true;
        }
    }
}

