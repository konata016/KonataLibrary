using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

public class MVPAndStateBasePatternScriptGenerator : ScriptGeneratorBase
{
    private EditorInputField<string> inputField;
    private EditorPullDown pullDown;
    private EditorSaveFolder saveFolder;
    private EditorErrorHelpBox errorHelpBox;

    private int currentPullDownIndex;
    private string savePath;

    private static readonly string savePathLabel = "保存場所";
    private static readonly string scriptNameLabel = "生成するScript名の要素数";
    private static readonly string pullDownLabel = "Enumの種類";

    private static readonly string enumDefineScriptName = $"{EditorPathDefine.EnumDefineScriptName}";
    private static readonly string templateScriptPath = $"{EditorPathDefine.TemplateScriptFolderPath}/MvpAndStateBase";
    private static readonly string scriptSaveFolderPath = EditorPathDefine.ScriptSaveFolderPath;

    private static readonly string templateScriptPresenterName = "TEMPLATE_SCRIPT_STATE_PATTERN_BASE_PRESENTER";
    private static readonly string templateScriptViewName = "TEMPLATE_SCRIPT_STATE_PATTERN_BASE_VIEW";
    private static readonly string templateScriptModelName = "TEMPLATE_SCRIPT_STATE_PATTERN_BASE_MODEL";
    private static readonly string templateScriptStateName = "TEMPLATE_SCRIPT_STATE_BASE_TEMPLATE";
    private static readonly string templateScriptStateBaseName = "TEMPLATE_SCRIPT_STATE_BASE_BASE";
    private static readonly string templateScriptStateInfoName = "TEMPLATE_SCRIPT_STATE_BASE_INFO";
    private static readonly string templateEnum = "TEMPLATE_ENUM";

    private static readonly string selectedIndexSaveKey = "selectedIndexSaveKey";
    private static readonly string savePathSaveKey = "savePathSaveKey";

    public MVPAndStateBasePatternScriptGenerator()
    {
        SetupPullDownLabel();
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
        currentPullDownIndex = pullDown.GetPullDownIndex();

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
                var presenterName = $"{scriptName}PresenterBase";
                var viewName = $"{scriptName}ViewBase";
                var modelName = $"{scriptName}ModelBase";
                var standbyStateName = $"Standby{scriptName}State";
                var discardingStateName = $"Discarding{scriptName}State";
                var stateBaseName = $"{scriptName}StateBase";
                var infoName = $"{scriptName}Info";

                var templateScriptGenerator =
                    new EditorTemplateScriptGenerator(templateScriptPath, savePath);

                templateScriptGenerator.AddDataReplacing(templateScriptPresenterName, presenterName);
                templateScriptGenerator.AddDataReplacing(templateScriptViewName, viewName);
                templateScriptGenerator.AddDataReplacing(templateScriptModelName, modelName);
                templateScriptGenerator.AddDataReplacing(templateScriptStateName, standbyStateName);
                templateScriptGenerator.AddDataReplacing(templateScriptStateBaseName, stateBaseName);
                templateScriptGenerator.AddDataReplacing(templateScriptStateInfoName, infoName);
                templateScriptGenerator.AddDataReplacing(templateEnum, pullDown.GetPullDownLabel(currentPullDownIndex));

                templateScriptGenerator.CreateScript(presenterName, templateScriptPresenterName);
                templateScriptGenerator.CreateScript(viewName, templateScriptViewName);
                templateScriptGenerator.CreateScript(modelName, templateScriptModelName);
                templateScriptGenerator.CreateScript(standbyStateName, templateScriptStateName);
                templateScriptGenerator.CreateScript(discardingStateName, templateScriptStateName);
                templateScriptGenerator.CreateScript(stateBaseName, templateScriptStateBaseName);
                templateScriptGenerator.CreateScript(infoName, templateScriptStateInfoName);
            }
        }

        RefreshAssetDatabase();
    }

    private void SetupPullDownLabel()
    {
        const string enumPattern = @"enum\s+(\w+)\s*{";
        const string namespacePattern = @"namespace\s+(\w+)\s*\{(?:[^{}]|(?<open>\{)|(?<-open>\}))+(?(open)(?!))\}";

        var guidArr = AssetDatabase.FindAssets(enumDefineScriptName);
        var path = AssetDatabase.GUIDToAssetPath(guidArr[0]).Replace("Assets/", "");
        var data = File.ReadAllText($"{Application.dataPath}/{path}");
        var pullDownLabelList = new List<string>();
        
        var enumMatches = Regex.Matches(data, enumPattern, RegexOptions.Singleline);
        var namespaceMatches = Regex.Matches(data, namespacePattern, RegexOptions.Singleline);
        for (var i = 0; i < enumMatches.Count; i++)
        {
            var enumMatch = enumMatches[i];
            var enumName = enumMatch.Groups[1].Value;
            var combinedName = enumName;

            for (var ii = 0; ii < namespaceMatches.Count; ii++)
            {
                var namespaceMatch = namespaceMatches[ii];
                var namespaceName = namespaceMatch.Groups[1].Value;

                if (enumMatch.Index > namespaceMatch.Index)
                {
                    combinedName = $"{namespaceName}.{enumName}";
                }
            }
            
            pullDownLabelList.Add(combinedName);
        }

        pullDown = new EditorPullDown(pullDownLabel, selectedIndexSaveKey, pullDownLabelList.ToArray());
    }
}