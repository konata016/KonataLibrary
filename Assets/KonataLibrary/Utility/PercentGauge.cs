using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PercentGauge : MonoBehaviour
{
    [SerializeField] private Image gaugeImage;
    [SerializeField] private TextMeshProUGUI gaugeText;

    private Range<float> range;
    private float value;
    
    public float Value => value;
    public float Max => range.Max;
    public float Min => range.Min;

    public bool IsMax => value >= range.Max;

    public void Initialize(Range<float> range, float value)
    {
        this.range = range;
        UpdateValue(value);
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }
    
    public void Hide()
    {
        gameObject.SetActive(false);
    }
    
    public void UpdateValue(float value)
    {
        this.value = Mathf.Clamp(value, range.Min, range.Max);
        UpdateView();
    }
    
    private void UpdateView()
    {
        gaugeImage.fillAmount = value / range.Max;
        gaugeText.text = $"{value / range.Max * 100f:0'%'}";
    }
}
