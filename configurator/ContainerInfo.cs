using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace Configurator
{
    public class ContainerInfo
    {
        public string Name { get; set; }
        public Rectangle Area { get; set; }

        public ContainerInfo(string name, Rectangle area)
        {
            Name = name;
            Area = area;
        }
    }

    public class ContainerViewModel : INotifyPropertyChanged
    {
        public ContainerViewModel()
        {
            Containers = new ObservableCollection<ContainerInfo>(new ContainerInfo[] { new ContainerInfo("no", new Rectangle()) });
        }

        public ObservableCollection<ContainerInfo> Containers { get; }

        private string _selectedContainer;
        public string SelectedContainer
        {
            get => _selectedContainer;
            set
            {
                if (_selectedContainer == value)
                {
                    return;
                }
                _selectedContainer = value;
                OnPropertyChanged("PhonebookEntry");
            }
        }

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
