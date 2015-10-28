using System.ComponentModel;
using System.Runtime.CompilerServices;
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

        private ICommand _writeMessageCommand;
        public ICommand WriteMessageCommand
        {
            get { return _writeMessageCommand ?? (_writeMessageCommand = new AsyncCommand(WriteMessageAsync)); }
        }

        public MainViewModel()
        {
            _message = GetTextToDisplay(_clickCount);
        }

        private Task WriteMessageAsync()
        {
            return Task.Factory.StartNew(() => { Message = GetTextToDisplay(++_clickCount); });
        }

        private static string GetTextToDisplay(int count)
        {
            return string.Format("Was clicked : {0} times", count);
        }
    }
}