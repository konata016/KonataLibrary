using System;
using System.IO;
using UnityEditor;
using UnityEngine;

public class EditorTemplateScriptGenerator : MonoBehaviour
{
    private readonly string templateScriptPath;
    private readonly string savePath;
    private Action onDataReplacing;

    private string scriptData;

    public EditorTemplateScriptGenerator(
        string templateScriptPath,
        string savePath)
    {
        this.templateScriptPath = templateScriptPath;
        this.savePath = savePath;
    }

    public void CreateScript(string scriptName, string templateScriptName)
    {
        // もととなるscriptのクラス名書き換え
        Debug.Log($"{templateScriptPath}/{templateScriptName}");
        scriptData = File.ReadAllText($"{Application.dataPath}/{templateScriptPath}/{templateScriptName}.cs");
        scriptData = scriptData.Replace(templateScriptName, scriptName);
        onDataReplacing?.Invoke();

        // script作成
        var filePath = $"{savePath}/{scriptName}.cs";
        var assetPath = AssetDatabase.GenerateUniqueAssetPath(filePath);
        File.WriteAllText(assetPath, scriptData);
    }

    public void AddDataReplacing(string oldValue, string newValue)
    {
        onDataReplacing += () => scriptData = scriptData.Replace(oldValue, newValue);
    }
}
