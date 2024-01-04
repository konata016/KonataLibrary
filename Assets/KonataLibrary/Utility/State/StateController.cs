using System;
using System.Collections.Generic;

public class StateController<T> where T : Enum
{
    private Dictionary<T, IState<T>> stateMap;

    public T CurrentStateType { get; private set; }

    public StateController()
    {
        stateMap = new Dictionary<T, IState<T>>();
    }

    /// <summary>
    /// ステートの追加
    /// </summary>
    public void AddState(T stateType, IState<T> state)
    {
        stateMap.Add(stateType, state);
    }

    /// <summary>
    /// ステートを変更する
    /// </summary>
    public void ChangeState(T nextStateType)
    {
        if (CurrentStateType.Equals(nextStateType))
        {
            return;
        }

        if (stateMap.ContainsKey(CurrentStateType))
        {
            stateMap[CurrentStateType].OnExit();
        }

        if (!stateMap.ContainsKey(nextStateType))
        {
            UnityEngine.Debug.LogError($"想定外のtype：{nextStateType}");
            return;
        }

        CurrentStateType = nextStateType;
        stateMap[CurrentStateType].OnEnter();
    }

    /// <summary>
    /// 更新時処理
    /// </summary>
    public void OnStateUpdate()
    {
        if (!stateMap.ContainsKey(CurrentStateType))
        {
            return;
        }

        stateMap[CurrentStateType].OnUpdate();
    }
}