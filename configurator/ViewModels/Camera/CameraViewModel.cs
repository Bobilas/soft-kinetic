using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using OpenCvSharp;
using OpenCvSharp.Extensions;
using PropertyChanged;

namespace Configurator
{
    public class CameraViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public BitmapSource Image { get; private set; }
        public int CameraId { get; set; } = 0;

        private static bool _cameraChanged = false;

        public CameraViewModel()
        {
            PropertyChanged += Camera_PropertyChanged;
            StartCameraThread();
        }

        private void Camera_PropertyChanged(object sender, PropertyChangedEventArgs args)
        {
            if (args.PropertyName == nameof(CameraId))
            {
                _cameraChanged = true;
            }
        }

        private void StartCameraThread()
        {
            Task.Run(async () =>
            {
                while (true)
                {
                    try
                    {
                        using (var cam = new VideoCapture(CameraId))
                        {
                            cam.Set(CaptureProperty.FrameWidth, 640);
                            cam.Set(CaptureProperty.FrameHeight, 480);
                            using (var mat = new Mat())
                            {
                                while (true)
                                {
                                    if (_cameraChanged)
                                    {
                                        _cameraChanged = false;
                                        break;
                                    }

                                    cam.Read(mat);
                                    var source = mat.Flip(FlipMode.Y).ToBitmapSource();
                                    source.Freeze();
                                    Image = source;
                                    OnPropertiesChanged(nameof(Image));
                                }
                            }
                        }
                    }
                    catch { }
                    await Task.Delay(10);
                }
            });
        }

        private void OnPropertiesChanged(params string[] propertyNames)
        {
            foreach (var name in propertyNames)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }
    }
}
