using System;
using System.Threading.Tasks;
using System.Windows.Input;
using NFluent;
using Xunit;

namespace Mvvm.Async.Tests
{
    public class AsyncCommandShould
    {
        private static readonly Func<Task> SOME_ACTION = () => Task.Factory.StartNew(() => { });

        [Fact]
        public async Task execute_an_async_delegate()
        {
            var result = string.Empty;

            Func<Task> action = () => Task.Factory.StartNew(() => result = "Success");

            var asyncCommand = new AsyncCommand(action);

            await asyncCommand.ExecuteAsync();

            Check.That(result).IsEqualTo("Success");
        }

        [Fact]
        public void not_be_executable_when_predicate_returns_false()
        {
            var asyncCommand = new AsyncCommand(SOME_ACTION, () => false);

            var canExecute = asyncCommand.CanExecute();

            Check.That(canExecute).IsFalse();
        }

        [Fact]
        public void be_executable_when_predicate_returns_false()
        {
            var asyncCommand = new AsyncCommand(SOME_ACTION, () => true);

            var canExecute = asyncCommand.CanExecute();

            Check.That(canExecute).IsTrue();
        }

        [Fact]
        public void be_executable_when_no_predicate_defined()
        {
            var asyncCommand = new AsyncCommand(SOME_ACTION);

            var canExecute = asyncCommand.CanExecute();

            Check.That(canExecute).IsTrue();
        }
    }
}