using OpenCvSharp;
using OpenCvSharp.Extensions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Window = System.Windows.Window;

namespace Configurator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ContainerViewModel _containerViewModel = new ContainerViewModel();

        public MainWindow()
        {
            InitializeComponent();
            StartCameraThread();
            ComboBox_Containers.DataContext = _containerViewModel;
        }

        private void StartCameraThread()
        {
            Canvas.SetTop(Rectangle_Selection, 0);
            Canvas.SetLeft(Rectangle_Selection, 0);
            Rectangle_Selection.Height = 0;
            Rectangle_Selection.Width = 0;

            Task.Run(async () =>
            {
                while (true)
                {
                    try
                    {
                        using (var cam = new VideoCapture(0))
                        {
                            cam.Set(CaptureProperty.FrameWidth, 640);
                            cam.Set(CaptureProperty.FrameHeight, 480);
                            using (var mat = new Mat())
                            {
                                while (true)
                                {
                                    cam.Read(mat);
                                    Dispatcher.Invoke(() =>
                                    {
                                        var source = mat.Flip(FlipMode.Y).ToBitmapSource();
                                        source.Freeze();
                                        Image_Camera.Source = source;
                                    });
                                }
                            }
                        }
                    }
                    catch { }
                    await Task.Delay(1000);
                }
            });
        }

        private void Button_Add_Click(object sender, RoutedEventArgs e)
        {
            _containerViewModel.Containers.Add(new ContainerInfo())
        }

        private void Button_Remove_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Save_Click(object sender, RoutedEventArgs e)
        {

        }

        private System.Windows.Point _startingPosition;
        private void Image_Camera_MouseDown(object sender, MouseButtonEventArgs e)
        {
            _startingPosition = e.MouseDevice.GetPosition(Border_Rectangle);
            _startingPosition.X -= Border_Rectangle.Padding.Left;
            _startingPosition.Y -= Border_Rectangle.Padding.Top;
        }

        private void Image_Camera_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                var pos = e.MouseDevice.GetPosition(Border_Rectangle);
                pos.X -= Border_Rectangle.Padding.Left;
                pos.Y -= Border_Rectangle.Padding.Top;

                double width = Math.Abs(_startingPosition.X - pos.X);
                double height = Math.Abs(_startingPosition.Y - pos.Y);
                double left = Math.Min(_startingPosition.X, pos.X);
                double top = Math.Min(_startingPosition.Y, pos.Y);

                Rectangle_Selection.Margin = new Thickness(left, top, 0, 0);

                Rectangle_Selection.Width = width;
                Rectangle_Selection.Height = height;
            }
        }
    }
}
