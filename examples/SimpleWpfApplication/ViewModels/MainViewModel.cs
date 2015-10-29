using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Mvvm.Async;

namespace SimpleWpfApplication.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private int _clickCount;

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }

        private string _message;
        public string Message
        {
            get { return _message; }
            set
            {
                _message = value;
                OnPropertyChanged();
            }
        }

        private string _message2;
        public string Message2
        {
            get { return _message2; }
            set
            {
                _message2 = value;
                OnPropertyChanged();
            }
        }

        private IAsyncCommand _writeMessageCommand;
        public IAsyncCommand WriteMessageCommand
        {
            get { return _writeMessageCommand ?? (_writeMessageCommand = new AsyncCommand(WriteMessageAsync)); }
        }

        private IAsyncCommand<string> _writeMessageWithParameterCommand;
        public IAsyncCommand<string> WriteMessageWithParameterCommand
        {
            get { return _writeMessageWithParameterCommand ?? (_writeMessageWithParameterCommand = new AsyncCommand<string>(WriteMessageWithParameterAsync)); }
        }

        public MainViewModel()
        {
            _message = GetTextToDisplay(_clickCount);
        }

        private Task WriteMessageAsync()
        {
            return Task.Factory.StartNew(() =>
            {
                Thread.Sleep(20);
                Message = GetTextToDisplay(++_clickCount);
            });
        }
        private Task WriteMessageWithParameterAsync(string message)
        {
            return Task.Factory.StartNew(() =>
            {
                Message2 = message;
            });
        }

        private static string GetTextToDisplay(int count)
        {
            return string.Format("Was clicked : {0} times", count);
        }
    }
}