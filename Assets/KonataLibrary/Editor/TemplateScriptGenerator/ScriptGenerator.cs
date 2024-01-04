using System;
using System.IO;
using UnityEditor;
using UnityEngine;

public class ScriptGenerator : ScriptGeneratorBase
{
    private EditorInputField<string> inputField;
    private EditorPullDown templateScriptPullDown;
    private EditorPullDown templateFolderPullDown;
    private EditorSaveFolder saveFolder;
    private EditorErrorHelpBox errorHelpBox;

    private int currentTemplateScriptPullDownIndex;
    private int currentTemplateFolderPullDownIndex;
    private string savePath;
    private string folderName;

    private static readonly string savePathLabel = "保存場所";
    private static readonly string scriptNameLabel = "生成するScript名の要素数";
    private static readonly string templateScriptPullDownLabel = "作成するScriptの種類";
    private static readonly string templateFolderPullDownLabel = "作成するFolderの種類";
    private static readonly string templateScriptPath = $"{EditorPathDefine.TemplateScriptFolderPath}";
    private static readonly string scriptSaveFolderPath = EditorPathDefine.ScriptSaveFolderPath;

    private static readonly string selectedTemplateScriptIndexSaveKey = "ScriptGenerator_SelectedTemplateScriptIndexSaveKey";
    private static readonly string selectedTemplateFolderIndexSaveKey = "ScriptGenerator_SelectedTemplateFolderIndexSaveKey";
    private static readonly string savePathSaveKey = "ScriptGenerator_SavePathSaveKey";

    public ScriptGenerator()
    {
        SetupTemplateScriptPullDownLabel(folderName);
        SetupScriptFolderPullDownLabel();
        saveFolder = new EditorSaveFolder(savePathLabel, savePathSaveKey, scriptSaveFolderPath);
        inputField = new EditorInputField<string>(scriptNameLabel);

        errorHelpBox = new EditorErrorHelpBox(CreateScript);
        errorHelpBox.Add("作成するScriptの種類が存在しない", () => currentTemplateFolderPullDownIndex == -1);
        errorHelpBox.Add("保存場所が指定されていない", () => String.IsNullOrEmpty(savePath));
        errorHelpBox.Add("script名が指定されていない", () => inputField.Arr == null || inputField.Arr.Length == 0);
        errorHelpBox.Add("script名が重複している", () => inputField.IsDuplicate);
    }

    public override void OnGUI()
    {
        EditorGUILayout.LabelField("");
        currentTemplateFolderPullDownIndex = templateFolderPullDown.GetPullDownIndex();

        EditorGUILayout.LabelField("");
        currentTemplateScriptPullDownIndex = templateScriptPullDown.GetPullDownIndex();

        EditorGUILayout.LabelField("");
        inputField.UpdateView((name, index) =>
            EditorGUILayout.TextField($"{index}", name));

        EditorGUILayout.LabelField("");
        savePath = saveFolder.GetSavePath();

        EditorGUILayout.LabelField("");
        errorHelpBox.UpdateView();

        UpdateTemplateFolderPullDownView();
    }

    private void CreateScript()
    {
        if (!GUILayout.Button("script作成"))
        {
            return;
        }

        var templateScriptName = templateScriptPullDown.GetPullDownLabel(currentTemplateScriptPullDownIndex);
        var templateScriptGenerator = new EditorTemplateScriptGenerator($"{templateScriptPath}/{folderName}", savePath);

        for (int i = 0; i < inputField.Arr.Length; i++)
        {
            var scriptName = inputField.Arr[i];

            if (scriptName != "")
            {
                templateScriptGenerator.CreateScript(scriptName, templateScriptName);
            }
        }

        RefreshAssetDatabase();
    }

    private void SetupTemplateScriptPullDownLabel(string folderName)
    {
        var path = $"Assets/{templateScriptPath}/{folderName}";
        var pathArr = Directory.GetFiles(path, "*.cs");
        var pullDownLabelArr = new string[pathArr.Length];
        for (var i = 0; i < pullDownLabelArr.Length; i++)
        {
            pullDownLabelArr[i] = Path.GetFileNameWithoutExtension(pathArr[i]);
        }

        templateScriptPullDown = new EditorPullDown(templateScriptPullDownLabel, selectedTemplateScriptIndexSaveKey, pullDownLabelArr);
    }

    private void SetupScriptFolderPullDownLabel()
    {
        var path = $"Assets/{templateScriptPath}";
        var pathArr = Directory.GetFiles(path, "*");
        var pullDownLabelArr = new string[pathArr.Length];
        for (var i = 0; i < pullDownLabelArr.Length; i++)
        {
            pullDownLabelArr[i] = Path.GetFileNameWithoutExtension(pathArr[i]);
        }

        templateFolderPullDown = new EditorPullDown(templateFolderPullDownLabel, selectedTemplateFolderIndexSaveKey, pullDownLabelArr);
    }

    private void UpdateTemplateFolderPullDownView()
    {

        var newFolderName = templateFolderPullDown.GetPullDownLabel(currentTemplateFolderPullDownIndex);
        if (folderName != newFolderName)
        {
            folderName = newFolderName;
            SetupTemplateScriptPullDownLabel(folderName);
        }
    }
}