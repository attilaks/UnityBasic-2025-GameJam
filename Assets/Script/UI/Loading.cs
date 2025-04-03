using System.Collections;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class DoTweenLoading : MonoBehaviour
{
    [SerializeField] private float _minLoadTime = 3f;
    [SerializeField] private float _maxLoadTime = 10f;
    [SerializeField] private CanvasGroup _canvasGroup;
    [SerializeField] private Image _circleBar;
    [SerializeField] private CanvasGroup _startButtonCanvasGroup;
    [SerializeField] private float _buttonShowDuration = 0.8f;
    [SerializeField] private Ease _buttonShowEase = Ease.OutBack;
    
    private float _fadeDuration = 1f;

    private void Start()
    {
        InitializeUI();
        StartLoading();
    }

    private void StartLoading()
    {
        _canvasGroup.alpha = 1f;
        _canvasGroup.gameObject.SetActive(true);
        CircleAnimation();
    }
    private void InitializeUI()
    {
        _startButtonCanvasGroup.alpha = 0;
        _startButtonCanvasGroup.gameObject.SetActive(false);
    }
    
    private void CircleAnimation()
    {
        _circleBar.DOFillAmount(1, Random.Range(_minLoadTime, _maxLoadTime))
            .OnComplete(() => FinishLoading())
            .SetEase(Ease.Linear);
    }
    private void FinishLoading()
    {
        _canvasGroup.DOFade(0, _fadeDuration);
        ShowStartButton();
    }
    private void ShowStartButton()
    {
        _startButtonCanvasGroup.gameObject.SetActive(true);
        _startButtonCanvasGroup.DOFade(1, _buttonShowDuration);
    }
    
    public void OnStartButtonClick()
    {
        
    }
    
    
}