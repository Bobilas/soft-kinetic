using Po.Forms.Threading;
using OpenCvSharp;
using OpenCvSharp.Extensions;
using System;

namespace SmartInstructor.Camera
{
    public partial class CameraThread : ThreadService
    {
        private void OnDepthUpdate()
        {
            short[,] depthMap = _depthSensorRunner.GetDepthMap();

            if (depthMap != null)
            {
                for (int row = 0; row < 480; row++)
                {
                    for (int col = 0; col < 640; col++)
                    {
                        var pixel = _depthIndexer[row, col];
                        short value = depthMap[row / 2, col / 2];

                        pixel.Item0 = (byte)((value / 3) % 256);
                        pixel.Item1 = 255;
                        pixel.Item2 = 255;
                        if (value > 10000)
                        {
                            pixel.Item1 = 0;
                            pixel.Item2 = 0;
                        }
                        _depthIndexer[row, col] = pixel;
                    }
                }

                Cv2.CvtColor(_depthFrame, _depthFrame, ColorConversionCodes.HSV2BGR);

                _depthFramesCounter.Update();
                DepthImageProcessed?.Invoke(this, new ImageProcessedEventArgs(_depthFrame.ToBitmap(), _depthFramesCounter.GetFps()));
            }
        }
    }
}
