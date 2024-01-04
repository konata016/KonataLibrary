using UnityEngine;

public class TEMPLATE_SCRIPT_STATE_PATTERN_PRESENTER : MonoBehaviour
{
    [field: SerializeField] public TEMPLATE_SCRIPT_STATE_PATTERN_VIEW View { get; private set; }
    public TEMPLATE_SCRIPT_STATE_PATTERN_MODEL Model { get; private set; }

    private void Awake()
    {
        Initialize();
    }

    public void Initialize()
    {
        Model = new TEMPLATE_SCRIPT_STATE_PATTERN_MODEL();
        View.Initialize();

        var info = new TEMPLATE_SCRIPT_STATE_INFO(Model, View);

        BindState(info);

        Model.ChangeState(TEMPLATE_ENUM.Standby);
    }

    public void OnUpdate()
    {
        Model.StateController.OnStateUpdate();
    }

    public void Discard()
    {
        Model.ChangeState(TEMPLATE_ENUM.Discarding);
    }

    private void BindState(TEMPLATE_SCRIPT_STATE_INFO info)
    {
        Model.AddState(new TEMPLATE_SCRIPT_STATE_TEMP(), info, TEMPLATE_ENUM.Standby);
    }
}