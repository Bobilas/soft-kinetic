using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Input;
using PropertyChanged;

namespace Configurator
{
    public class ContainerViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public ObservableCollection<ContainerInfo> Items { get; }
        public int SelectedContainer { get; set; } = -1;
        public string NewContainerName { get; set; }
        public ICommand AddCommand { get; set; }
        public ICommand RemoveCommand { get; set; }

        public readonly SelectionViewModel SelectionViewModel;

        public ContainerViewModel(ObservableCollection<ContainerInfo> items)
        {
            SelectionViewModel = new SelectionViewModel();
            Items = items;
            PropertyChanged += Container_PropertyChanged;
            AddCommand = new RelayCommand(AddContainer);
            RemoveCommand = new RelayCommand(RemoveContainer);
        }

        private void AddContainer()
        {
            Items.Add(new ContainerInfo(NewContainerName, new Rectangle()));
            NewContainerName = string.Empty;
            SelectedContainer = Items.Count - 1;
            SelectionViewModel.Reset();

            OnPropertiesChanged(nameof(Items), nameof(NewContainerName), nameof(SelectedContainer));
        }
        private void RemoveContainer()
        {
            if (SelectedContainer != -1)
            {
                Items.RemoveAt(SelectedContainer);
                SelectedContainer = Items.Count - 1;
                SelectionViewModel.Reset();

                OnPropertiesChanged(nameof(SelectedContainer), nameof(Items));
            }
        }

        public void SaveSelection()
        {
            if (SelectedContainer != -1)
            {
                Items[SelectedContainer].Area = new Rectangle(
                    (int)SelectionViewModel.Position.X, 
                    (int)SelectionViewModel.Position.Y, 
                    SelectionViewModel.Width, 
                    SelectionViewModel.Height);
            }
        }

        private void Container_PropertyChanged(object sender, PropertyChangedEventArgs args)
        {
            if (args.PropertyName == nameof(SelectedContainer) && SelectedContainer != -1)
            {
                SelectionViewModel.SetSelection(Items[SelectedContainer].Area);
            }
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
