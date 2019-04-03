using System;
using System.Windows.Input;

namespace Configurator
{
    public class RelayCommand : ICommand
    {
        private Action _action;
        public event EventHandler CanExecuteChanged;
        
        public RelayCommand(Action action)
        {
            _action = action;
        }

        public virtual bool CanExecute(object parameter)
        {
            return true;
        }

        public virtual void Execute(object parameter)
        {
            _action();
        }
    }
}
