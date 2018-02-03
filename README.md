# Xamarin Forms Resim Boyutlandırma
* http://ozaksut.com/xamarin-forms-resim-islemleri/
* Interface
<pre><code class='language-cs'>
    public interface IMediaService
    {
        byte[] ResizeImage(byte[] imageData, float width, float height);
        byte[] ResizeImage(string imagePath, float width, float height);
    }
</code></pre>
* Android için;
<pre><code class='language-cs'>
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
</code></pre>

* iOS için;
<pre><code class='language-cs'>
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
</code></pre>

* Xamarin Forms kullanım için;
<pre><code class='language-cs'>
var resizeFile = DependencyService.Get<IMediaService>().ResizeImage(file.Path, 300, 300);
var resizeFile = DependencyService.Get<IMediaService>().ResizeImage(ReadFully(file.GetStream()), 300, 300);
</code></pre>

<pre><code class='language-cs'>
        public byte[] ReadFully(Stream input)
        {
            byte[] buffer = new byte[16 * 1024];
            using (MemoryStream ms = new MemoryStream())
            {
                int read;
                while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }
                return ms.ToArray();
            }
        }
</code></pre>

## yigit@ozaksut.com