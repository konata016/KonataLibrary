using System;
using UnityEngine;
using UnityEngine.UI;

public abstract class ScrollerViewBase : MonoBehaviour
{
    [field: SerializeField] protected ScrollRect scrollRect { get; private set; }
    
    public RectTransform Transform { get; private set; }
    public Vector3 Position => Transform.position;
    public RectTransform Content => scrollRect.content;
    public RectTransform Viewport => scrollRect.viewport;

    public void Initialize()
    {
        Transform = transform as RectTransform;
    }
    
    public void RegisterScrollRect(Action onValueChanged)
    {
        scrollRect.onValueChanged.RemoveAllListeners();
        scrollRect.onValueChanged.AddListener(_ => onValueChanged?.Invoke());
    }
    
    public void EnableScroll(bool enable)
    {
        scrollRect.enabled = enable;
    }

    public void SetScrollSensitivity(float sensitivity)
    {
        scrollRect.scrollSensitivity = sensitivity;
    }

    public void SetContentSizeY(float y)
    {
        var sizeDelta = Content.sizeDelta;
        sizeDelta.y = y;
        Content.sizeDelta = sizeDelta;
    }
    
    public void MoveScrollToTop()
    {
        scrollRect.verticalNormalizedPosition = 1;
    }
    
    public void MoveScroll(float y)
    {
        scrollRect.verticalNormalizedPosition = y;
    }

    public void Discard()
    {
        SetContentSizeY(0);
    }
}
