using UnityEngine;

public abstract class PoolObjectBase<T> : MonoBehaviour where T : IPoolObjectData
{
    /// <summary>初期化</summary>
    public abstract void Initialize();
    /// <summary>登場時処理</summary>
    public abstract void OnEntry(T data);
    /// <summary>退出時処理</summary>
    public abstract void OnExit();
    /// <summary>破棄時処理</summary>
    public abstract void Discard();
    
    /// <summary>
    /// 非表示
    /// </summary>
    public virtual void Hide()
    {
        gameObject.SetActive(false);
    }
     
    /// <summary>
    /// 表示
    /// </summary>
    public virtual void Show()
    {
        gameObject.SetActive(true);
    }
}