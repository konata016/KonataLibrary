using UnityEngine;

namespace OutGame
{
    public class TEMPLATE_SCRIPT_CELL : MonoBehaviour, IScrollerCell<TEMPLATE_SCRIPT_CELL_DATA>
    {
        public RectTransform RectTransform { get; private set; }

        public void Initialize()
        {
            RectTransform = transform as RectTransform;
        }

        public void Setup(TEMPLATE_SCRIPT_CELL_DATA data)
        {
        }

        public void Show()
        {
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }

        public void Discard()
        {
        }
    }
}