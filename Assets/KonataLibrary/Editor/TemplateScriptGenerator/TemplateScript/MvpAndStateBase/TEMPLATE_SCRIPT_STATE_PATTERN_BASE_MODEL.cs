
public abstract class TEMPLATE_SCRIPT_STATE_PATTERN_BASE_MODEL
{
    public readonly StateController<TEMPLATE_ENUM> StateController;

    public TEMPLATE_SCRIPT_STATE_PATTERN_BASE_MODEL()
    {
        StateController = new StateController<TEMPLATE_ENUM>();
    }

    public void AddState<TModel, TView>(
        TEMPLATE_SCRIPT_STATE_BASE_BASE<TModel, TView> state,
        TEMPLATE_SCRIPT_STATE_BASE_INFO<TModel, TView> info,
        TEMPLATE_ENUM type)
        where TModel : TEMPLATE_SCRIPT_STATE_PATTERN_BASE_MODEL
        where TView : TEMPLATE_SCRIPT_STATE_PATTERN_BASE_VIEW
    {
        state.Initialize(info, type);
        StateController.AddState(type, state);
    }


    public void ChangeState(TEMPLATE_ENUM type)
    {
        StateController.ChangeState(type);
    }
}