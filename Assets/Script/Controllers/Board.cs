using ScriptableObjects;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Script.Controllers
{
	public class Board : MonoBehaviour
	{
		[SerializeField] private BoardData _boardData;
		[SerializeField] private Transform[] _rows;

		public const int BoardSize = 8;

		private Transform[,] _boardCells;
		
		private void Awake()
		{
			if (SceneManager.GetActiveScene().buildIndex != 1) return;
			
			_boardCells = new Transform[BoardSize, BoardSize];
			InitializeBoard();
			SpawnPlayer();
		}
    
		private void InitializeBoard()
		{
			for (int row = 0; row < _rows.Length; row++)
			{
				for (int col = 0; col < _rows[row].childCount; col++)
				{
					_boardCells[row, col] = _rows[row].GetChild(col);
					
					ChessCell cellComponent = _boardCells[row, col].gameObject.AddComponent<ChessCell>();
					cellComponent.SetCoordinates(row, col);
				}
			}
		}
		
		private void SpawnPlayer()
		{
			var spawnCell = GetCell(0, Random.Range(0, Board.BoardSize));
			if (!spawnCell)
			{
				Debug.LogError("Не удалось найти клетку для спавна игрока!");
				return;
			}

			// Создаем игрока
			var player = Instantiate(_boardData.Player, new Vector3(0, 1, 0), Quaternion.Euler(90, 0, 0), spawnCell);
			var cellComponent = spawnCell.GetComponent<ChessCell>();
			if (cellComponent)
			{
				player.SetPosition(cellComponent.Row, cellComponent.Column);
			}
			else
			{
				Debug.LogWarning("Клетка не имеет компонента ChessCell, позиция может быть неверной");
			}

			Debug.Log($"Игрок создан на позиции {spawnCell.GetComponent<ChessCell>()?.ChessNotation ?? "unknown"}");
		}
		
		public Transform GetCell(int row, int col)
		{
			if (row < 0 || row >= BoardSize || col < 0 || col >= BoardSize)
				return null;
        
			return _boardCells[row, col];
		}
		
		public Transform GetCell(string chessNotation)
		{
			if (chessNotation.Length != 2) return null;
			chessNotation = chessNotation.ToLower();
        
			char letterChar = chessNotation[0];
			char numberChar = chessNotation[1];
        
			int col = letterChar - 'a';
			int row = numberChar - '0';
        
			return GetCell(row, col);
		}
	}
}