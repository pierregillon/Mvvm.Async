﻿using System;
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
            if (CanExecute()) {
                await _action();
            }
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

    public class AsyncCommand<T> : ICommand
    {
        private readonly Func<T, Task> _parameterizedAction;
        private readonly Predicate<T> _canExecute;

        public AsyncCommand(Func<T, Task> parameterizedAction, Predicate<T> canExecute = null)
        {
            _parameterizedAction = parameterizedAction;
            _canExecute = canExecute;
        }

        public async Task ExecuteAsync(T value)
        {
            if (CanExecute(value)) {
                await _parameterizedAction(value);
            }
        }
        public bool CanExecute(T value)
        {
            return _canExecute == null || _canExecute(value);
        }

        // ----- Explicit implementations
        bool ICommand.CanExecute(object parameter)
        {
            return CanExecute((T) parameter);
        }
        async void ICommand.Execute(object parameter)
        {
            await ExecuteAsync((T)parameter);
        }
        public event EventHandler CanExecuteChanged;
    }
}