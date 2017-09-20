using System;
using Android.Graphics.Drawables;
using KickassUI.Spotify.Controls;
using KickassUI.Spotify.Droid.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(AudioSlider), typeof(AudioSliderRenderer))]
namespace KickassUI.Spotify.Droid.Renderers
{
    public class AudioSliderRenderer : SliderRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Slider> e)
        {
            base.OnElementChanged(e);

            if (Control != null && e.NewElement != null)
            {
                Control.SetPadding(0, 0, 0, 0);
                Control.SetPaddingRelative(0, 0, 0, 0);

                // Set custom drawable resource
                Control.SetProgressDrawableTiled(Resources.GetDrawable(Resource.Drawable.custom_slider, (this.Context).Theme));

                // Hide thumb
                if (!(e.NewElement as AudioSlider).HasThumb)
                {
                    Control.SetThumb(new ColorDrawable(Android.Graphics.Color.Transparent));
                }
            }
        }
    }
}
