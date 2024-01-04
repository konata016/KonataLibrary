using UnityEngine;

public interface IScrollerCell<T> where T : IScrollerCellData
{
    public RectTransform RectTransform { get; }
    public void Initialize();

    public void Setup(T data);

    public void Show();

    public void Hide();

    public void Discard();
}