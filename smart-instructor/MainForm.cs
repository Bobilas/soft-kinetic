using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OpenCvSharp.Extensions;
using OpenCvSharp;
using Forms.Threading;
using System.Drawing.Imaging;
using System.Threading;
using System.Diagnostics;
using DepthSenseWrapper;

namespace SmartInstructor
{
	public partial class MainForm : Form
	{
		public MainForm()
		{
			InitializeComponent();
			initCameraThread();
			initGC();
			initFpsUpdater();
			_garbageCollector.Start();
			_depthSensorThread.Start();

			void initCameraThread()
			{
				_guiUpdater.Update += new EventHandler<ThreadService>((o, service) =>
				{
					var frame = new Mat();

					short[,] depthMap = _depthSensorThread.GetDepthMap();

					if (depthMap != null)
					{
						var mat = new MatOfByte3(480, 640);
						var indexer = mat.GetIndexer();

						//short maxValue = 1;
						//foreach (short value in depthMap)
						//{
						//    if (Math.Abs(value) < 20000 && value > maxValue)
						//    {
						//        maxValue = value;
						//    }
						//}
						//maxValue = 1500;

						for (int row = 0; row < 480; row++)
						{
							for (int col = 0; col < 640; col++)
							{
								var pixel = indexer[row, col];
								short value = depthMap[row / 2, col / 2];
								pixel.Item0 = (byte)(value / 3 % 256);
								pixel.Item1 = 255;
								pixel.Item2 = 255;//(byte)(255 - value * 220 / maxValue);
								if (Math.Abs(value) > 20000)
								{
									pixel.Item1 = 0;
									pixel.Item2 = 0;
								}
								indexer[row, col] = pixel;
							}
						}
						Cv2.CvtColor(mat, mat, ColorConversionCodes.HSV2BGR);

						BeginInvoke(new MethodInvoker(() =>
						{
							PictureBox_DepthMap.Image = mat.ToBitmap();
						}));
					}

					if (_capture.Read(frame) && frame.Width > 0)
					{
						//Cv2.Flip(frame, frame, FlipMode.Y);
						//frame = AlterMat(frame);

						BeginInvoke(new MethodInvoker(() =>
						{
							string[] oldInfo = Label_FrameInfo.Text.Split(' ');
							string fpsInfo = oldInfo.Length > 1 ? oldInfo[1] : "?";
							Label_FrameInfo.Text = $"{frame.Width}x{frame.Height} {fpsInfo}";
							PictureBox_Camera.Image = frame.ToBitmap();
						}));

						_framesCounter.Update();
					}
				});
				_guiUpdater.ThreadCompleted += new EventHandler<ThreadService>((o, service) =>
				{
					BeginInvoke(new MethodInvoker(() =>
					{
						PictureBox_Camera.Image = null;
					}));
					_framesCounter.Reset();
					Label_FrameInfo.Text = "N/A";
				});
				_guiUpdater.UpdateInterval = 0;
			}
			void initGC()
			{
				_garbageCollector.Update += new EventHandler<ThreadService>((obj, service) =>
				{
					GC.Collect();
				});
				_garbageCollector.UpdateInterval = 1000;
			}
			void initFpsUpdater()
			{
				_fpsUpdater.Update += new EventHandler<ThreadService>((obj, service) =>
				{
					BeginInvoke(new MethodInvoker(() =>
					{
						int fps = _framesCounter.GetFps();
						if (fps != -1)
						{
							string[] oldInfo = Label_FrameInfo.Text.Split(' ');
							string resInfo = oldInfo.Length > 0 ? oldInfo[0] : "?x?";
							Label_FrameInfo.Text = $"{resInfo} {fps}";
						}
					}));
				});
				_fpsUpdater.UpdateInterval = 500;
			}
		}

		private DepthSensorThread _depthSensorThread = new DepthSensorThread();

		private bool _guiFlag = false;
		private ThreadService _guiUpdater = new ThreadService("gui-updater");
		private ThreadService _garbageCollector = new ThreadService("garbage-collector");
		private ThreadService _fpsUpdater = new ThreadService("fps-updater");
		private VideoCapture _capture = new VideoCapture(0)
		{
			FrameWidth = 640,
			FrameHeight = 480
		};

		private FramesCounter _framesCounter = new FramesCounter();

		private Mat AlterMat(Mat mat)
		{
			var copy = new Mat();
			mat.CopyTo(copy);

			// Slowest way
			//for (int i = 0; i < copy.Height; i++)
			//{
			//    for (int j = 0; j < copy.Width; j++)
			//    {
			//        var pixel = copy.Get<Vec3b>(i, j);
			//        pixel.Item0 = pixel.Item2;
			//        copy.Set(i, j, pixel);
			//    }
			//}

			// Faster way
			var fasterCopy = new MatOfByte3(mat);
			var indexer = fasterCopy.GetIndexer();
			for (int row = 0; row < fasterCopy.Height; row++)
			{
				for (int col = 0; col < fasterCopy.Width; col++)
				{
					var pixel = indexer[row, col];
					if (50 < pixel.Item2 && pixel.Item2 < 110 && 10 < pixel.Item1 && pixel.Item1 < 100)
					{
						pixel.Item2 = 255;
						pixel.Item1 = 0;
					}
					indexer[row, col] = pixel;
				}
			}

			return fasterCopy;
		}

		private void Button_Demo_Click(object sender, EventArgs e)
		{
			if (!_guiFlag)
			{
				_fpsUpdater.Start();
				_guiUpdater.Start();
			}
			else
			{
				_fpsUpdater.Stop();
				_guiUpdater.Stop();
			}

			_guiFlag = !_guiFlag;
		}
	}
}
