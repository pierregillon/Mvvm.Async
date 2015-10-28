using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Mvvm.Async
{
    public class AsyncCommand : ICommand
    {
        private readonly Func<Task> _action;
        private readonly Func<bool> _predicate;

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

        // ----- Implement ICommand
        public bool CanExecute(object parameter)
        {
            return CanExecute();
        }
        public async void Execute(object parameter)
        {
            await ExecuteAsync();
        }
        public event EventHandler CanExecuteChanged;
    }
}
