using UnityEngine;

public class EmptyPoolObject : PoolObjectBase<EmptyPoolObjectData>
{
    public Transform Transform { get; private set; }
    
    public Vector3 Position
    {
        get => Transform.position;
        set => Transform.position = value;
    }
    
    public Vector3 LocalPosition
    {
        get => Transform.localPosition;
        set => Transform.localPosition = value;
    }
    
    public Quaternion Rotation
    {
        get => Transform.rotation;
        set => Transform.rotation = value;
    }
    
    public Quaternion LocalRotation
    {
        get => Transform.localRotation;
        set => Transform.localRotation = value;
    }
    
    public Vector3 LocalScale
    {
        get => Transform.localScale;
        set => Transform.localScale = value;
    }

    public override void Initialize()
    {
        Transform = transform;
    }

    public override void OnEntry(EmptyPoolObjectData data)
    {
    }

    public override void OnExit()
    {
    }

    public override void Discard()
    {
    }
    
    public void SetParent(Transform parent)
    {
        Transform.SetParent(parent);
    }
}