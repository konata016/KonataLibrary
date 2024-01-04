using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class UniTaskEvent
{
    private CancellationTokenSource source;
    private Func<CancellationToken, UniTask> task;
    private GameObject gameObject;
    private bool isCancel;
    
    private Action onFinished;

    public UniTaskEvent()
    {
        isCancel = false;
    }
    
    public void RegisterTask(Func<CancellationToken, UniTask> task)
    {
        this.task = task;
    }

    public void Play()
    {
        source = new CancellationTokenSource();
        var token = gameObject == null
            ? source
            : CancellationTokenSource.CreateLinkedTokenSource(
                source.Token,
                gameObject.GetCancellationTokenOnDestroy());
        
        isCancel = false;
        ProcessAsync(token.Token).Forget();
    }
    
    public void Kill()
    {
        isCancel = true;
        source?.Cancel();
    }
    
    public void SetLink(GameObject obj)
    {
        gameObject = obj;
    }

    public void RegisterOnFinished(Action onFinished)
    {
        this.onFinished = onFinished;
    }

    private async UniTask ProcessAsync(CancellationToken token)
    {
        await UniTask.WhenAny(
            task.Invoke(token),
            UniTask.WaitUntil(() => isCancel, cancellationToken: token));
        
        onFinished?.Invoke();
    }
}
