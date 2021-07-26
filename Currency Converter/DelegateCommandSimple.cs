using System;
using System.Windows.Input;

namespace Commands
{
    // This class implements a DelegateCommand
    // but without the ability to change the CanExecute property.
    // The actions are always executable.
    // If CanExecute should change, use the DelegateCommand class.

    public class DelegateCommandSimple : ICommand
    {

        private readonly Action _action;

        public DelegateCommandSimple(Action action)
        {
            _action = action;
        }

        public void Execute(object parameter)
        {
            try
            {
                _action();
            }
            catch (Exception ex)
            {
            }
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

#pragma warning disable 67
        public event EventHandler CanExecuteChanged;
#pragma warning restore 67
    }
}
