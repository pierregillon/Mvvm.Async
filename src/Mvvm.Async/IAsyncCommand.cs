using System.Threading.Tasks;

namespace Mvvm.Async
{
    public interface IAsyncCommand
    {
        Task ExecuteAsync();
        bool CanExecute();
        void RaiseCanExecuteChanged();
    }
}