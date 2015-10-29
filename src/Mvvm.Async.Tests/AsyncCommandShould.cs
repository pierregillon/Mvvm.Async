using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Moq;
using NFluent;
using Xunit;

namespace Mvvm.Async.Tests
{
    public class AsyncCommandShould
    {
        private static readonly Func<Task> SOME_ACTION = () => Task.Factory.StartNew(() => { });
        private static readonly Func<string, Task> SOME_PARAMETERIZED_ACTION = s => Task.Factory.StartNew(() => { });

        private readonly Mock<IAction> _action;
        private readonly Mock<IAction<string>> _parameterizedAction;

        public AsyncCommandShould()
        {
            _action = new Mock<IAction>();
            _parameterizedAction = new Mock<IAction<string>>();
        }

        [Fact]
        public async Task execute_an_async_delegate()
        {
            var asyncCommand = new AsyncCommand(_action.Object.Execute);

            await asyncCommand.ExecuteAsync();

            _action.Verify(x => x.Execute(), Times.Once);
        }

        [Fact]
        public async Task execute_an_async_delegate_with_parameter()
        {
            var asyncCommand = new AsyncCommand<string>(_parameterizedAction.Object.Execute);

            await asyncCommand.ExecuteAsync("Boom");

            _parameterizedAction.Verify(x => x.Execute("Boom"), Times.Once);
        }

        [Fact]
        public void not_be_executable_when_predicate_returns_false()
        {
            var asyncCommand = new AsyncCommand(SOME_ACTION, () => false);

            var canExecute = asyncCommand.CanExecute();

            Check.That(canExecute).IsFalse();
        }

        [Fact]
        public void not_be_executable_when_parameterized_predicate_returns_false()
        {
            var asyncCommand = new AsyncCommand<string>(SOME_PARAMETERIZED_ACTION, value => false);

            var canExecute = asyncCommand.CanExecute("Boom");

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
        public void be_executable_when_parameterized_predicate_returns_false()
        {
            var asyncCommand = new AsyncCommand<string>(SOME_PARAMETERIZED_ACTION, x => true);

            var canExecute = asyncCommand.CanExecute("Boom");

            Check.That(canExecute).IsTrue();
        }

        [Fact]
        public void be_executable_when_no_predicate_defined()
        {
            var asyncCommand = new AsyncCommand(SOME_ACTION);

            var canExecute = asyncCommand.CanExecute();

            Check.That(canExecute).IsTrue();
        }

        [Fact]
        public async Task not_execute_when_predicate_returns_false()
        {
            var asyncCommand = new AsyncCommand(_action.Object.Execute, () => false);

            await asyncCommand.ExecuteAsync();

            _action.Verify(x => x.Execute(), Times.Never);
        }

        [Fact]
        public async Task not_execute_when_parameterized_predicate_returns_false()
        {
            var asyncCommand = new AsyncCommand<string>(_parameterizedAction.Object.Execute, x => false);
            
            await asyncCommand.ExecuteAsync("Boom");

            _parameterizedAction.Verify(x => x.Execute(It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public void execute_an_async_delegate_with_ICommand()
        {
            ICommand asyncCommand = new AsyncCommand(_action.Object.Execute);

            asyncCommand.Execute(null);

            _action.Verify(x => x.Execute(), Times.Once);
        }

        [Fact]
        public void execute_an_async_parameterized_delegate_with_ICommand()
        {
            ICommand asyncCommand = new AsyncCommand<string>(_parameterizedAction.Object.Execute);

            asyncCommand.Execute("Boom");

            _parameterizedAction.Verify(x => x.Execute("Boom"), Times.Once);
        }

        [Fact]
        public void not_be_executable_with_ICommand_when_predicate_returns_false()
        {
            ICommand asyncCommand = new AsyncCommand(SOME_ACTION, () => false);

            var canExecute = asyncCommand.CanExecute(null);

            Check.That(canExecute).IsFalse();
        }

        [Fact]
        public void not_be_executable_with_ICommand_when_parameterized_predicate_returns_false()
        {
            ICommand asyncCommand = new AsyncCommand<string>(SOME_PARAMETERIZED_ACTION, x => false);

            var canExecute = asyncCommand.CanExecute("Boom");

            Check.That(canExecute).IsFalse();
        }

        [Fact]
        public void raise_can_execute_changed_for_command()
        {
            var canExecuteChanged = false;
            var asyncCommand = new AsyncCommand(SOME_ACTION, () => false);
            ((ICommand) asyncCommand).CanExecuteChanged += (sender, args) => { canExecuteChanged = true; };

            asyncCommand.RaiseCanExecuteChanged();

            Thread.Sleep(20);

            Check.That(canExecuteChanged).IsTrue();
        }

        [Fact]
        public void raise_can_execute_changed_for_generic_command()
        {
            var canExecuteChanged = false;
            var asyncCommand = new AsyncCommand<string>(SOME_PARAMETERIZED_ACTION, x => false);
            ((ICommand)asyncCommand).CanExecuteChanged += (sender, args) => { canExecuteChanged = true; };

            asyncCommand.RaiseCanExecuteChanged();

            Thread.Sleep(20);

            Check.That(canExecuteChanged).IsTrue();
        }

        // ----- Internal classes

        public interface IAction
        {
            Task Execute();
        }
        public interface IAction<in T>
        {
            Task Execute(T value);
        }
    }
}