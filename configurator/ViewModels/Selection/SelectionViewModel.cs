using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows;
using PropertyChanged;
using Point = System.Windows.Point;

namespace Configurator
{
    public class SelectionViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private bool _drawing = false;
        private Point _origin;
        public Point Position { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }

        public SelectionViewModel() { }

        public void SetOrigin(Point origin)
        {
            Reset();
            _origin = origin;
            _drawing = true;
        }

        public void Move(Point point)
        {
            if (!_drawing)
            {
                return;
            }

            Position = new Point(
                Math.Min(_origin.X, point.X),
                Math.Min(_origin.Y, point.Y));
            Width = (int)Math.Abs(_origin.X - point.X);
            Height = (int)Math.Abs(_origin.Y - point.Y);

            OnPropertiesChanged(nameof(Position), nameof(Width), nameof(Height));
        }

        public void Stop()
        {
            _drawing = false;
        }

        public void Reset()
        {
            Stop();
            Width = 0;
            Height = 0;

            OnPropertiesChanged(nameof(Width), nameof(Height));
        }

        public void SetSelection(Rectangle selection)
        {
            Position = new Point(selection.Location.X, selection.Location.Y);
            Width = selection.Width;
            Height = selection.Height;

            OnPropertiesChanged(nameof(Position), nameof(Width), nameof(Height));
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
