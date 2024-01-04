
public abstract class TEMPLATE_SCRIPT_STATE_BASE_BASE<TModel, TView> : IState<TEMPLATE_ENUM>
    where TModel : TEMPLATE_SCRIPT_STATE_PATTERN_BASE_MODEL
    where TView : TEMPLATE_SCRIPT_STATE_PATTERN_BASE_VIEW
{
    public StateController<TEMPLATE_ENUM> StateController { get; private set; }
    public TEMPLATE_ENUM StateType { get; private set; }

    protected TModel model { get; private set; }
    protected TView view { get; private set; }

    protected bool isCurrentStateType => StateType == StateController.CurrentStateType;

    protected abstract void OnInitialize();
    protected abstract void OnStateEnter();
    protected abstract void OnStateUpdate();
    protected abstract void OnStateExit();

    public void Initialize(
        TEMPLATE_SCRIPT_STATE_BASE_INFO<TModel, TView> info,
        TEMPLATE_ENUM stateType)
    {
        model = info.Model;
        view = info.View;
        StateController = model.StateController;

        StateType = stateType;

        OnInitialize();
    }

    public void OnEnter()
    {
        OnStateEnter();
    }

    public void OnUpdate()
    {
        OnStateUpdate();
    }

    public void OnExit()
    {
        OnStateExit();
    }
}
