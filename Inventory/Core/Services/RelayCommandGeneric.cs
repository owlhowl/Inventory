using System;
using System.Windows.Input;

namespace MyInventory.Core.Services
{
    public class RelayCommand<T> : ICommand
    {
        Action<T> execute;
        Func<bool> canExecute;

        public RelayCommand(Action<T> execute, Func<bool> canExecute)
        {
            this.execute = execute;
            this.canExecute = canExecute;
        }

        public event EventHandler CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }

        public bool CanExecute(object parameter)
        {
            return canExecute();
        }

        public void Execute(object parameter)
        {
            if (parameter == null)
                throw new Exception("Command<T> execute parameter is null.");
            execute((T)parameter);
        }
    }
}
