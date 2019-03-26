using Po.Forms.Threading;
using OpenCvSharp;
using OpenCvSharp.Extensions;

namespace SmartInstructor.Camera
{
    public partial class CameraThread : ThreadService
    {
        private void OnCameraUpdate()
        {
            if (_capture.Read(_cameraFrame) && _cameraFrame.Width > 0)
            {
                //Cv2.Flip(frame, frame, FlipMode.Y);
                AlterMat();
                _framesCounter.Update();
                ImageProcessed?.Invoke(this, new ImageProcessedEventArgs(_cameraFrame.ToBitmap(), _framesCounter.GetFps()));
            }
        }

        private void AlterMat()
        {
            if (_cameraIndexer == null && _cameraFrame.Width > 0)
            {
                _cameraIndexer = _cameraFrame.GetIndexer();
            }

            for (int row = 0; row < _cameraFrame.Height; row++)
            {
                for (int col = 0; col < _cameraFrame.Width; col++)
                {
                    var pixel = _cameraIndexer[row, col];
                    _cameraIndexer[row, col] = pixel;
                }
            }
        }
    }
}
