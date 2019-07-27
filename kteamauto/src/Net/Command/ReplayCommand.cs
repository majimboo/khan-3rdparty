using System;
using System.Windows.Input;

namespace KhanEngine
{
    public class ReplayCommand : ICommand
    {
        public Action _execute;
        public Func<bool> _canExecute;

        public ReplayCommand(Action execute, Func<bool> CanExecute)
        {
            if (execute == null)
                throw new NullReferenceException("execute");
            _execute = execute;
            _canExecute = CanExecute;
        }

        public ReplayCommand(Action execute) : this(execute, null)
        {
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public bool CanExecute(object parameter)
        {
            return _canExecute == null ? true : _canExecute();
        }

        public void Execute(object parameter)
        {
            _execute.Invoke();
        }
    }
}