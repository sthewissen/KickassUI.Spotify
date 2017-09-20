using System;
using Xamarin.Forms;

namespace KickassUI.Spotify.Controls
{
    public class AudioSlider : Slider
    {
        public static readonly BindableProperty HasThumbProperty =
            BindableProperty.Create(nameof(HasThumb), typeof(bool), typeof(HorizontalScrollView), true);

        public bool HasThumb
        {
            get { return (bool)GetValue(HasThumbProperty); }
            set { SetValue(HasThumbProperty, value); }
        }
    }
}
