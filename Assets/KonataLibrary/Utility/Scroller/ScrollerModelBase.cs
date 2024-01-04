using UnityEngine;

public abstract class ScrollerModelBase<TCell, TCellData, TFactory>
    where TCell : MonoBehaviour, IScrollerCell<TCellData>
    where TCellData : IScrollerCellData
    where TFactory : ScrollerCellFactoryBase<TCell, TCellData>
{
    public float ContentSizeY { get; private set; }

    protected float currentCellHeight { get; private set; }
    protected float diffPreFramePositionY { get; private set; } // 一つ前の位置
    protected int bottomCurrentCellDataIndex { get; private set; }
    protected int topCurrentCellDataIndex { get; private set; }
    protected TCellData[] cellDataArr { get; private set; }

    private ScrollerCellFactoryController<TCell, TCellData, TFactory> factoryController;

    private float bottomCellTopHeight;
    private bool isClear;
    private Vector2 cellSize;
    
    public TCell FirstCell => factoryController.FirstCell;
    public TCell LastCell => factoryController.LastCell;
    private int maxViewCellData => startVerticalCellCount * horizontalCellCount;
    
    /// <summary>スクロール感度</summary>
    public abstract float ScrollSensitivity { get; }
    
    /// <summary>初回の縦のcellの数</summary>
    protected abstract int startVerticalCellCount { get; }

    /// <summary>横のcellの数</summary>
    protected abstract int horizontalCellCount { get; }

    /// <summary>横のcell間のスペース</summary>
    protected abstract float horizontalCellSpace { get; }

    /// <summary>縦のcell間のスペース</summary>
    protected abstract float verticalCellSpace { get; }

    public ScrollerModelBase(TCell cell, Transform parent)
    {
        factoryController = new ScrollerCellFactoryController<TCell, TCellData, TFactory>(cell, parent);
    }

    /// <summary>
    /// エントリー
    /// </summary>
    public void Entry(TCellData[] cellDataArr)
    {
        this.cellDataArr = cellDataArr;
        var count = maxViewCellData;
        for (var i = 0; i < count; i++)
        {
            RentLastCell(i);
        }

        cellSize = factoryController.FirstCell.RectTransform.sizeDelta;
        currentCellHeight = cellSize.y + verticalCellSpace;
        bottomCurrentCellDataIndex = count;
        topCurrentCellDataIndex = 0;
        diffPreFramePositionY = 0;

        var bottomCellTopIndex = cellDataArr.Length - startVerticalCellCount;
        bottomCellTopHeight = -currentCellHeight * Mathf.Ceil((float)bottomCellTopIndex / (float)horizontalCellCount);
        ContentSizeY = currentCellHeight * Mathf.Ceil((float)cellDataArr.Length / (float)horizontalCellCount);
        
        isClear = false;
    }

    /// <summary>
    /// データの更新
    /// </summary>
    public void UpdateData(float contentPositionY)
    {
        if (isClear)
        {
            return;
        }
        
        // 下にスクロールした時
        if (bottomCurrentCellDataIndex < cellDataArr.Length)
        {
            while (-contentPositionY - diffPreFramePositionY < -currentCellHeight * 2)
            {
                diffPreFramePositionY -= currentCellHeight;
                if(diffPreFramePositionY < bottomCellTopHeight)
                {
                    diffPreFramePositionY = bottomCellTopHeight;
                    break;
                }

                for (var i = 0; i < horizontalCellCount; i++)
                {
                    if (bottomCurrentCellDataIndex > cellDataArr.Length - 1)
                    {
                        break;
                    }

                    factoryController.ReturnFirstCell();
                    //Debug.Log($"diffPreFramePositionY {diffPreFramePositionY} {bottomCellTopHeight}");

                    RentLastCell(bottomCurrentCellDataIndex);
                    topCurrentCellDataIndex++;
                    bottomCurrentCellDataIndex++;
                }
            }
        }

        // 上にスクロールした時
        if (topCurrentCellDataIndex > 0)
        {
            while (-contentPositionY - diffPreFramePositionY > 0)
            {
                diffPreFramePositionY += currentCellHeight;
                if(diffPreFramePositionY > 0)
                {
                    diffPreFramePositionY = 0;
                    return;
                }

                for (var i = 0; i < horizontalCellCount; i++)
                {
                    if (topCurrentCellDataIndex <= 0)
                    {
                        return;
                    }

                    if (bottomCurrentCellDataIndex < maxViewCellData)
                    {
                        break;
                    }

                    factoryController.ReturnLastCell();
                    //Debug.Log($"diffPreFramePositionY {diffPreFramePositionY}");

                    topCurrentCellDataIndex--;
                    bottomCurrentCellDataIndex--;

                    RentFirstCell(topCurrentCellDataIndex);
                }
            }
        }
    }
    
    /// <summary>
    /// 破棄
    /// </summary>
    public void Discard()
    {
        topCurrentCellDataIndex = 0;
        diffPreFramePositionY = 0;
        factoryController.ReturnCellAll();
        isClear = true;
    }
    
    /// <summary>
    /// Indexからcellの位置を取得
    /// </summary>
    public float GetCellPositionY(int index, float contentHeight, float scrollHeight) 
    {
        var sizeY = cellSize.y + verticalCellSpace;
        var vertical = Mathf.Floor(index / horizontalCellCount) * sizeY;
        var normalizedPosition = 1f - (vertical / (contentHeight - scrollHeight));
        return Mathf.Clamp01(normalizedPosition);;
    }

    /// <summary>
    ///  cellを借りる（後尾の列に格納）
    /// </summary>
    private void RentLastCell(int dataIndex)
    {
        if(dataIndex >= cellDataArr.Length || dataIndex < 0)
        {
            return;
        }

        var cell = factoryController.RentLast(cellDataArr[dataIndex]);
        SetupCell(dataIndex, cell);
    }

    /// <summary>
    /// cellを借りる（先頭のListに格納）
    /// </summary>
    private void RentFirstCell(int dataIndex)
    {
        if(dataIndex >= cellDataArr.Length || dataIndex < 0)
        {
            return;
        }
        
        var cell = factoryController.RentFirst(cellDataArr[dataIndex]);
        SetupCell(dataIndex, cell);
    }

    /// <summary>
    /// cellの設定
    /// </summary>
    private void SetupCell(int dataIndex, TCell cell)
    {
        var cellSize = cell.RectTransform.sizeDelta;
        var sizeX = cellSize.x + horizontalCellSpace;
        var sizeY = cellSize.y + verticalCellSpace;
        var horizontal = dataIndex % horizontalCellCount * sizeX;
        var vertical = Mathf.Floor(dataIndex / horizontalCellCount) * sizeY;
        cell.RectTransform.anchoredPosition = new Vector2(horizontal, -vertical);
    }
}
