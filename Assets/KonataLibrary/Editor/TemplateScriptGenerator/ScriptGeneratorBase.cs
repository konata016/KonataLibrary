using UnityEditor;

public abstract class ScriptGeneratorBase
{
    private bool isRefresh;

    public abstract void OnGUI();

    public void SetRefreshActive(bool isActive)
    {
        isRefresh = isActive;
    }

    protected void RefreshAssetDatabase()
    {
        if (!isRefresh)
        {
            return;
        }

        AssetDatabase.Refresh();
    }
}