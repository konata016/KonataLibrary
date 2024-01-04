using UnityEditor;
using UnityEngine;
public class MaterialChangingEditorWindow : EditorWindow
{
    private EditorErrorHelpBox errorHelpBox;
    private Material material;
    
    [MenuItem("Tools/MaterialChangingEditorWindow")]
    public static void Open()
    {
        GetWindow(typeof(MaterialChangingEditorWindow));
    }

    private void OnEnable()
    {
        errorHelpBox = new EditorErrorHelpBox(ChangeMaterial);
        errorHelpBox.Add("選択されていない", () => Selection.count == 0);
        errorHelpBox.Add("マテリアルがない", () => material == null);
    }

    private void OnGUI()
    {
        EditorGUILayout.LabelField("");
        material = EditorGUILayout.ObjectField(
            "マテリアル",
            material,
            typeof(Material),
            false) as Material;
        
        errorHelpBox.UpdateView();
    }

    private void ChangeMaterial()
    {
        if (!GUILayout.Button("マテリアル差し替え"))
        {
            return;
        }
        
        var selectObjArr = Selection.gameObjects;
        for (var i = 0; i < selectObjArr.Length; i++)
        {
            var children = selectObjArr[i].GetComponentsInChildren<Renderer>();
            for (var ii = 0; ii < children.Length; ii++)
            {
                children[ii].material = material;
            }
        }
    }
}
