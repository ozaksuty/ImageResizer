using Android.Graphics;
using ImageResizer.Droid;
using System.IO;
using Xamarin.Forms;

[assembly: Dependency(typeof(MediaService))]
namespace ImageResizer.Droid
{
    class MediaService : IMediaService
    {
        public byte[] ResizeImage(byte[] imageData, float width, float height)
        {
            try
            {
                BitmapFactory.Options options = new BitmapFactory.Options()
                {
                    InPurgeable = true,
                };
                Bitmap originalImage = BitmapFactory.DecodeByteArray(imageData, 0, imageData.Length, options);
                return CreateImage(originalImage, width, height);
            }
            catch (System.Exception ex)
            {
                return imageData;
            }
        }

        public byte[] ResizeImage(string imagePath, float width, float height)
        {
            try
            {
                BitmapFactory.Options options = new BitmapFactory.Options()
                {
                    InPurgeable = true,
                };
                Bitmap originalImage = BitmapFactory.DecodeFile(imagePath, options);
                return CreateImage(originalImage, width, height);
            }
            catch (System.Exception ex)
            {
                return new byte[0];
            }
        }

        private byte[] CreateImage(Bitmap bitmap, float width, float height)
        {
            float newHeight = 0;
            float newWidth = 0;

            var originalHeight = bitmap.Height;
            var originalWidth = bitmap.Width;

            if (originalHeight > originalWidth)
            {
                newHeight = height;
                float ratio = originalHeight / height;
                newWidth = originalWidth / ratio;
            }
            else
            {
                newWidth = width;
                float ratio = originalWidth / width;
                newHeight = originalHeight / ratio;
            }

            Bitmap resizedImage = Bitmap.CreateScaledBitmap(bitmap, (int)newWidth, (int)newHeight, true);
            bitmap.Recycle();

            using (MemoryStream ms = new MemoryStream())
            {
                resizedImage.Compress(Bitmap.CompressFormat.Png, 100, ms);
                resizedImage.Recycle();
                return ms.ToArray();
            }
        }
    }
}