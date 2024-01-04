using UnityEditor;

public class ScriptGeneratorEditorWindow : EditorWindow
{
    private EditorPullDown pullDown;
    private EditorErrorHelpBox errorHelpBox;

    private ScriptGeneratorBase scriptGenerator;
    private ScriptGeneratorBase mvpScriptGenerator;
    private ScriptGeneratorBase mvpAndStatePatternScriptGenerator;
    private ScriptGeneratorBase mvpAndStateBasePatternScriptGenerator;
    private ScriptGeneratorBase scrollerScriptGenerator;

    private int currentPullDownIndex;
    private bool isRefreshAfterCreation;

    private static readonly string refreshToggle = "Script作成後にリフレッシュを行うか";
    private static readonly string pullDownLabel = "Editorの種類";
    private static readonly string selectedPullDownIndexSaveKey = "ScriptGeneratorEditorWindow_SelectedPullDownIndexSaveKey";

    private static readonly string[] pullDownLabelArr = new[]
    {
        "TemplateScriptの種類を選ぶ作成方法",
        "MVPの作成を行う",
        "MVP・Stateパターンの作成を行う(通常)",
        "MVP・Stateパターンの作成を行う(ベース)",
        "スクローラーの作成を行う",
    };

    [MenuItem("Tools/ScriptGeneratorEditorWindow")]
    public static void Open()
    {
        GetWindow(typeof(ScriptGeneratorEditorWindow));
    }

    private void OnEnable()
    {
        isRefreshAfterCreation = true;

        scriptGenerator = new ScriptGenerator();
        mvpScriptGenerator = new MVPScriptGenerator();
        mvpAndStatePatternScriptGenerator = new MVPAndStatePatternScriptGenerator();
        mvpAndStateBasePatternScriptGenerator = new MVPAndStateBasePatternScriptGenerator();
        scrollerScriptGenerator = new ScrollerScriptGenerator();

        pullDown = new EditorPullDown(pullDownLabel, selectedPullDownIndexSaveKey, pullDownLabelArr);

        errorHelpBox = new EditorErrorHelpBox(UpdateView);
        errorHelpBox.Add("Editorの種類が選ばれていない", () => currentPullDownIndex == -1);
    }

    private void OnGUI()
    {
        EditorGUILayout.LabelField("");
        isRefreshAfterCreation = EditorGUILayout.Toggle(refreshToggle, isRefreshAfterCreation);

        EditorGUILayout.LabelField("");
        currentPullDownIndex = pullDown.GetPullDownIndex();

        EditorGUILayout.LabelField("");
        errorHelpBox.UpdateView();
    }

    private void UpdateView()
    {
        var generator = currentPullDownIndex switch
        {
            0 => scriptGenerator,
            1 => mvpScriptGenerator,
            2 => mvpAndStatePatternScriptGenerator,
            3 => mvpAndStateBasePatternScriptGenerator,
            4 => scrollerScriptGenerator,
        };

        generator.SetRefreshActive(isRefreshAfterCreation);
        generator.OnGUI();
    }
}