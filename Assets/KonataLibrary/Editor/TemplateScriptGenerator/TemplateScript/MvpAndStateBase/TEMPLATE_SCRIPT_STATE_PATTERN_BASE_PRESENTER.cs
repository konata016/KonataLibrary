using UnityEngine;

public abstract class TEMPLATE_SCRIPT_STATE_PATTERN_BASE_PRESENTER<TModel, TView> : MonoBehaviour
    where TModel : TEMPLATE_SCRIPT_STATE_PATTERN_BASE_MODEL
    where TView : TEMPLATE_SCRIPT_STATE_PATTERN_BASE_VIEW
{
    [field: SerializeField] public TView View { get; private set; }
    public TModel Model { get; protected set; }


    protected abstract void OnInitialize();
    protected abstract void OnDiscarding();
    protected abstract void OnBindState(TEMPLATE_SCRIPT_STATE_BASE_INFO<TModel, TView> info);

    public void Initialize()
    {
        OnInitialize();

        var info = new TEMPLATE_SCRIPT_STATE_BASE_INFO<TModel, TView>(Model, View);
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

    private void BindState(TEMPLATE_SCRIPT_STATE_BASE_INFO<TModel, TView> info)
    {
        OnBindState(info);
    }
}