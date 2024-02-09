using System.Threading;
using Cysharp.Threading.Tasks;

public interface ICommand
{
    public UniTask Execute(CancellationToken token);
    public void Cancel();
}