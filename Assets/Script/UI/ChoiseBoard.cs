using Script.GlobalManagers;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Script.UI
{
    public class ChoiseBoard : MonoBehaviour
    {
        [SerializeField] private GameObject _firstBoard;
        [SerializeField] private GameObject _secondBoard;

        [SerializeField] private Button _selectBoardButton;

        private GameObject _currentBoard;

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

        private void SpawnLocation(GameObject board)
        {
            if(_currentBoard != null)
            {
                Destroy(_currentBoard);
            }
            if(board != null)
            {
                _currentBoard = Instantiate(board, Vector3.zero, Quaternion.identity);
            }
        }
    
        private void LoadBoard()
        {
            BoardManager.ChosenBoard = _currentBoard;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }
}
