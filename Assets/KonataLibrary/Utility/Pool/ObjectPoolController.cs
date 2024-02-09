using System.Collections.Generic;
using System.Linq;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

public class ObjectPoolController<TObj, TData>
    where TObj : PoolObjectBase<TData>
    where TData : IPoolObjectData
{
    private readonly LinkedList<TObj> activeObjLinkedList;
    private readonly Subject<TObj> pool;
    private readonly ObjectPool<TObj, TData> factory;

    /// <summary>
    /// コンストラクタ
    /// </summary>
    public ObjectPoolController(
        TObj prefab,
        Transform parent)
    {
        activeObjLinkedList = new LinkedList<TObj>();
        pool = new Subject<TObj>();
        factory = Construct(prefab, parent);

        parent.OnDestroyAsObservable().Subscribe(_ => factory.Dispose());
        pool.Subscribe(obj => factory.Return(obj)).AddTo(parent.gameObject);
    }

    /// <summary>
    /// プールオブジェクトの貸出
    /// </summary>
    public TObj Rent(TData data)
    {
        var obj = factory.Rent();
        activeObjLinkedList.AddLast(obj);
        factory.OnBeforeRent(obj, data);
        return obj;
    }

    /// <summary>
    /// プールオブジェクトの返却
    /// </summary>
    public void Return(TObj obj)
    {
        activeObjLinkedList.Remove(obj);
        pool.OnNext(obj);
    }

    /// <summary>
    /// 全てのプールオブジェクトの返却
    /// </summary>
    public void ReturnAll()
    {
        if (activeObjLinkedList.Count == 0)
        {
            return;
        }

        var objArr = activeObjLinkedList.ToArray();
        for (var i = 0; i < objArr.Length; i++)
        {
            pool.OnNext(objArr[i]);
        }

        activeObjLinkedList.Clear();
    }

    /// <summary>
    /// 破棄
    /// </summary>
    public void Discard()
    {
        ReturnAll();
        factory.Clear();
    }

    private ObjectPool<TObj, TData> Construct(TObj dialog, Transform parent)
    {
        return (ObjectPool<TObj, TData>)typeof(ObjectPool<TObj, TData>)
            .GetConstructor(new[] { typeof(TObj), typeof(Transform) })
            ?.Invoke(new object[] { dialog, parent });
    }
}