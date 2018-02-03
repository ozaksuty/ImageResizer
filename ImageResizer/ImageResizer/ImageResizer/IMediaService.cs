namespace ImageResizer
{
    public interface IMediaService
    {
        byte[] ResizeImage(byte[] imageData, float width, float height);
        byte[] ResizeImage(string imagePath, float width, float height);
    }
}