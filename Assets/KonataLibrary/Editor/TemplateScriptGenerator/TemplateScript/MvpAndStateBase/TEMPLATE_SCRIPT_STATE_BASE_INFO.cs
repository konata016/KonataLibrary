public class TEMPLATE_SCRIPT_STATE_BASE_INFO<TModel, TView>
    where TModel : TEMPLATE_SCRIPT_STATE_PATTERN_BASE_MODEL
    where TView : TEMPLATE_SCRIPT_STATE_PATTERN_BASE_VIEW
{
    public readonly TModel Model;
    public readonly TView View;

    public TEMPLATE_SCRIPT_STATE_BASE_INFO(
        TModel model,
        TView view)
    {
        Model = model;
        View = view;
    }

    public TEMPLATE_SCRIPT_STATE_BASE_INFO<
        TEMPLATE_SCRIPT_STATE_PATTERN_BASE_MODEL,
        TEMPLATE_SCRIPT_STATE_PATTERN_BASE_VIEW> GetInfo()
    {
        return new TEMPLATE_SCRIPT_STATE_BASE_INFO<
            TEMPLATE_SCRIPT_STATE_PATTERN_BASE_MODEL,
            TEMPLATE_SCRIPT_STATE_PATTERN_BASE_VIEW>(
            Model,
            View);
    }
}
