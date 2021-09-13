using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace MyInventory.Core.Services
{
    public class RelayCommand : ICommand
    {
        Action action;
        Func<bool> canExecute;

        public RelayCommand(Action action, Func<bool> canExecute)
        {
            this.action = action;
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
            action();
        }
    }
}
