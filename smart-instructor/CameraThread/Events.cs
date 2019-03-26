using Po.Forms.Threading;
using System;
using System.Drawing;

namespace SmartInstructor.Camera
{
    public partial class CameraThread : ThreadService
    {
        public event EventHandler<ImageProcessedEventArgs> ImageProcessed;
        public event EventHandler<ImageProcessedEventArgs> DepthImageProcessed;

        public class ImageProcessedEventArgs : EventArgs
        {
            public Bitmap Image;
            public int Fps;

            public ImageProcessedEventArgs(Bitmap image, int fps)
            {
                Image = image;
                Fps = fps;
            }
        }
    }
}
