using System.Linq;
using OutGame;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneEditorWindow : EditorWindow
{
    private static readonly string[] sceneNameArr =
    {
    };

    [MenuItem("Tools/SceneEditorWindow")]
    public static void Open()
    {
        GetWindow(typeof(SceneEditorWindow));
    }

    private void OnGUI()
    {
        for (var i = 0; i < sceneNameArr.Length; i++)
        {
            var name = sceneNameArr[i];
            if (GUILayout.Button(name))
            {
                var scene = new[] { SceneManager.GetActiveScene() };
                if (!EditorSceneManager.SaveModifiedScenesIfUserWantsTo(scene))
                {
                    return;
                }

                var sceneAssets = AssetDatabase.FindAssets("t:SceneAsset")
                    .Select(AssetDatabase.GUIDToAssetPath)
                    .Select(path => AssetDatabase.LoadAssetAtPath(path, typeof(SceneAsset)))
                    .Where(obj => obj != null)
                    .Select(obj => (SceneAsset)obj)
                    .Where(asset => asset.name == name);
                
                var scenePath = AssetDatabase.GetAssetPath(sceneAssets.First());
                Debug.Log(scenePath);
                EditorSceneManager.OpenScene(scenePath);
            }
        }
    }
}
