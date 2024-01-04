using System;
using UnityEditor;
using UnityEngine;

public class MVPScriptGenerator : ScriptGeneratorBase
{
    private EditorInputField<string> inputField;
    private EditorSaveFolder saveFolder;
    private EditorErrorHelpBox errorHelpBox;

    private string savePath;

    private static readonly string savePathLabel = "保存場所";
    private static readonly string scriptNameLabel = "生成するScript名の要素数";
    private static readonly string templateScriptPath = $"{EditorPathDefine.TemplateScriptFolderPath}/Mvp";
    private static readonly string scriptSaveFolderPath = EditorPathDefine.ScriptSaveFolderPath;

    private static readonly string templateScriptPresenterName = "TEMPLATE_SCRIPT_PRESENTER";
    private static readonly string templateScriptViewName = "TEMPLATE_SCRIPT_VIEW";
    private static readonly string templateScriptModelName = "TEMPLATE_SCRIPT_MODEL";

    private static readonly string savePathSaveKey = "savePathSaveKey";

    public MVPScriptGenerator()
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

            if (scriptName != "")
            {
                var presenterName = $"{scriptName}Presenter";
                var viewName = $"{scriptName}View";
                var modelName = $"{scriptName}Model";

                var templateScriptGenerator =
                    new EditorTemplateScriptGenerator(templateScriptPath, savePath);

                templateScriptGenerator.AddDataReplacing(
                    templateScriptPresenterName, presenterName);
                templateScriptGenerator.AddDataReplacing(
                    templateScriptViewName, viewName);
                templateScriptGenerator.AddDataReplacing(
                    templateScriptModelName, modelName);

                templateScriptGenerator.CreateScript(presenterName, templateScriptPresenterName);
                templateScriptGenerator.CreateScript(viewName, templateScriptViewName);
                templateScriptGenerator.CreateScript(modelName, templateScriptModelName);
            }
        }

        RefreshAssetDatabase();
    }
}