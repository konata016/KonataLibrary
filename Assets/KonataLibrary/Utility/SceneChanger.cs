using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneChanger : SingletonMonoBehaviour<SceneChanger>
{
    [SerializeField] private Image fadeImage;

    private CancellationTokenSource token;

    private static readonly float fadeTime = 0.25f;

    public bool IsCurrentScene(string sceneName)
    {
        return SceneManager.GetActiveScene().name == sceneName;
    }
    
    public void PlayChangingScene(string sceneName)
    {
        token?.Cancel();
        token = new CancellationTokenSource();
        ChangeScene(sceneName, token.Token).Forget();
    }

    private async UniTask ChangeScene(string sceneNam, CancellationToken token)
    {
        SetActiveFadeImage(true);
        await fadeImage.DOFade(1, fadeTime)
            .SetEase(Ease.Linear)
            .SetUpdate(true)
            .ToUniTask(cancellationToken: token);

        DOTween.Clear(true);
        SceneManager.LoadScene(sceneNam);

        await Resources.UnloadUnusedAssets().ToUniTask(cancellationToken: token);
        GC.Collect();

        await UniTask.NextFrame(token);
        await fadeImage.DOFade(0, fadeTime)
            .SetEase(Ease.Linear)
            .SetUpdate(true)
            .ToUniTask(cancellationToken: token);

        SetActiveFadeImage(false);
    }

    private void SetActiveFadeImage(bool isActive)
    {
        var color = fadeImage.color;
        color.a = isActive ? 0 : 1;
        fadeImage.color = color;
        fadeImage.gameObject.SetActive(isActive);
    }
}
