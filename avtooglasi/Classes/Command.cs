using System;
using System.Windows.Input;

namespace avtooglasi.Classes
{
    public class Command : ICommand
    {
        private readonly Action<object?> _action;
        private readonly Func<object?, bool>? _canExecute;

        public event EventHandler? CanExecuteChanged;

        public Command(Action<object?> execute, Func<object?, bool>? canExecute = null)
        {
            _action = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }

        public bool CanExecute(object? parameter)
        {
            return _canExecute?.Invoke(parameter) ?? true;
        }

        public void Execute(object? parameter)
        {
            _action(parameter);
        }

        public void RaiseCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
