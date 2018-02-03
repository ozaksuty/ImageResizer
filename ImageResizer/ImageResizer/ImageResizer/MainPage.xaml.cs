using Plugin.Media;
using Plugin.Media.Abstractions;
using System;
using System.IO;
using Xamarin.Forms;

namespace ImageResizer
{
    public partial class MainPage : ContentPage
	{
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