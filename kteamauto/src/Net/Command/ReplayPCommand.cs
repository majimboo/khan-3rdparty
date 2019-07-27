using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace KhanEngine
{
    public class ReplayPCommand : ICommand
    {
        public Action<object> _execute;
        public Func<bool> _canExecute;

        public ReplayPCommand(Action<object> execute, Func<bool> CanExecute)
        {
            if (execute == null)
                throw new NullReferenceException("execute");
            _execute = execute;
            _canExecute = CanExecute;
        }

        public ReplayPCommand(Action<object> execute) : this(execute, null)
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
            _execute(parameter);
        }
    }
}