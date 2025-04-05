using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Script.UI
{
    public class MenuManager : MonoBehaviour
    {
        [SerializeField] private float _minLoadTime = 3f;
        [SerializeField] private float _maxLoadTime = 10f;
        [SerializeField] private CanvasGroup _loadingAnimationCanvasGroup;
        [SerializeField] private Image _circleBar;
        [SerializeField] private CanvasGroup _startButtonCanvasGroup;
        [SerializeField] private float _buttonShowDuration = 0.8f;
        [SerializeField] private Ease _buttonShowEase = Ease.OutBack;
        
        [SerializeField] private ChoiseBoard _chooseBoardPanel;
    
        private float _fadeDuration = 1f;

        private void Start()
        {
            InitializeUI();
            StartLoading();
        }

        private void StartLoading()
        {
            _loadingAnimationCanvasGroup.alpha = 1f;
            _loadingAnimationCanvasGroup.gameObject.SetActive(true);
            CircleAnimation();
        }
        private void InitializeUI()
        {
            _startButtonCanvasGroup.alpha = 0;
            _startButtonCanvasGroup.gameObject.SetActive(false);
            
            _chooseBoardPanel.gameObject.SetActive(false);
        }
    
        private void CircleAnimation()
        {
            _circleBar.DOFillAmount(1, Random.Range(_minLoadTime, _maxLoadTime))
                .OnComplete(FinishLoading)
                .SetEase(Ease.Linear);
        }
        private void FinishLoading()
        {
            _loadingAnimationCanvasGroup.DOFade(0, _fadeDuration);
            ShowStartButton();
            
            _loadingAnimationCanvasGroup.gameObject.SetActive(false);
            _chooseBoardPanel.gameObject.SetActive(true);
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
}