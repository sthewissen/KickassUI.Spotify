using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using Android.Content;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.Renderscripts;
using Android.Widget;
using KickassUI.Spotify.Droid.Renderers;
using KickassUI.Spotify.Controls;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(BlurredImage), typeof(BlurredImageRenderer))]
namespace KickassUI.Spotify.Droid.Renderers
{
    public class BlurredImageRenderer : ViewRenderer<BlurredImage, ImageView>
    {
        private bool _isDisposed;

        private const float BITMAP_SCALE = 0.3f;
        private const float RESIZE_SCALE = 0.2f;

        public BlurredImageRenderer()
        {
            AutoPackage = false;
        }

        protected override void OnElementChanged(ElementChangedEventArgs<BlurredImage> e)
        {
            base.OnElementChanged(e);

            if (Control == null)
            {
                var imageView = new BlurredImageView(Context);
                SetNativeControl(imageView);
            }

            UpdateBitmap(e.OldElement);
            UpdateAspect();
        }

        protected override void OnElementPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            if (e.PropertyName == Image.SourceProperty.PropertyName)
            {
                UpdateBitmap(null);
                return;
            }
            if (e.PropertyName == Image.AspectProperty.PropertyName)
            {
                UpdateAspect();
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (_isDisposed)
            {
                return;
            }
            _isDisposed = true;
            BitmapDrawable bitmapDrawable;
            if (disposing && Control != null && (bitmapDrawable = (Control.Drawable as BitmapDrawable)) != null)
            {
                Bitmap bitmap = bitmapDrawable.Bitmap;
                if (bitmap != null)
                {
                    bitmap.Recycle();
                    bitmap.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        private void UpdateAspect()
        {
            using (ImageView.ScaleType scaleType = ToScaleType(Element.Aspect))
            {
                Control.SetScaleType(scaleType);
            }
        }

        private static ImageView.ScaleType ToScaleType(Aspect aspect)
        {
            switch (aspect)
            {
                case Aspect.AspectFill:
                    return ImageView.ScaleType.CenterCrop;
                case Aspect.Fill:
                    return ImageView.ScaleType.FitXy;
            }
            return ImageView.ScaleType.FitCenter;
        }

        private async void UpdateBitmap(Image previous = null)
        {
            Bitmap bitmap = null;
            ImageSource source = Element.Source;
            if (previous == null || !object.Equals(previous.Source, Element.Source))
            {
                SetIsLoading(true);
                ((BlurredImageView)base.Control).SkipInvalidate();

                // I'm not sure where this comes from.
                Control.SetImageResource(17170445);
                if (source != null)
                {
                    try
                    {
                        bitmap = await GetImageFromImageSource(source, Context);
                    }
                    catch (TaskCanceledException)
                    {
                    }
                    catch (IOException)
                    {
                    }
                    catch (NotImplementedException)
                    {
                    }
                }
                if (Element != null && object.Equals(Element.Source, source))
                {
                    if (!_isDisposed)
                    {
                        Control.SetImageBitmap(bitmap);
                        if (bitmap != null)
                        {
                            bitmap.Dispose();
                        }
                        SetIsLoading(false);
                        ((IVisualElementController)base.Element).NativeSizeChanged();
                    }
                }
            }
        }

        private async Task<Bitmap> GetImageFromImageSource(ImageSource imageSource, Context context)
        {
            IImageSourceHandler handler;

            if (imageSource is FileImageSource)
            {
                handler = new FileImageSourceHandler();
            }
            else if (imageSource is StreamImageSource)
            {
                handler = new StreamImagesourceHandler(); // sic
            }
            else if (imageSource is UriImageSource)
            {
                handler = new ImageLoaderSourceHandler(); // sic
            }
            else
            {
                throw new NotImplementedException();
            }

            var originalBitmap = await handler.LoadImageAsync(imageSource, context);

            // Blur it twice!
            var blurredBitmap = await Task.Run(() => CreateBlurredImage(CreateResizedImage(originalBitmap), 25));

            return blurredBitmap;
        }

        private Bitmap CreateBlurredImage(Bitmap originalBitmap, int radius)
        {
            // Create another bitmap that will hold the results of the filter.
            Bitmap blurredBitmap;
            blurredBitmap = Bitmap.CreateBitmap(originalBitmap);

            // Create the Renderscript instance that will do the work.
            RenderScript rs = RenderScript.Create(Context);

            // Allocate memory for Renderscript to work with
            Allocation input = Allocation.CreateFromBitmap(rs, originalBitmap, Allocation.MipmapControl.MipmapFull, AllocationUsage.Script);
            Allocation output = Allocation.CreateTyped(rs, input.Type);

            // Load up an instance of the specific script that we want to use.
            ScriptIntrinsicBlur script = ScriptIntrinsicBlur.Create(rs, Android.Renderscripts.Element.U8_4(rs));
            script.SetInput(input);

            // Set the blur radius
            script.SetRadius(radius);

            // Start Renderscript working.
            script.ForEach(output);

            // Copy the output to the blurred bitmap
            output.CopyTo(blurredBitmap);

            return blurredBitmap;
        }

        private Bitmap CreateResizedImage(Bitmap originalBitmap)
        {
            int width = Convert.ToInt32(Math.Round(originalBitmap.Width * BITMAP_SCALE));
            int height = Convert.ToInt32(Math.Round(originalBitmap.Height * BITMAP_SCALE));

            // Create another bitmap that will hold the results of the filter.
            Bitmap inputBitmap = Bitmap.CreateScaledBitmap(originalBitmap, width, height, false);
            Bitmap outputBitmap = Bitmap.CreateBitmap(inputBitmap);

            // Create the Renderscript instance that will do the work.
            RenderScript rs = RenderScript.Create(Context);


            Allocation tmpIn = Allocation.CreateFromBitmap(rs, inputBitmap);
            Allocation tmpOut = Allocation.CreateFromBitmap(rs, outputBitmap);

            // Allocate memory for Renderscript to work with
            var t = Android.Renderscripts.Type.CreateXY(rs, tmpIn.Element, Convert.ToInt32(width * RESIZE_SCALE), Convert.ToInt32(height * RESIZE_SCALE));
            Allocation tmpScratch = Allocation.CreateTyped(rs, t);

            ScriptIntrinsicResize theIntrinsic = ScriptIntrinsicResize.Create(rs);

            // Resize the original img down.
            theIntrinsic.SetInput(tmpIn);
            theIntrinsic.ForEach_bicubic(tmpScratch);

            // Resize smaller img up.
            theIntrinsic.SetInput(tmpScratch);
            theIntrinsic.ForEach_bicubic(tmpOut);
            tmpOut.CopyTo(outputBitmap);

            return outputBitmap;
        }

        private class BlurredImageView : ImageView
        {
            private bool _skipInvalidate;

            public BlurredImageView(Context context) : base(context)
            {
            }

            public override void Invalidate()
            {
                if (this._skipInvalidate)
                {
                    this._skipInvalidate = false;
                    return;
                }
                base.Invalidate();
            }

            public void SkipInvalidate()
            {
                this._skipInvalidate = true;
            }
        }

        private static FieldInfo _isLoadingPropertyKeyFieldInfo;

        private static FieldInfo IsLoadingPropertyKeyFieldInfo
        {
            get
            {
                if (_isLoadingPropertyKeyFieldInfo == null)
                {
                    _isLoadingPropertyKeyFieldInfo = typeof(Image).GetField("IsLoadingPropertyKey", BindingFlags.Static | BindingFlags.NonPublic);
                }
                return _isLoadingPropertyKeyFieldInfo;
            }
        }

        private void SetIsLoading(bool value)
        {
            var fieldInfo = IsLoadingPropertyKeyFieldInfo;
            ((IElementController)base.Element).SetValueFromRenderer((BindablePropertyKey)fieldInfo.GetValue(null), value);
        }
    }
}
