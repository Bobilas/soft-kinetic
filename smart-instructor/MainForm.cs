using System;
using System.Windows.Forms;
using OpenCvSharp.Extensions;
using OpenCvSharp;
using Po.Forms.Threading;
using static SmartInstructor.Camera.CameraThread;
using SmartInstructor.Camera;

namespace SmartInstructor
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            initCameraThread();
            initGC();
            _garbageCollector.Start();

            void initCameraThread()
            {
                _cameraThread.ImageProcessed += new EventHandler<ImageProcessedEventArgs>((obj, e) =>
                {
                    BeginInvoke((MethodInvoker)delegate
                    {
                        Label_FrameInfo.Text = $"{e.Fps}";
                        PictureBox_Camera.Image = e.Image;
                    });
                });
                _cameraThread.DepthImageProcessed += new EventHandler<ImageProcessedEventArgs>((obj, e) =>
                {
                    BeginInvoke((MethodInvoker)delegate
                    {
                        PictureBox_DepthMap.Image = e.Image;
                    });
                });
                _cameraThread.ThreadCompleted += new EventHandler<ThreadService>((obj, service) =>
                {
                    BeginInvoke((MethodInvoker)delegate
                    {
                        PictureBox_Camera.Image = null;
                        PictureBox_DepthMap.Image = null;
                    });
                });

            }
            void initGC()
            {
                _garbageCollector.Update += new EventHandler<ThreadService>((obj, service) =>
                {
                    GC.Collect();
                });
                _garbageCollector.UpdateInterval = 5000;
            }
        }

        private CameraThread _cameraThread = new CameraThread();
        private bool _runFlag = false;
        private ThreadService _garbageCollector = new ThreadService("garbage-collector");

        private void Button_Demo_Click(object sender, EventArgs e)
        {
            if (!_runFlag)
            {
                _cameraThread.Start();
            }
            else
            {
                _cameraThread.Stop();
            }

            _runFlag = !_runFlag;
        }
    }
}
