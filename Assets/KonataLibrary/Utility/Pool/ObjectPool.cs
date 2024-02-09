using UniRx.Toolkit;
using UnityEngine;

public class ObjectPool<TObj, TData> : ObjectPool<TObj>
    where TObj : PoolObjectBase<TData>
    where TData : IPoolObjectData
{
    private TObj prefab;
    private Transform parent;

    public ObjectPool(TObj prefab, Transform parent)
    {
        this.prefab = prefab;
        this.parent = parent;
    }

    public void OnBeforeRent(TObj obj, TData data)
    {
        obj.OnEntry(data);
    }

    protected override TObj CreateInstance()
    {
        var obj = MonoBehaviour.Instantiate(prefab, parent);
        obj.Initialize();
        return obj;
    }

    protected override void OnBeforeRent(TObj obj)
    {
        obj.Show();
    }

    protected override void OnBeforeReturn(TObj obj)
    {
        obj.OnExit();
        obj.Hide();
    }

    protected override void OnClear(TObj obj)
    {
        obj.Discard();
        base.OnClear(obj);
    }
}