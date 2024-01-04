
public abstract class TEMPLATE_SCRIPT_STATE_BASE : IState<TEMPLATE_ENUM>
{
    public StateController<TEMPLATE_ENUM> StateController { get; private set; }
    public TEMPLATE_ENUM StateType { get; private set; }

    protected TEMPLATE_SCRIPT_STATE_PATTERN_MODEL model { get; private set; }
    protected TEMPLATE_SCRIPT_STATE_PATTERN_VIEW view { get; private set; }

    protected bool isCurrentStateType => StateType == StateController.CurrentStateType;

    protected abstract void OnInitialize();
    protected abstract void OnStateEnter();
    protected abstract void OnStateUpdate();
    protected abstract void OnStateExit();

    public void Initialize(
        TEMPLATE_SCRIPT_STATE_INFO info,
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
