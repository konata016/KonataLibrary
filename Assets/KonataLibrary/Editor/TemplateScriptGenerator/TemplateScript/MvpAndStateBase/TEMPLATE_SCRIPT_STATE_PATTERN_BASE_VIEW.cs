using UnityEngine;

public abstract class TEMPLATE_SCRIPT_STATE_PATTERN_BASE_VIEW : MonoBehaviour
{
    public Transform Transform { get; private set; }
    public Vector3 Position => Transform.position;

    protected abstract void OnInitialize();

    public void Initialize()
    {
        Transform = transform;
        OnInitialize();
    }
}