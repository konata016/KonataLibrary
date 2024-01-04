
public class TEMPLATE_SCRIPT_STATE_PATTERN_MODEL
{
    public readonly StateController<TEMPLATE_ENUM> StateController;

    public TEMPLATE_SCRIPT_STATE_PATTERN_MODEL()
    {
        StateController = new StateController<TEMPLATE_ENUM>();
    }

    public void AddState(
        TEMPLATE_SCRIPT_STATE_BASE state,
        TEMPLATE_SCRIPT_STATE_INFO info,
        TEMPLATE_ENUM type)
    {
        state.Initialize(info, type);
        StateController.AddState(type, state);
    }

    public void ChangeState(TEMPLATE_ENUM type)
    {
        StateController.ChangeState(type);
    }
}