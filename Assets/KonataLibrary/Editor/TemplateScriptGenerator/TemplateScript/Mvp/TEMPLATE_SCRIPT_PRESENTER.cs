using UnityEngine;

public class TEMPLATE_SCRIPT_PRESENTER : MonoBehaviour
{
    [field: SerializeField] public TEMPLATE_SCRIPT_VIEW View { get; private set; }
    public TEMPLATE_SCRIPT_MODEL Model { get; private set; }

    private void Awake()
    {
        Initialize();
    }

    public void Initialize()
    {
        Model = new TEMPLATE_SCRIPT_MODEL();
        View.Initialize();
    }
}
