using MyInventory.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Text;
using System.Windows.Data;

namespace MyInventory.Converters
{
    public class ExcludeNotSelectedConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            List<Profile> profiles = new List<Profile>();

            if (value is ObservableCollection<Profile>)
                profiles = new List<Profile>((ObservableCollection<Profile>)value);
            else if (value is List<Profile>)
                profiles = (List<Profile>)value;

            profiles.Remove(profiles.Find((p) => p.ID == 0));
            return profiles;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
