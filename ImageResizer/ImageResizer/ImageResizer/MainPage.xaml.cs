using Plugin.Media;
using Plugin.Media.Abstractions;
using System;
using System.IO;
using Xamarin.Forms;

namespace ImageResizer
{
    public partial class MainPage : ContentPage
    {
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
        public MainPage()
		{
			InitializeComponent();
		}

        async void TakePicture(object sender, EventArgs e)
        {
            var init = await CrossMedia.Current.Initialize();
            if (init)
            {
                var file = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions
                {
                    PhotoSize = PhotoSize.Medium,
                    CompressionQuality = 100,
                    //Directory = "Resize",
                    Name = Guid.NewGuid().ToString() + ".jpg",
                    //RotateImage = true,
                    //AllowCropping = true,
                    //DefaultCamera = CameraDevice.Rear,
                    //MaxWidthHeight = 300,
                    //SaveToAlbum = true,
                });
                if (file == null)
                    return;

                imgOriginalImage.Source = file.Path;

                //var resizeFile = DependencyService.Get<IMediaService>().ResizeImage(ReadFully(file.GetStream()), 300, 300);
                var resizeFile = DependencyService.Get<IMediaService>().ResizeImage(file.Path, 300, 300);
                if (resizeFile.Length > 0)
                {
                    imgResize.Source = ImageSource.FromStream(() =>
                    {
                        return new MemoryStream(resizeFile);
                    });
                }
                else
                {
                    imgResize.Source = file.Path;
                }

                file.Dispose();
            }
        }
    }
}