using System;
using CoreGraphics;
using KickassUI.Spotify.Controls;
using KickassUI.Spotify.iOS.Renderers;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(AudioSlider), typeof(AudioSliderRenderer))]
namespace KickassUI.Spotify.iOS.Renderers
{
    public class AudioSliderRenderer : SliderRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Slider> e)
        {
            base.OnElementChanged(e);

            if (Control != null)
            {
                Control.MinimumTrackTintColor = UIColor.White;
                Control.MaximumTrackTintColor = UIColor.FromRGBA(173, 174, 178, 40);

                if (e.NewElement != null && (e.NewElement as AudioSlider).HasThumb)
                {
                    Control.SetThumbImage(UIImage.FromBundle("small_slider"), UIControlState.Normal);
                }
                else
                {
                    Control.SetThumbImage(new UIImage(), UIControlState.Normal);
                }
            }
        }
    }
}
