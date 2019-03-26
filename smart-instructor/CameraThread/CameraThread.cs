using Po.Forms.Threading;
using OpenCvSharp;
using SmartInstructor.Depth;
using System;
using System.Threading.Tasks;

namespace SmartInstructor.Camera
{
    public partial class CameraThread : ThreadService
    {
        private VideoCapture _capture;
        private FramesCounter _framesCounter = new FramesCounter();
        private FramesCounter _depthFramesCounter = new FramesCounter();
        private static DepthSensorRunner _depthSensorRunner = new DepthSensorRunner();

        private MatOfByte3 _cameraFrame = new MatOfByte3();
        private MatIndexer<Vec3b> _cameraIndexer = null;
        private MatOfByte3 _depthFrame = new MatOfByte3(480, 640);
        private MatIndexer<Vec3b> _depthIndexer = null;

        public CameraThread() : base("camera-thread")
        {
            Update += new EventHandler<ThreadService>((obj, service) => OnUpdate());
            ThreadCompleted += new EventHandler<ThreadService>((obj, service) => OnThreadCompleted());
            UpdateInterval = 0;

            _depthIndexer = _depthFrame.GetIndexer();

            _capture = new VideoCapture(0)
            {
                FrameWidth = 640,
                FrameHeight = 480
            };
        }

        private void OnUpdate()
        {
            OnCameraUpdate();
            OnDepthUpdate();
        }
        
        private void OnThreadCompleted()
        {
            _framesCounter.Reset();
            _depthFramesCounter.Reset();
        }
    }
}
