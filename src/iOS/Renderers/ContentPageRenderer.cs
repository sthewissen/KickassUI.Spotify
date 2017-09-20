using System.Linq;
using CoreGraphics;
using KickassUI.Spotify.iOS.Helpers;
using KickassUI.Spotify.iOS.Renderers;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(ContentPage), typeof(ContentPageRenderer))]
namespace KickassUI.Spotify.iOS.Renderers
{
    public class ContentPageRenderer : PageRenderer
    {
        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);

            // Move navigation bar toolbar items over to the left if more than 1 is present.
            if (NavigationController == null)
                return;

            var navigationItem = NavigationController.TopViewController.NavigationItem;
            var leftNativeButtons = (navigationItem.LeftBarButtonItems ?? new UIBarButtonItem[] { }).ToList();
            var rightNativeButtons = (navigationItem.RightBarButtonItems ?? new UIBarButtonItem[] { }).ToList();

            if (rightNativeButtons.Count > 1)
            {
                var nativeItem = rightNativeButtons.Last();
                rightNativeButtons.Remove(nativeItem);
                leftNativeButtons.Add(nativeItem);
            }

            navigationItem.RightBarButtonItems = rightNativeButtons.ToArray();
            navigationItem.LeftBarButtonItems = leftNativeButtons.ToArray();

            ModalPresentationCapturesStatusBarAppearance = false;

            // Set the status bar to light.
            UIApplication.SharedApplication.SetStatusBarStyle(UIStatusBarStyle.LightContent, false);
        }

        public override void ViewWillDisappear(bool animated)
        {
            base.ViewWillDisappear(animated);

            // Set the status bar to light.
            UIApplication.SharedApplication.SetStatusBarStyle(UIStatusBarStyle.LightContent, false);
        }
    }
}
