using System;
using System.Windows.Input;

namespace Commands
{
    public class DelegateCommand<T> : ICommand
    {
        private readonly Action<object> _executeAction;
        private readonly Predicate<object> _canExecute; // A predicate is a kind of function pointer that returns a bool

        /// <summary>
        /// object is CommandParameter in Xaml if used
        /// </summary>
        /// <param name="executeAction">The action, can be anonymous delegate</param>
        /// <param name="canExecute">Predicate whether the action can be executed or not</param>
        public DelegateCommand(Action<object> executeAction, Predicate<object> canExecute)
        {
            _executeAction = executeAction;
            _canExecute = canExecute;
        }

        public bool CanExecute(object parameter)
        {
            return _canExecute == null || _canExecute(parameter);
        }

        public void Execute(object parameter)
        {
            _executeAction(parameter);
        }

        public event EventHandler CanExecuteChanged;

        public void RaiseCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
