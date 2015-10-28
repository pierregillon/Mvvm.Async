using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Mvvm.Async
{
    public class AsyncCommand : ICommand, IAsyncCommand
    {
        private readonly Func<Task> _action;
        private readonly Func<bool> _predicate;
        private event EventHandler _canExecuteChanged;
        
        public AsyncCommand(Func<Task> action, Func<bool> predicate = null)
        {
            _action = action;
            _predicate = predicate;
        }
        
        public async Task ExecuteAsync()
        {
            await _action();
        }
        public bool CanExecute()
        {
            return _predicate == null || _predicate();
        }
        public void RaiseCanExecuteChanged()
        {
            var handler = _canExecuteChanged;
            if (handler != null) handler(this, EventArgs.Empty);
        }

        // ----- Implement ICommand
        bool ICommand.CanExecute(object parameter)
        {
            return CanExecute();
        }
        async void ICommand.Execute(object parameter)
        {
            await ExecuteAsync();
        }
        event EventHandler ICommand.CanExecuteChanged
        {
            add { _canExecuteChanged += value; }
            remove { _canExecuteChanged -= value; }
        }
    }
}