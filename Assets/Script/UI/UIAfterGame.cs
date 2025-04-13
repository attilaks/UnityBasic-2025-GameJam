using System;
using DG.Tweening;
using Script.Controllers;
using UnityEngine;

namespace Script.UI
{
    public class UIManager : MonoBehaviour
    {
        [Header("Event invokers")]
        [SerializeField] private BoardSpawner _boardSpawner;
        
        [Header("Object references")]
        [SerializeField] private CanvasGroup _victoryScreen;
        [SerializeField] private CanvasGroup _defeatScreen;

        [Header("Settings")]
        [SerializeField] private float _fadeDuration = 0.5f;
        [SerializeField] private float _scaleDuration = 0.3f;
        
        private void Awake()
        {
            _boardSpawner.OnEndOfGame += OnEndOfGameHandler;
        }

        private void OnDestroy()
        {
            _boardSpawner.OnEndOfGame -= OnEndOfGameHandler;
        }

        private void OnEndOfGameHandler(bool playerWon)
        {
            if (playerWon)
            {
                ShowVictoryScreen();
            }
            else
            {
                ShowDefeatScreen();
            }
        }

        private void ShowVictoryScreen()
        {
            _victoryScreen.gameObject.SetActive(true);
            
            _victoryScreen.DOFade(1, _fadeDuration).From(0);
            _victoryScreen.transform.DOScale(1, _scaleDuration).From(0).SetEase(Ease.OutBack).SetUpdate(true);
        }

        private void ShowDefeatScreen()
        {
            _defeatScreen.gameObject.SetActive(true);

            _defeatScreen.DOFade(1, _fadeDuration).From(0);
            _defeatScreen.transform.DOScale(1, _scaleDuration).From(0).SetEase(Ease.Linear).SetUpdate(true);
        }
        
        public void RestartGame()
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(1);
        }
    
        public void QuitGame()
        {
            Application.Quit();
            
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#endif
        }
        
    }
}

