using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 任意のオブジェクトの表示・非表示を切り替える見た目を持つToggle
/// </summary>
public class ObjectChangeViewToggle : MonoBehaviour
{
    [SerializeField] private Toggle toggle;
    [SerializeField] private GameObject selectedObject;
    [SerializeField] private GameObject unselectedObject;
    
    public RectTransform RectTransform { get; private set; }
    public UniTask OnValueChangedAsync(CancellationToken token) => toggle.OnValueChangedAsync(cancellationToken: token);
    
    private Action<bool> onValueChanged;

    public void Initialize(
        Action<bool> onValueChanged,
        bool isOn,
        bool isSkipFirstCallback = false)
    {
        RectTransform = transform as RectTransform;
        
        SetIsOn(isOn);
        
        this.onValueChanged = onValueChanged;
        toggle.OnValueChanged(OnValueChanged);

        if (!isSkipFirstCallback)
        {
            OnValueChanged(toggle.isOn);
        }
    }

    public void SetIsOn(bool isOn)
    {
        toggle.isOn = isOn;
    }
    
    private void OnValueChanged(bool isOn)
    {
        selectedObject.SetActive(isOn);
        unselectedObject.SetActive(!isOn);
        onValueChanged?.Invoke(isOn);
    }
}
