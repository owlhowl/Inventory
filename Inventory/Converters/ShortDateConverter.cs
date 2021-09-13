using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows.Data;

namespace MyInventory.Converters
{
    public class ShortDateConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var date = (DateTime)value;
            if (date == new DateTime())
                return "Не указано";
            return date.ToShortDateString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string stringDate = (string)value;
            string[] ddmmyyyy = stringDate.Split('/');
            int day = int.Parse(ddmmyyyy[0]);
            int month = int.Parse(ddmmyyyy[1]);
            int year = int.Parse(ddmmyyyy[2]);
            DateTime date;
            try
            {
                date = new DateTime(year, month, day);
            }
            catch (ArgumentOutOfRangeException)
            {
                return new DateTime();
            }
            return date;
        }
    }
}
