﻿using System;
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

        private string _countMessage;
        public string CountMessage
        {
            get { return _countMessage; }
            set
            {
                _countMessage = value;
                OnPropertyChanged();
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
            }
        }

        private IAsyncCommand _writeMessageCommand;
        public IAsyncCommand CountCommand
        {
            get { return _writeMessageCommand ?? (_writeMessageCommand = new AsyncCommand(WriteMessageAsync)); }
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

        private Task WriteMessageAsync()
        {
            return Task.Factory.StartNew(() =>
            {
                Thread.Sleep(20);
                CountMessage = GetTextToDisplay(++_clickCount);
            });
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