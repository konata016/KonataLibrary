using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class UiUvScroll : MonoBehaviour
{
    [SerializeField] private Material material;
    [SerializeField] private MaskableGraphic image;
    [SerializeField] private Vector2 step;
    [SerializeField] private float duration;
    
    private Tweener tweener;

    #if UNITY_EDITOR
    private void Reset()
    {
        image = GetComponent<MaskableGraphic>();
    }
    #endif

    private void Awake()
    {
        image.material = material;
        tweener?.Kill();
        tweener = material
            .DOOffset(step, duration)
            .SetEase(Ease.Linear)
            .SetLoops(-1, LoopType.Restart);
    }
}
