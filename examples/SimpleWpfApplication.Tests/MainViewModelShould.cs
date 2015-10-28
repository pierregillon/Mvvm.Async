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
        public void be_initialized_with_valid_message()
        {

            Check.That(_mainViewModel.Message).IsEqualTo("Was clicked : 0 times");
        }

        [Fact]
        public async Task update_message_when_calling_command()
        {
            await _mainViewModel.WriteMessageCommand.ExecuteAsync();
            await _mainViewModel.WriteMessageCommand.ExecuteAsync();

            Check.That(_mainViewModel.Message).IsEqualTo("Was clicked : 2 times");
        }
    }
}
