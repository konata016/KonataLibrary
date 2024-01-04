using System.Linq;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class CircleGauge : MonoBehaviour
{
    [SerializeField] Image gaugeImage;
    
    private Range<float>[] rangeArr;
    private int currentRangeIndex;
    private float time;
    private Tweener gaugeAnimation;

    public void Initialize(Range<float>[] rangeArr, float time)
    {
        this.time = time;
        this.rangeArr = rangeArr.OrderBy(range => range.Max).Take(rangeArr.Length - 1).ToArray();
        gaugeImage.fillAmount = 0;
    }

    public void UpdateView(float currentValue, bool isAnimation = true)
    {
        gaugeAnimation?.Kill();
        currentRangeIndex = GetRangeIndex(currentValue);
        
        var min= rangeArr[currentRangeIndex].Min;
        var max = rangeArr[currentRangeIndex].Max;
        var value = (currentValue - min) / (max - min);
        
        if (isAnimation)
        {
            gaugeAnimation = gaugeImage.DOFillAmount(value, time).SetEase(Ease.Linear);
        }
        else
        {
            gaugeImage.fillAmount = value;
        }
    }
    
    private int GetRangeIndex(float value)
    {
        var index = -1;
        for (var i = 0; i < rangeArr.Length; i++)
        {
            var range = rangeArr[i];
            if (value <= range.Max)
            {
                index = i;
                break;
            }
        }

        return index == -1 ? rangeArr.Length - 1 : index;
    }
}
