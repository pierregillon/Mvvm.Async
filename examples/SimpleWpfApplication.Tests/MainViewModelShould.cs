using System.Threading.Tasks;
using NFluent;
using SimpleWpfApplication.ViewModels;
using Xunit;

namespace SimpleWpfApplication.Tests
{
    public class MainViewModelShould
    {
        private readonly MainViewModel _mainViewModel;

        public MainViewModelShould()
        {
            _mainViewModel = new MainViewModel();
        }

        [Fact]
        public void be_initialized_with_valid_count_message()
        {
            Check.That(_mainViewModel.CountMessage).IsEqualTo("Was clicked : 0 times");
        }

        [Fact]
        public async Task update_count_message_when_calling_count_command()
        {
            await _mainViewModel.CountCommand.ExecuteAsync();
            await _mainViewModel.CountCommand.ExecuteAsync();

            Check.That(_mainViewModel.CountMessage).IsEqualTo("Was clicked : 2 times");
        }

        [Fact]
        public async Task update_message_when_calling_write_command()
        {
            const string message = "Hello world";

            await _mainViewModel.WriteMessageWithParameterCommand.ExecuteAsync(message);

            Check.That(_mainViewModel.Message).IsEqualTo(message);
        }

        [Fact]
        public void not_able_to_execute_write_message_when_no_empty_message()
        {
            var canExecute = _mainViewModel.WriteMessageWithParameterCommand.CanExecute(string.Empty);

            Check.That(canExecute).IsFalse();
        }

        [Fact]
        public void not_be_able_to_update_last_message_when_no_message_entered()
        {
            _mainViewModel.WriteMessageWithParameterCommand.ExecuteAsync("hello world");
            _mainViewModel.WriteMessageWithParameterCommand.ExecuteAsync("hello world 2");
            _mainViewModel.WriteMessageWithParameterCommand.ExecuteAsync(string.Empty);

            Check.That(_mainViewModel.Message).IsEqualTo("hello world 2");
        }
    }
}
