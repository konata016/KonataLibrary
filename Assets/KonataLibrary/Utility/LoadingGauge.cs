using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class LoadingGauge : MonoBehaviour
{
    [SerializeField] private Image progressBarImage;
    
    private SequenceUniTask playProgressSequence;

    public void Initialize()
    {
        OnStart();
        
        playProgressSequence = new SequenceUniTask();
        playProgressSequence.AddTask(PlayProgress);
        playProgressSequence.RegisterOnCancel(OnFinished);
        playProgressSequence.SetLink(gameObject);
    }

    public void PlayProgress()
    {
        OnStart();
        playProgressSequence.Play();
    }
    
    public void CancelProgress()
    {
        playProgressSequence.Kill();
    }

    private async UniTask PlayProgress(CancellationToken token)
    {
        for (var i = 1; i < 10; i++)
        {
            var sec = Random.Range(0.2f, 0.4f);
            await progressBarImage.DOFillAmount(i * 0.1f, sec)
                .SetEase(Ease.Linear)
                .ToUniTask(cancellationToken: token);
            
            if (token.IsCancellationRequested)
            {
                return;
            }
        }
    }
    
    private void OnStart()
    {
        progressBarImage.fillAmount = 0;
    }
    
    private void OnFinished()
    {
        progressBarImage.fillAmount = 1;
    }
}
