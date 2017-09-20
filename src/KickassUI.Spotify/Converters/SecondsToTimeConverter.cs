using System;
using System.Globalization;
using Xamarin.Forms;

namespace KickassUI.Spotify.Converters
{
    public class SecondsToTimeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int)
            {
                var seconds = TimeSpan.FromSeconds((int)value);

                return $"{seconds.ToString(@"mm\:ss")}";
            }

            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
