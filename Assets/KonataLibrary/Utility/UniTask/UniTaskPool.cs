using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class UniTaskPool
{
    private Queue<UniTaskEvent> taskQueue;
    private List<UniTaskEvent> taskList;
    private readonly Func<CancellationToken, UniTask> task;
    private readonly GameObject obj;

    public UniTaskPool(GameObject obj = null)
    {
        this.obj = obj;

        taskList = new List<UniTaskEvent>();
    }

    public void PLay(Func<CancellationToken, UniTask> task)
    {
        if (taskQueue == null)
        {
            taskQueue = new Queue<UniTaskEvent>();
        }

        var instance = taskQueue.Count > 0
            ? taskQueue.Dequeue()
            : CreateInstance();

        instance.RegisterTask(task);
        instance.Play();
    }

    public void Clear()
    {
        if (taskQueue == null)
        {
            return;
        }
        while (taskQueue.Count != 0)
        {
            var instance = taskQueue.Dequeue();
            instance.Kill();
        }
    }

    public void KillAll()
    {
        for (var i = 0; i < taskList.Count; i++)
        {
            taskList[i].Kill();
        }
    }

    private void Return(UniTaskEvent instance)
    {
        if (instance == null)
        {
            throw new ArgumentNullException("instanceがnull");
        }

        if (taskQueue == null)
        {
            taskQueue = new Queue<UniTaskEvent>();
        }

        instance.Kill();
        taskQueue.Enqueue(instance);
    }

    private UniTaskEvent CreateInstance()
    {
        var instance = new UniTaskEvent();
        instance.RegisterTask(task);
        if (obj != null)
        {
            instance.SetLink(obj);
        }

        instance.RegisterOnFinished(() => Return(instance));
        taskList.Add(instance);
        return instance;
    }
}
