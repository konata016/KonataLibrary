using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class SequenceUniTask
{
    private CancellationTokenSource source;
    private List<Func<CancellationToken, UniTask>> taskList;
    private GameObject gameObject;
    private bool isLoop;
    private bool isCancel;

    private Action onCancel;
    private Action onComplete;

    public SequenceUniTask()
    {
        taskList = new List<Func<CancellationToken, UniTask>>();
        gameObject = null;
        isLoop = false;
        isCancel = false;
    }

    public void AddTask(Func<CancellationToken, UniTask> task)
    {
        taskList.Add(task);
    }

    public void RegisterOnCancel(Action onCancel)
    {
        this.onCancel = onCancel;
    }

    public void RegisterOnComplete(Action onComplete)
    {
        this.onComplete = onComplete;
    }

    public void RemoveTaskAll()
    {
        Kill();
        taskList.Clear();
    }

    public void Play()
    {
        isCancel = true;
        source?.Cancel();
        
        source = new CancellationTokenSource();
        var token = gameObject == null
            ? source
            : CancellationTokenSource.CreateLinkedTokenSource(
                source.Token,
                gameObject.GetCancellationTokenOnDestroy());

        isCancel = false;
        ProcessLoopAsync(token.Token).Forget();
    }

    public void SetLoop()
    {
        isLoop = true;
    }

    public void Kill()
    {
        isCancel = true;
        source?.Cancel();
        onCancel?.Invoke();
    }

    public void SetLink(GameObject obj)
    {
        gameObject = obj;
    }

    private async UniTask ProcessLoopAsync(CancellationToken token)
    {
        for (;;)
        {
            await ProcessAsync(token);
            
            if (!isLoop || isCancel || token.IsCancellationRequested)
            {
                onComplete?.Invoke();
                return;
            }
        }
    }
    
    private async UniTask ProcessAsync(CancellationToken token)
    {
        for (var i = 0; i < taskList.Count; i++)
        {
            var task = taskList[i];
            await UniTask.WhenAny(
                task.Invoke(token),
                UniTask.WaitUntil(() => isCancel, cancellationToken: token));

            if (isCancel)
            {
                return;
            }
        }
    }
}
