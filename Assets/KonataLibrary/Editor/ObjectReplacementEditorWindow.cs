using UnityEditor;
using UnityEngine;

public class ObjectReplacementEditorWindow : EditorWindow
{
    private GameObject prefab;

    private static readonly string undoName = " Prefabを差し替え";

    [MenuItem("Tools/ObjectReplacementEditorWindow")]
    public static void Open()
    {
        ObjectReplacementEditorWindow.GetWindow(typeof(ObjectReplacementEditorWindow));
    }

    private void OnEnable()
    {
        //選択されている物が変わったらGUIを更新するように
        Selection.selectionChanged += Repaint;
    }

    private void OnGUI()
    {
        EditorGUILayout.LabelField("差し替え先のPrefab");
        prefab = EditorGUILayout.ObjectField(prefab, typeof(GameObject), false) as GameObject;

        if (prefab == null)
        {
            EditorGUILayout.HelpBox("差し替え先のPrefabが設定されていません", MessageType.Error);
        }
        else if (Selection.transforms.Length == 0)
        {
            EditorGUILayout.HelpBox("差し替えるオブジェクトが選択されていません", MessageType.Error);
        }
        else if (GUILayout.Button("差し替え"))
        {
            var selectObjArr = Selection.gameObjects;
            for (var i = 0; i < selectObjArr.Length; i++)
            {
                var beforeObj = selectObjArr[i];
                var beforeTransform = beforeObj.transform;

                var afterObj = PrefabUtility.InstantiatePrefab(prefab) as GameObject;
                var afterTransform = afterObj.transform;

                Undo.RegisterCreatedObjectUndo(afterObj, undoName);
                Undo.SetTransformParent(afterTransform, beforeTransform.parent, undoName);
                afterTransform.SetSiblingIndex(beforeTransform.GetSiblingIndex());

                afterTransform.position = beforeTransform.position;
                afterTransform.rotation = beforeTransform.rotation;
                afterTransform.localScale = beforeTransform.localScale;

                Undo.DestroyObjectImmediate(beforeObj);
            }
        }
    }
}
