using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Logic.Helpers
{
    public class RelayCommand : ICommand
    {
        readonly Action<object> execute;
        readonly Func<object, bool> canExecute;

        public RelayCommand(Action<object> action, Func<object, bool> canExecute = null!)
        {
            execute = action;
            this.canExecute = canExecute;
        }

        public event EventHandler? CanExecuteChanged;

        public void RaiseCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }

        public bool CanExecute(object? parameter)
        {
            return canExecute == null || canExecute(parameter!);
        }

        public void Execute(object? parameter)
        {
            execute(parameter!);
        }
    }
}
