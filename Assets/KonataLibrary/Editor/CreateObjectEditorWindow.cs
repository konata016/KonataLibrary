using System.Linq;
using UnityEditor;
using UnityEngine;

public class CreateObjectEditorWindow : EditorWindow
{
    private string folderPath;

    [MenuItem("Tools/CreateObjectEditorWindow")]
    public static void Open()
    {
        GetWindow(typeof(CreateObjectEditorWindow));
    }

    private void OnGUI()
    {
        folderPath = EditorGUILayout.TextField("フォルダーパス", folderPath);

        if (GUILayout.Button("オブジェクト作成"))
        {
            OnClickCreateButton(folderPath);
        }
    }

    private void OnClickCreateButton(string path)
    {
        var idArr = AssetDatabase.FindAssets("t:Prefab", new[] { path });

        for (var i = 0; i < idArr.Length; i++)
        {
            var prefabPath = AssetDatabase.GUIDToAssetPath(idArr[i]);
            var prefab = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath);
            var capitalizedName = CapitalizeFirstLetter(prefab.name);
            var obj = new GameObject(capitalizedName);


            var prefabObj = PrefabUtility.InstantiatePrefab(prefab) as GameObject;
            if (prefabObj == null)
            {
                Debug.LogError("prefabObj is null");
                continue;
            }

            var childTransform = prefabObj.transform;
            var parentTransform = obj.transform;
            
            parentTransform.position = Vector3.zero;
            parentTransform.rotation = Quaternion.identity;
            parentTransform.localScale = Vector3.one;
            
            childTransform.parent = obj.transform;
            childTransform.localPosition = Vector3.zero;
            childTransform.localRotation = Quaternion.identity;
            //childTransform.localScale = Vector3.one;
        }
    }

    private static string CapitalizeFirstLetter(string input)
    {
        var stringWithoutSpaces = new string(input.Where(c => !char.IsWhiteSpace(c)).ToArray());
        if (string.IsNullOrEmpty(stringWithoutSpaces))
        {
            return stringWithoutSpaces;
        }
        
        return char.ToUpper(stringWithoutSpaces[0]) + stringWithoutSpaces.Substring(1);
    }
}
