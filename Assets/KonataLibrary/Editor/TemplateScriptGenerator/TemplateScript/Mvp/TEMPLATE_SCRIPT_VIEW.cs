using UnityEngine;

public class TEMPLATE_SCRIPT_VIEW : MonoBehaviour
{
    public Transform Transform { get; private set; }
    public Vector3 Position => Transform.position;

    public void Initialize()
    {
        Transform = transform;
    }
}
