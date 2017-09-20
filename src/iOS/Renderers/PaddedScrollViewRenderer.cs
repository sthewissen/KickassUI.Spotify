using System;
using KickassUI.Spotify.Controls;
using KickassUI.Spotify.iOS.Renderers;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(PaddedScrollView), typeof(PaddedScrollViewRenderer))]
namespace KickassUI.Spotify.iOS.Renderers
{
    public class PaddedScrollViewRenderer : ScrollViewRenderer
    {
        public override void LayoutSubviews()
        {
            base.LayoutSubviews();

            // An inset at the bottom because else we will have too much space there.
            ContentInset = new UIEdgeInsets(0, 0, -60, 0);
        }
    }
}
