using UnityEngine;

public abstract class ScrollerPresenterBase<TCell, TCellData, TFactory> : MonoBehaviour
    where TCell : MonoBehaviour, IScrollerCell<TCellData>
    where TCellData : IScrollerCellData
    where TFactory : ScrollerCellFactoryBase<TCell, TCellData>
{
    protected ScrollerModelBase<TCell, TCellData, TFactory> model;
    protected abstract ScrollerModelBase<TCell, TCellData, TFactory> Model { get; }
    protected abstract ScrollerViewBase View { get; }
    protected abstract void OnInitialize();
    protected abstract void OnEntry();
    protected abstract void OnUpdated();

    public void Initialize()
    {
        model = Model;
        View.Initialize();
        View.SetScrollSensitivity(model.ScrollSensitivity);
        OnInitialize();
    }

    public void Entry(TCellData[] cellDataArr)
    {
        model.Entry(cellDataArr);
        View.SetContentSizeY(model.ContentSizeY);
        OnEntry();
    }

    public void OnUpdate()
    {
        model.UpdateData(View.Content.anchoredPosition.y);
        OnUpdated();
    }
    
    public void Discard()
    {
        model.Discard();
        View.Discard();
    }
    
    /// <summary>
    /// 表示・非表示を切り替える
    /// </summary>
    public void SetActive(bool isActive)
    {
        gameObject.SetActive(isActive);
    }
    
    /// <summary>
    /// スクロールを有効にする
    /// </summary>
    public void EnableScroll(bool enable)
    {
        View.EnableScroll(enable);
    }

    /// <summary>
    /// スクロールを一番上に戻す
    /// </summary>
    public void MoveScrollToTop()
    {
        View.MoveScrollToTop();   
    }

    /// <summary>
    /// Indexから特定のセルの位置にスクロールする
    /// </summary>
    public void MoveScrollTargetCell(int index)
    {
        var contentHeight = View.Content.rect.height;
        var viewportHeight = View.Viewport.rect.height;
        View.MoveScroll(model.GetCellPositionY(index, contentHeight, viewportHeight));
    }
}
