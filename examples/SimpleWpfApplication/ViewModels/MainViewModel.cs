using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
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

        private string _countMessage;
        public string CountMessage
        {
            get { return _countMessage; }
            set
            {
                _countMessage = value;
                OnPropertyChanged();
                _countCommand.RaiseCanExecuteChanged();
            }
        }

        private string _message;
        public string Message
        {
            get { return _message; }
            set
            {
                _message = value;
                OnPropertyChanged();
                _writeMessageWithParameterCommand.RaiseCanExecuteChanged();
            }
        }

        private IAsyncCommand _countCommand;
        public IAsyncCommand CountCommand
        {
            get
            {
                return _countCommand ??
                       (_countCommand = new AsyncCommand(CountAsync, CanCount));
            }
        }

        private IAsyncCommand<string> _writeMessageWithParameterCommand;
        public IAsyncCommand<string> WriteMessageWithParameterCommand
        {
            get
            {
                return _writeMessageWithParameterCommand ??
                       (_writeMessageWithParameterCommand = new AsyncCommand<string>(WriteMessageWithParameterAsync, CanWriteMessageAsync));
            }
        }

        public MainViewModel()
        {
            _countMessage = GetTextToDisplay(_clickCount);
        }

        private Task CountAsync()
        {
            return Task.Factory.StartNew(() =>
            {
                Thread.Sleep(20);
                CountMessage = GetTextToDisplay(++_clickCount);
            });
        }
        private bool CanCount()
        {
            return _clickCount < 10;
        }

        private Task WriteMessageWithParameterAsync(string message)
        {
            return Task.Factory.StartNew(() => { Message = message; });
        }
        private bool CanWriteMessageAsync(string value)
        {
            return string.IsNullOrEmpty(value) == false;
        }

        private static string GetTextToDisplay(int count)
        {
            return string.Format("Was clicked : {0} times", count);
        }
    }
}