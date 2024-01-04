using UnityEngine;
using UniRx.Toolkit;

public abstract class ScrollerCellFactoryBase<TCell, TCellData> : ObjectPool<TCell>
    where TCell : MonoBehaviour, IScrollerCell<TCellData>
    where TCellData : IScrollerCellData
{
    private readonly TCell prefab;
    private readonly Transform parent;

    public abstract void OnInitializeCell(TCell cell);


    public ScrollerCellFactoryBase(TCell prefab, Transform parent)
    {
        this.prefab = prefab;
        this.parent = parent;
    }

    public void OnBeforeRent(TCell cell, TCellData data)
    {
        cell.Setup(data);
        OnInitializeCell(cell);
    }

    protected override TCell CreateInstance()
    {
        var obj = MonoBehaviour.Instantiate(prefab, parent);
        obj.Initialize();
        // obj.RectTransform.anchorMax = new Vector2(0.5f, 1f);
        // obj.RectTransform.anchorMin = new Vector2(0.5f, 1f);
        // obj.RectTransform.pivot = new Vector2(0.5f, 1f);
        // obj.RectTransform.anchoredPosition = Vector2.zero;
        return obj;
    }

    protected override void OnBeforeRent(TCell cell)
    {
        cell.Show();
    }

    protected override void OnBeforeReturn(TCell cell)
    {
        cell.Hide();
    }

    protected override void OnClear(TCell cell)
    {
        cell.Discard();
    }
}
