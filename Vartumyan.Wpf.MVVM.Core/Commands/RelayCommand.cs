using System;
using System.Windows.Input;

namespace Vartumyan.Wpf.MVVM.Core
{
    public class RelayCommand : ICommand
    {
        private readonly Predicate<object> _canExecute;
        private readonly Action<object> _execute;

        public event EventHandler CanExecuteChanged
        {
            add
            {
                CommandManager.RequerySuggested += value;
            }
            remove
            {
                CommandManager.RequerySuggested -= value;
            }
        }

        public RelayCommand(Action<object> execute)
           : this(execute, null)
                =>  _execute = execute;

        public RelayCommand(Action<object> execute, Predicate<object> canExecute)
        {
            if (execute == null)
                throw new ArgumentNullException("execute");
            _execute = execute;
            _canExecute = canExecute;
        }

        public virtual bool CanExecute(object parameter) => _canExecute == null || _canExecute(parameter);

        public virtual void Execute(object parameter) => _execute(parameter);
    }

    public class RelayCommand<T> : ICommand
    {
        private readonly Predicate<T> _canExecute;
        private readonly Action<T> _execute;

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public RelayCommand(Action<T> execute)
           : this(execute, null)
                => _execute = execute;

        public RelayCommand(Action<T> execute, Predicate<T> canExecute)
        {
            if (execute == null)
                throw new ArgumentNullException("execute");

            _execute = execute;
            _canExecute = canExecute;
        }

        public virtual bool CanExecute(object parameter) 
            => _canExecute == null || _canExecute((T)parameter);

        public virtual void Execute(object parameter)
            => _execute((T)parameter);
    }
}
