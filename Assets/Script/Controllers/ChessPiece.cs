using UnityEngine;
// ReSharper disable InconsistentNaming

namespace Script.Controllers
{
	public class ChessPiece : MonoBehaviour
	{
		protected Board _board;
		protected ChessCell _currentCell;
        
		// нужно для наложения фигурки над клеткой, чтобы фигурка была видна
		protected readonly Vector3 _aboveCellPosition = new (0, 1, 0);
		
		protected virtual void Awake()
		{
			_board = FindObjectOfType<Board>();
			if (!_board)
			{
				Debug.LogError("Board не найден на сцене!");
				return;
			}
            
			_currentCell = transform.parent.GetComponent<ChessCell>();
			if (!_currentCell)
			{
				Debug.LogError("Компонент ChessCell не найден на игроке!");
				return;
			}
            
			transform.localPosition = _aboveCellPosition;
			transform.rotation = Quaternion.Euler(90, 0, 0);
		}
	}
}