using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using Window = System.Windows.Window;

namespace Configurator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly ObservableCollection<ContainerInfo> Containers = new ObservableCollection<ContainerInfo>();

        private ContainerViewModel _containerViewModel;
        private CameraViewModel _cameraViewModel;
        private SelectionViewModel _selectionViewModel;

        public MainWindow()
        {
            InitializeComponent();

            initViewModels();
            assignDataContexts();

            void initViewModels()
            {
                _containerViewModel = new ContainerViewModel(Containers);
                _selectionViewModel = _containerViewModel.SelectionViewModel;
                _cameraViewModel = new CameraViewModel();
            }
            void assignDataContexts()
            {
                ComboBox_Containers.DataContext = _containerViewModel;
                TextBox_Container.DataContext = _containerViewModel;
                Button_Add.DataContext = _containerViewModel;
                Button_Remove.DataContext = _containerViewModel;

                Image_Camera.DataContext = _cameraViewModel;

                Rectangle_Selection.DataContext = _selectionViewModel;
            }
        }

        private void Image_Camera_MouseDown(object sender, MouseButtonEventArgs e)
        {
            _selectionViewModel.SetOrigin(e.MouseDevice.GetPosition(Image_Camera));
        }

        private void Image_Camera_MouseMove(object sender, MouseEventArgs e)
        {
            _selectionViewModel.Move(e.MouseDevice.GetPosition(Image_Camera));
        }

        private void Image_Camera_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (e.ButtonState == MouseButtonState.Released)
            {
                _selectionViewModel.Stop();
                _containerViewModel.SaveSelection();
            }
        }
    }
}
