using System.Linq;
using CoreGraphics;
using KickassUI.Spotify.iOS.Helpers;
using KickassUI.Spotify.iOS.Renderers;
using KickassUI.Spotify.Controls;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(ModalPage), typeof(ModalPageRenderer))]
namespace KickassUI.Spotify.iOS.Renderers
{
    public class ModalPageRenderer : PageRenderer
    {
        private string _title;
        private string _subtitle;
        private UIView _statusBarUnderlay = new UIView();

        protected override void OnElementChanged(VisualElementChangedEventArgs e)
        {
            base.OnElementChanged(e);

            _title = (e.NewElement as ModalPage).Title;
            _subtitle = (e.NewElement as ModalPage).Subtitle;
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);

            // Move navigation item over to the left
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

            // Remove the blurview for these pages.
            var blurView = this.NavigationController.NavigationBar.ViewWithTag(74619);
            if (blurView != null) blurView.RemoveFromSuperview();

            NavigationController.NavigationBar.TintColor = UIColor.White;
            NavigationController.NavigationBar.SetBackgroundImage(new UIImage(), UIBarMetrics.Default);
            NavigationController.NavigationBar.ShadowImage = new UIImage();
            NavigationController.NavigationBar.BarStyle = UIBarStyle.Black;
            NavigationController.NavigationBar.Translucent = true;

            ModalPresentationCapturesStatusBarAppearance = true;


            //var constraint = View.TopAnchor.ConstraintEqualTo(TopLayoutGuide.GetBottomAnchor(), 20);
            //NavigationController.NavigationBar.AddConstraint(constraint);
        }


        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            //.ActivateConstraints(constraint);

            //_statusBarUnderlay.BackgroundColor = UIColor.Black;
            //_statusBarUnderlay.Frame = new CGRect(0, -20, UIScreen.MainScreen.Bounds.Size.Width, 20);
            //View.AddSubview(_statusBarUnderlay);
        }

        public override void WillMoveToParentViewController(UIViewController parent)
        {
            base.WillMoveToParentViewController(parent);

            var page = (ContentPage)Element;

            if (!string.IsNullOrEmpty(page.Title))
            {
                // Add the title
                var titleView = new UIView(new CGRect(0, 0, 150, 44));
                var firstFrame = new CGRect(0, 5, 150, 22);
                var secondFrame = new CGRect(0, 20, 150, 22);

                var firstLabel = new UILabel(firstFrame)
                {
                    TextColor = UIColor.White,
                    Font = UIFont.FromName("CircularSpotifyTxT-Book", 10),
                    Text = _title,
                    TextAlignment = UITextAlignment.Center
                };

                var secondLabel = new UILabel(secondFrame)
                {
                    Font = UIFont.FromName("CircularSpotifyTxT-Book", 13),
                    TextColor = UIColor.White,
                    Text = _subtitle,
                    TextAlignment = UITextAlignment.Center
                };

                titleView.AddSubview(firstLabel);
                titleView.AddSubview(secondLabel);

                parent.NavigationItem.TitleView = titleView;
            }
        }

        public override void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);

            // Slide out our navigation bar
            UIApplication.SharedApplication.SetStatusBarHidden(true, UIStatusBarAnimation.Slide);
        }

        public override void ViewWillDisappear(bool animated)
        {
            base.ViewWillDisappear(animated);

            // Slide in our navigation bar
            UIApplication.SharedApplication.SetStatusBarHidden(false, UIStatusBarAnimation.Slide);
        }
    }
}
