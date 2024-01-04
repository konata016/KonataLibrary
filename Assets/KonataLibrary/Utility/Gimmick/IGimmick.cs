using System;
using UnityEngine;

public interface IGimmick<out T> where T : Enum
{
    public T GimmickType { get; }
    public void Initialize();
    public void Hide();
    public void Show();
    public void OnGimmickEnter(Transform character);
    public void Discard();
}