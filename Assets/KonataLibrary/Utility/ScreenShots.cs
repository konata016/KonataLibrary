using System;
using System.IO;
using UnityEditor;
using UnityEngine;
     
#if UNITY_EDITOR

[CustomEditor(typeof(ScreenShots))]
public class ScreenShotsEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        var exampleScript = target as ScreenShots;

        if (GUILayout.Button("スクショ背景あり"))
        {
            Debug.Log("押した!");

            exampleScript.CaptureScreenShot();
            AssetDatabase.Refresh();
        }

        if (GUILayout.Button("スクショ背景なし"))
        {
            Debug.Log("押した!");

            exampleScript.CaptureScreenShotAlpha();
            AssetDatabase.Refresh();
        }
    }
}

#endif

public class ScreenShots : MonoBehaviour
{
    [SerializeField] private string _folderRootName = "Assets/Resources";
    [SerializeField] private Camera _camera;
    [SerializeField] private int _depth = 10;

    // カメラのスクリーンショットを保存する
    public void CaptureScreenShotAlpha()
    {
        var rt = new RenderTexture(_camera.pixelWidth, _camera.pixelHeight, _depth);
        var prev = _camera.targetTexture;
        _camera.targetTexture = rt;
        _camera.Render();
        _camera.targetTexture = prev;
        RenderTexture.active = rt;

        var screenShot = new Texture2D(
            _camera.pixelWidth,
            _camera.pixelHeight,
            TextureFormat.ARGB32,
            false);


        screenShot.ReadPixels(new Rect(0, 0, screenShot.width, screenShot.height), 0, 0);
        screenShot.Apply();

        var bytes = screenShot.EncodeToPNG();

        Debug.Log("スクショ");
        File.WriteAllBytes(_folderRootName + "/" + DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss") + ".png", bytes);
    }

    public void CaptureScreenShot()
    {
        ScreenCapture.CaptureScreenshot(_folderRootName + "/" + DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss") + ".png");
    }

}
