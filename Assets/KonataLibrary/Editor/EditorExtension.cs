using System;
using System.Linq;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class EditorExtension
{
    [MenuItem("Assets/DeleteByCollision")]
    public static void DeleteByCollision()
    {
        if (Selection.count == 0)
        {
            return;
        }
        
        var selectObjArr = Selection.gameObjects;

        for (var i = 0; i < selectObjArr.Length; i++)
        {
            var children = selectObjArr[i].GetComponentsInChildren<Collider>();
            for (var ii = 0; ii < children.Length; ii++)
            {
                MonoBehaviour.DestroyImmediate(children[ii], true);
            }
            
            PrefabUtility.SavePrefabAsset(selectObjArr[i]);
        }
    }

    [MenuItem("Assets/ChangeShader")]
    public static void ChangeShader()
    {
        if (Selection.count == 0)
        {
            return;
        }
        
        var selectObjArr = Selection.objects;

        for (var i = 0; i < selectObjArr.Length; i++)
        {
            var children = selectObjArr[i] as Material;
            children.shader = Shader.Find("Standard");
            children.SetFloat("_Smoothness", 0);
            children.SetTexture("_EmissionMap", children.GetTexture("_MainTex"));
        }
    }
}

public class EditorPullDown
{
    private readonly string pullDownLabel;
    private readonly string selectedIndexSaveKey;
    private readonly string[] pullDownLabelArr;
    
    private int selectedIndex;

    public EditorPullDown(
        string pullDownLabel,
        string selectedIndexSaveKey,
        string[] pullDownLabelArr)
    {
        this.pullDownLabel = pullDownLabel;
        this.selectedIndexSaveKey = selectedIndexSaveKey;
        this.pullDownLabelArr = pullDownLabelArr;
    }

    public int GetPullDownIndex()
    {
        if (pullDownLabelArr == null)
        {
            return -1;
        }

        selectedIndex = EditorPrefs.GetInt(selectedIndexSaveKey);
        var index = pullDownLabelArr.Length > 0
            ? EditorGUILayout.Popup(pullDownLabel, selectedIndex, pullDownLabelArr)
            : -1;

        if (index != selectedIndex)
        {
            EditorPrefs.SetInt(selectedIndexSaveKey, index);
        }

        return index;
    }

    public string GetPullDownLabel(int index)
    {
        return pullDownLabelArr[index];
    }
    
    public string GetPullDownLabel()
    {
        return pullDownLabelArr[selectedIndex];
    }
}

public class EditorSaveFolder
{
    private readonly string savePathLabel;
    private readonly string savePathSaveKey;
    private readonly string folderPath;
    

    public EditorSaveFolder(
        string savePathLabel,
        string savePathSaveKey,
        string folderPath = "Assets")
    {
        this.savePathLabel = savePathLabel;
        this.savePathSaveKey = savePathSaveKey;
        this.folderPath = folderPath;
    }

    public string GetSavePath()
    {
        var savePath = EditorPrefs.GetString(savePathSaveKey);
        
        if (GUILayout.Button(savePathLabel))
        {
            savePath = EditorUtility.SaveFolderPanel(savePathLabel, folderPath, "");
            EditorPrefs.SetString(savePathSaveKey, savePath.Replace(Application.dataPath, "Assets"));
        }

        var label = String.IsNullOrEmpty(savePath) ? "未設定" : savePath;
        EditorGUILayout.LabelField(label);
        return savePath;
    }
}

public class EditorInputField<T>
{
    private int maxIndex;
    private int previousMaxIndex;
    private T[] arr;
    private readonly string label;

    public T[] Arr => arr;
    public bool IsDuplicate => 
        arr.GroupBy(x => x).Any(g => g.Count() > 1);

    public EditorInputField(string label)
    {
        this.label = label;
        arr = new T[] { };
    }

    public void UpdateView(Func<T, int, T> onUpdate)
    {
        maxIndex = EditorGUILayout.IntField(label, maxIndex);

        if (previousMaxIndex != maxIndex)
        {
            Array.Resize(ref arr, maxIndex);
            previousMaxIndex = maxIndex;
        }

        for (var i = 0; i < arr.Length; i++)
        {
            arr[i] = onUpdate.Invoke(arr[i], i);
        }
    }
}

public class EditorErrorHelpBox
{
    private class HelpBox
    {
        public readonly string Text;
        public readonly Func<bool> IsError;

        public HelpBox(string text = "", Func<bool> isError = null)
        {
            Text = text;
            IsError = isError;
        }
    }

    private HelpBox[] helpBoxArr;
    private Action onExecutableProcess;

    public EditorErrorHelpBox(Action onExecutableProcess)
    {
        helpBoxArr = new HelpBox[0];
        this.onExecutableProcess = onExecutableProcess;
    }

    public void Add(string text, Func<bool> isError)
    {
        Array.Resize(ref helpBoxArr, helpBoxArr.Length + 1);
        helpBoxArr[helpBoxArr.Length - 1] = new HelpBox(text, isError);
    }

    public void UpdateView()
    {
        for (var i = 0; i < helpBoxArr.Length; i++)
        {
            var helpBox = helpBoxArr[i];
            if (helpBox.IsError?.Invoke() ?? false)
            {
                EditorGUILayout.HelpBox(helpBox.Text, MessageType.Error);
                return;
            }
        }
        
        onExecutableProcess?.Invoke();
    }
}
