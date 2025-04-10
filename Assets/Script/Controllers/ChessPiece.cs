using UnityEngine;
// ReSharper disable InconsistentNaming

namespace Script.Controllers
{
	public class ChessPiece : MonoBehaviour
	{
		protected Board _board;
		public ChessCell CurrentCell {get; protected set;}
        
		// нужно для наложения фигурки над клеткой, чтобы фигурка была видна
		protected readonly Vector3 _aboveCellPosition = new (0, 1, 0);
		private readonly Quaternion _aboveCellRotation = Quaternion.Euler(90, 0, 0);
		
		protected virtual void Awake()
		{
			_board = FindObjectOfType<Board>();
			if (!_board)
			{
				Debug.LogError("Board не найден на сцене!");
				return;
			}
            
			CurrentCell = transform.parent.GetComponent<ChessCell>();
			if (!CurrentCell)
			{
				Debug.LogError("Компонент ChessCell не найден на игроке!");
				return;
			}
            
			transform.localPosition = _aboveCellPosition;
			transform.rotation = _aboveCellRotation;
		}
	}
}