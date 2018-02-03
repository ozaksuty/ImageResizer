using ImageResizer.iOS;
using System;
using System.Drawing;
using UIKit;
using Xamarin.Forms;

[assembly: Dependency(typeof(MediaService))]
namespace ImageResizer.iOS
{
    class MediaService : IMediaService
    {
        public byte[] ResizeImage(byte[] imageData, float width, float height)
        {
            UIImage originalImage = ImageFromByteArray(imageData);
            return CreateImage(originalImage, width, height);
        }

        public byte[] ResizeImage(string imagePath, float width, float height)
        {
            UIImage originalImage = ImageFormPath(imagePath);
            return CreateImage(originalImage, width, height);
        }

        private UIImage ImageFromByteArray(byte[] data)
        {
            if (data == null)
                return null;

            return new UIImage(Foundation.NSData.FromArray(data));
        }

        private UIImage ImageFormPath(string path)
        {
            if (String.IsNullOrEmpty(path))
                return null;

            return new UIImage(path);
        }

        private byte[] CreateImage(UIImage image, float width, float height)
        {
            var originalHeight = image.Size.Height;
            var originalWidth = image.Size.Width;

            nfloat newHeight = 0;
            nfloat newWidth = 0;

            if (originalHeight > originalWidth)
            {
                newHeight = height;
                nfloat ratio = originalHeight / height;
                newWidth = originalWidth / ratio;
            }
            else
            {
                newWidth = width;
                nfloat ratio = originalWidth / width;
                newHeight = originalHeight / ratio;
            }

            width = (float)newWidth;
            height = (float)newHeight;

            UIGraphics.BeginImageContext(new SizeF(width, height));
            image.Draw(new RectangleF(0, 0, width, height));
            var resizedImage = UIGraphics.GetImageFromCurrentImageContext();
            UIGraphics.EndImageContext();

            var bytesImagen = resizedImage.AsJPEG().ToArray();
            resizedImage.Dispose();
            return bytesImagen;
        }
    }
}