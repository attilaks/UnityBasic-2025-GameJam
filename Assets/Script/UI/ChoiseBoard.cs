using UnityEngine;

public class ChoiseBoard : MonoBehaviour
{
    [SerializeField] private GameObject _firstBoard;
    [SerializeField] private GameObject _secondBoard;

    private GameObject _currentBoard;

    public void SelectFirstBoard()
    {
        SpawnLocation(_firstBoard);
    }

    public void SelectSecondBoard()
    {
        SpawnLocation(_secondBoard);
    }
    
    public void SelectBoardName(string name)
    {
        if (name == _firstBoard.name)
        {
            SpawnLocation(_firstBoard);
        }
        else if (name == _secondBoard.name)
        {
            SpawnLocation(_secondBoard);
        }
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
}
