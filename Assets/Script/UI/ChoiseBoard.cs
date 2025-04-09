using Script.Controllers;
using Script.GlobalManagers;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Script.UI
{
    public class ChoiseBoard : MonoBehaviour
    {
        [SerializeField] private Board _firstBoard;
        [SerializeField] private Board _secondBoard;

        [SerializeField] private Button _selectBoardButton;

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
            if(_currentBoard != null)
            {
                Destroy(_currentBoard.gameObject);
            }
            if(board != null)
            {
                _currentBoard = Instantiate(board, new Vector3(5.5f, 0, 5f), Quaternion.identity);
            }
        }
    
        private void LoadBoard()
        {
            BoardManager.ChosenBoard = _currentBoard;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }
}
