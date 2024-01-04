using System.Linq;
using System.Collections.Generic;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

public class ScrollerCellFactoryController<TCell, TCellData, TFactory>
    where TCell : MonoBehaviour, IScrollerCell<TCellData>
    where TCellData : IScrollerCellData
    where TFactory : ScrollerCellFactoryBase<TCell, TCellData>
{
    private readonly LinkedList<TCell> activeCellLinkedList;
    private readonly TFactory factory;
    private readonly Subject<TCell> pool;

    public TCell FirstCell => activeCellLinkedList.First?.Value;
    public TCell LastCell => activeCellLinkedList.Last?.Value;


    public ScrollerCellFactoryController(TCell prefab, Transform parent)
    {
        activeCellLinkedList = new LinkedList<TCell>();
        pool = new Subject<TCell>();
        factory = Construct(prefab, parent);

        parent.OnDestroyAsObservable().Subscribe(_ => factory.Dispose());
        pool.Subscribe(obj => factory.Return(obj)).AddTo(parent.gameObject);
    }

    /// <summary>
    /// Cellを貸す
    /// Cellが存在していない場合は生成を行う
    /// </summary>
    public TCell RentLast(TCellData cellData)
    {
        var cell = factory.Rent();
        activeCellLinkedList.AddLast(cell);
        factory.OnBeforeRent(cell, cellData);
        return cell;
    }

    /// <summary>
    /// Cellを貸す
    /// Cellが存在していない場合は生成を行う
    /// </summary>
    public TCell RentFirst(TCellData cellData)
    {
        var cell = factory.Rent();
        activeCellLinkedList.AddFirst(cell);
        factory.OnBeforeRent(cell, cellData);
        return cell;
    }

    /// <summary>
    /// Cellを返す
    /// 破棄はしない
    /// </summary>
    public void ReturnFirstCell()
    {
        var cell = activeCellLinkedList.First.Value;
        activeCellLinkedList.RemoveFirst();
        pool.OnNext(cell);
    }

    /// <summary>
    /// Cellを返す
    /// 破棄はしない
    /// </summary>
    public void ReturnLastCell()
    {
        var cell = activeCellLinkedList.Last.Value;
        activeCellLinkedList.RemoveLast();
        pool.OnNext(cell);
    }

    /// <summary>
    /// Cellを返す
    /// 破棄はしない
    /// </summary>
    public void ReturnCell(TCell cell)
    {
        activeCellLinkedList.Remove(cell);
        pool.OnNext(cell);
    }

    /// <summary>
    /// Cellをすべて返す
    /// 破棄はしない
    /// </summary>
    public void ReturnCellAll()
    {
        if (activeCellLinkedList.Count == 0)
        {
            return;
        }

        var cellArr = activeCellLinkedList.ToArray();
        for (var i = 0; i < cellArr.Length; i++)
        {
            pool.OnNext(cellArr[i]);
        }

        activeCellLinkedList.Clear();
    }


    private TFactory Construct(TCell prefab, Transform parent)
    {
        return (TFactory)typeof(TFactory)
            .GetConstructor(new[] { typeof(TCell), typeof(Transform) })
            ?.Invoke(new object[] { prefab, parent });
    }
}
