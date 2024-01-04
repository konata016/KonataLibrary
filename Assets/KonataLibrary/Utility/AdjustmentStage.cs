using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;

[CanEditMultipleObjects, CustomEditor(typeof(AdjustmentStage))]
public class AdjustmentStageEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        
        var exampleScript = target as AdjustmentStage;

        if (GUILayout.Button("調整"))
        {
            Debug.Log("調整");
            
            exampleScript.Adjust();
        }
        
        if (GUILayout.Button("円状に調整"))
        {
            Debug.Log("円状に調整");
            
            exampleScript.AdjustCircle();
        }
    }
}

#endif

/// <summary>
/// オブジェクト群の位置の調整を行うScript
/// </summary>
public class AdjustmentStage : MonoBehaviour
{
    private enum ExecutionType
    {
        None,
        Normal,
        Circle
    }
    
    [SerializeField] private Transform _root;
    
    [Header("子オブジェクトのAdjustmentStageを実行するか")]
    [SerializeField] private bool _executeChildObjectAdjustmentStage;
    
    [Header("Position")]
    [SerializeField] private float _space;
    [SerializeField] private Vector3 _axis;
    [SerializeField] private bool _isCenter;
    
    [Header("円状に並べる")]
    [SerializeField] private float _radius;
    
    [Header("キャッシュ用")]
    [SerializeField, HideInInspector] private ExecutionType _executionType;

#if UNITY_EDITOR
    void Reset()
    {
        _root = transform;
    }

    /// <summary>
    /// 直列にオブジェクト群の位置を調整する
    /// </summary>
    public void Adjust()
    {
        _executionType = ExecutionType.Normal;
        for (var i = 0; i < _root.childCount; i++)
        {
            var child = _root.GetChild(i).transform;

            if (_isCenter)
            {
                var adjusted = i - (_root.childCount - 1) * 0.5f;
                var dis = adjusted * _space;
                child.localPosition = _axis * dis;
            }
            else
            {
                child.localPosition = _axis * (_space * i);
            }

            AdjustChildObject(child);
        }
    }

    /// <summary>
    /// 円状にオブジェクト群の位置を調整する
    /// </summary>
    public void AdjustCircle()
    {
        _executionType = ExecutionType.Circle;
        
        const float angle = 360f;
        for (var i = 0; i < _root.childCount; i++)
        {
            var child = _root.GetChild(i).transform;
            var r = (angle / _root.childCount) * i;
            r *= Mathf.Deg2Rad;
            var pos = new Vector3(_radius * Mathf.Cos(r), 0f, _radius * Mathf.Sin(r));
            child.localPosition = pos;
            
            AdjustChildObject(child);
        }
    }

    /// <summary>
    /// AdjustmentStageを持つオブジェクトの位置調整を実行する
    /// </summary>
    private void AdjustChildObject(Transform child)
    {
        if (!_executeChildObjectAdjustmentStage)
        {
            return;
        }
        
        var adjustmentStage = child.GetComponent<AdjustmentStage>();
        if (adjustmentStage == null)
        {
            return;
        }
        
        switch (adjustmentStage._executionType)
        {
            case ExecutionType.Normal:
                adjustmentStage.Adjust();
                break;
            case ExecutionType.Circle:
                adjustmentStage.AdjustCircle();
                break;
        }
    }
#endif
}