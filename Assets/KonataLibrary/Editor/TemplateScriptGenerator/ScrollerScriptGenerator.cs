using System;
using UnityEditor;
using UnityEngine;

public class ScrollerScriptGenerator : ScriptGeneratorBase
{
    private EditorInputField<string> inputField;
    private EditorSaveFolder saveFolder;
    private EditorErrorHelpBox errorHelpBox;

    private string savePath;

    private static readonly string savePathLabel = "保存場所";
    private static readonly string scriptNameLabel = "生成するScript名の要素数";
    private static readonly string templateScriptPath = $"{EditorPathDefine.TemplateScriptFolderPath}/Scroller";
    private static readonly string scriptSaveFolderPath = EditorPathDefine.ScriptSaveFolderPath;

    private static readonly string templateScriptCellName = "TEMPLATE_SCRIPT_CELL";
    private static readonly string templateScriptCellDataName = "TEMPLATE_SCRIPT_CELL_DATA";
    private static readonly string templateScriptCellFactoryName = "TEMPLATE_SCRIPT_CELL_FACTORY";
    private static readonly string templateScrollerModelName = "TEMPLATE_SCRIPT_SCROLLER_MODEL";
    private static readonly string templateScrollerViewName = "TEMPLATE_SCRIPT_SCROLLER_VIEW";
    private static readonly string templateScrollerPresenterName = "TEMPLATE_SCRIPT_SCROLLER_PRESENTER";

    private static readonly string savePathSaveKey = "savePathSaveKey";

    public ScrollerScriptGenerator()
    {
        saveFolder = new EditorSaveFolder(savePathLabel, savePathSaveKey, scriptSaveFolderPath);
        inputField = new EditorInputField<string>(scriptNameLabel);

        errorHelpBox = new EditorErrorHelpBox(CreateScript);
        errorHelpBox.Add("保存場所が指定されていない", () => String.IsNullOrEmpty(savePath));
        errorHelpBox.Add("script名が指定されていない", () => inputField.Arr == null || inputField.Arr.Length == 0);
        errorHelpBox.Add("script名が重複している", () => inputField.IsDuplicate);
    }

    public override void OnGUI()
    {
        EditorGUILayout.LabelField("");
        inputField.UpdateView((name, index) =>
            EditorGUILayout.TextField($"{index}", name));

        EditorGUILayout.LabelField("");
        savePath = saveFolder.GetSavePath();

        EditorGUILayout.LabelField("");
        errorHelpBox.UpdateView();
    }
    
    private void CreateScript()
    {
        if (!GUILayout.Button("script作成"))
        {
            return;
        }

        for (int i = 0; i < inputField.Arr.Length; i++)
        {
            var scriptName = inputField.Arr[i];

            if (scriptName == "")
            {
                continue;
            }

            var cellName = $"{scriptName}Cell";
            var cellDataName = $"{scriptName}CellData";
            var cellFactoryName = $"{scriptName}CellFactory";
            var scrollerModelName = $"{scriptName}ScrollerModel";
            var scrollerViewName = $"{scriptName}ScrollerView";
            var scrollerPresenterName = $"{scriptName}ScrollerPresenter";

            var templateScriptGenerator =
                new EditorTemplateScriptGenerator(templateScriptPath, savePath);

            templateScriptGenerator.AddDataReplacing(templateScriptCellName, cellName);
            templateScriptGenerator.AddDataReplacing($"{cellName}_DATA", cellDataName);
            templateScriptGenerator.AddDataReplacing($"{cellName}_FACTORY", cellFactoryName);
            templateScriptGenerator.AddDataReplacing(templateScrollerModelName, scrollerModelName);
            templateScriptGenerator.AddDataReplacing(templateScrollerViewName, scrollerViewName);
            templateScriptGenerator.AddDataReplacing(templateScrollerPresenterName, scrollerPresenterName);

            templateScriptGenerator.CreateScript(cellName, templateScriptCellName);
            templateScriptGenerator.CreateScript(cellDataName, templateScriptCellDataName);
            templateScriptGenerator.CreateScript(cellFactoryName, templateScriptCellFactoryName);
            templateScriptGenerator.CreateScript(scrollerModelName, templateScrollerModelName);
            templateScriptGenerator.CreateScript(scrollerViewName, templateScrollerViewName);
            templateScriptGenerator.CreateScript(scrollerPresenterName, templateScrollerPresenterName);
        }

        RefreshAssetDatabase();
    }
}
