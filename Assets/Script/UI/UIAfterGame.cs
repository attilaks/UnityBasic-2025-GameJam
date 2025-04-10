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
            _victoryScreen.alpha = 0;
            _victoryScreen.transform.localScale = Vector3.zero;
            
            _victoryScreen.DOFade(1, _fadeDuration);
            _victoryScreen.transform.DOScale(1, _scaleDuration).SetEase(Ease.OutBack).SetUpdate(true);
        }

        private void ShowDefeatScreen()
        {
            _defeatScreen.gameObject.SetActive(true);
            _defeatScreen.alpha = 0;
            _defeatScreen.transform.localScale = Vector3.zero;


            _defeatScreen.DOFade(1, _fadeDuration);
            _defeatScreen.transform.DOScale(1, _scaleDuration).SetEase(Ease.Linear).SetUpdate(true);
        }
        
        public void RestartGame()
        {
            Time.timeScale = 1;
            UnityEngine.SceneManagement.SceneManager.LoadScene(0);
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

