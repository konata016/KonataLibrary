using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class CommandController<T> where T : Enum
{
    private Dictionary<T, ICommand> commandMap;
    private Dictionary<T, SequenceUniTask> sequenceMap;

    public CommandController()
    {
        commandMap = new Dictionary<T, ICommand>();
        sequenceMap = new Dictionary<T, SequenceUniTask>();
    }

    public void AddCommand(T key, ICommand command)
    {
        if (commandMap.ContainsKey(key))
        {
            Debug.LogError($"コマンドが重複 key: {key}");
            return;
        }

        commandMap.Add(key, command);
        
        var sequence = new SequenceUniTask();
        sequence.AddTask(command.Execute);
        sequence.RegisterOnCancel(command.Cancel);
        sequenceMap.Add(key, sequence);
    }
    
    public void Execute(T type)
    {
        if (sequenceMap.TryGetValue(type, out var value))
        {
            value.Play();
        }
        else
        {
            Debug.LogError($"コマンドが存在しない type: {type}");
        }
    }

    public async UniTask Execute(T type, CancellationToken token)
    {
        if (commandMap.TryGetValue(type, out var value))
        {
            token.Register(value.Cancel);
            await value.Execute(token);
        }
        else
        {
            Debug.LogError($"コマンドが存在しない type: {type}");
        }
    }
    
    public void Discard()
    {
        foreach (var sequence in sequenceMap.Values)
        {
            sequence.Kill();
        }
    }
}