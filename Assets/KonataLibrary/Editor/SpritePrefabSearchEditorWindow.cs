using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class SpritePrefabSearchEditorWindow : EditorWindow
{
    private Sprite targetSprite;
    private List<string> prefabPathList = new();
    private Vector2 scrollPosition;

    [MenuItem("Tools/SpritePrefabSearchEditorWindow")]
    public static void Open()
    {
        GetWindow(typeof(SpritePrefabSearchEditorWindow));
    }
    
    private void OnGUI()
    {
        GUILayout.Label("検索対象", EditorStyles.boldLabel);
        targetSprite = EditorGUILayout.ObjectField(targetSprite, typeof(Sprite), false) as Sprite;

        if (GUILayout.Button("検索"))
        {
            FindPrefabsWithSprite();
        }
        
        if (prefabPathList is { Count: > 0 })
        {
            GUILayout.Label("検索結果");
            
            var maxScrollHeight = position.height - 100;
            var scrollViewHeight = Mathf.Min(prefabPathList.Count * 20, maxScrollHeight);
            scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition, GUILayout.Height(scrollViewHeight));
            
            var leftAlignedStyle = new GUIStyle(GUI.skin.button)
            {
                alignment = TextAnchor.MiddleLeft
            };
            
            foreach (var path in prefabPathList)
            {
                if (GUILayout.Button(path, leftAlignedStyle))
                {
                    EditorGUIUtility.PingObject(AssetDatabase.LoadAssetAtPath<GameObject>(path));
                }
            }
            
            EditorGUILayout.EndScrollView();
        }
    }
    
    
    private void FindPrefabsWithSprite()
    {
        if (targetSprite == null)
        {
            Debug.LogWarning("検索対象が設定されていない");
            return;
        }

        var prefabPathArr = AssetDatabase.GetAllAssetPaths().Where(path => path.EndsWith(".prefab")).ToArray();
        var prefabsWithSpriteList = new List<GameObject>();
        prefabPathList.Clear();

        foreach (var path in prefabPathArr)
        {
            var prefab = AssetDatabase.LoadAssetAtPath(path, typeof(GameObject)) as GameObject;
            if (prefab != null)
            {
                var spriteRendererArr = prefab.GetComponentsInChildren<Image>(true);
                foreach (var sr in spriteRendererArr)
                {
                    if (sr.sprite == null)
                    {
                        continue;
                    }
                    
                    if (sr.sprite == targetSprite)
                    {
                        prefabsWithSpriteList.Add(prefab);
                        break;
                    }
                }
            }
        }

        foreach (var prefab in prefabsWithSpriteList)
        {
            prefabPathList.Add(AssetDatabase.GetAssetPath(prefab));
        }

        Debug.Log("検索終了");
    }
}
