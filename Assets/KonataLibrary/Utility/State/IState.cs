using System;

public interface IState<T> where T : Enum
{
    StateController<T> StateController { get; }

    T StateType { get; }
    
    void OnEnter();
    
    void OnUpdate();
    
    void OnExit();
}
