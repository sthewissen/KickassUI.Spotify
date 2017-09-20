using System;
using System.Drawing;
using UIKit;

namespace KickassUI.Spotify.iOS.Helpers
{
    public static class Extensions
    {
        public static UIImage ToUIImage(this UIColor color)
        {
            var imageSize = new SizeF(30, 30);
            var imageSizeRectF = new RectangleF(0, 0, 30, 30);
            UIGraphics.BeginImageContextWithOptions(imageSize, false, 0);
            var context = UIGraphics.GetCurrentContext();

            context.SetFillColor(color.CGColor);
            context.FillRect(imageSizeRectF);
            var image = UIGraphics.GetImageFromCurrentImageContext();
            UIGraphics.EndImageContext();
            return image;
        }
    }
}
