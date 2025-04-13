using Script.Controllers;
using Script.GlobalManagers;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Script.UI
{
    public class ChoiseBoard : MonoBehaviour
    {
        [Header("Boards")]
        [SerializeField] private Board _firstBoard;
        [SerializeField] private Board _secondBoard;

        [Header("Buttons")]
        [SerializeField] private Button _selectBoardButton;
        
        [Header("Audio")]
        [SerializeField] private AudioSource _backgroundAudio;

        private Board _currentBoard;
        private SpawnerBomb _spawner;

        private void Awake()
        {
            _selectBoardButton.onClick.AddListener(LoadBoard);
        }

        private void OnDestroy()
        {
            _selectBoardButton.onClick.RemoveListener(LoadBoard);
        }

        public void SelectFirstBoard()
        {
            SpawnLocation(_firstBoard);
        }

        public void SelectSecondBoard()
        {
            SpawnLocation(_secondBoard);
        }

        private void SpawnLocation(Board board)
        {
            if(_currentBoard)
            {
                Destroy(_currentBoard.gameObject);
            }
            if(board)
            {
                _currentBoard = Instantiate(board, new Vector3(5.5f, 0, 5f), Quaternion.identity);
            }
        }
    
        private void LoadBoard()
        {
            if (!_currentBoard) return;
            
            InterSceneObjects.ChosenBoard = _currentBoard;
            InterSceneObjects.BackgroundMusic = _backgroundAudio;
            
            DontDestroyOnLoad(InterSceneObjects.ChosenBoard);
            DontDestroyOnLoad(InterSceneObjects.BackgroundMusic);
            
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }
}
