using ScriptableObjects;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Script.Controllers
{
	public sealed class Board : MonoBehaviour
	{
		[SerializeField] private BoardData _boardData;
		[SerializeField] private Transform[] _rows;

		public const int BoardSize = 8;
		private const int PlayerStartRow = 0;
		private const int DragonStartRow = BoardSize - 1;

		private Transform[,] _boardCells;

		private void Awake()
		{
			if (SceneManager.GetActiveScene().buildIndex != 1) return;
			
			_boardCells = new Transform[BoardSize, BoardSize];
			InitializeBoard();
			SpawnPlayer();
			SpawnDragon();
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
			var spawnCell = GetCell(PlayerStartRow, Random.Range(0, BoardSize));
			if (!spawnCell)
			{
				Debug.LogError("Не удалось найти клетку для спавна игрока!");
				return;
			}
			
			Instantiate(_boardData.Player, spawnCell);
		}

		private void SpawnDragon()
		{
			var spawnCell = GetCell(DragonStartRow, Random.Range(0, BoardSize));
			if (!spawnCell)
			{
				Debug.LogError("Не удалось найти клетку для спавна дракона!");
				return;
			}
			
			Instantiate(_boardData.Dragon, spawnCell);
		}
		
		public Transform GetCell(int row, int col)
		{
			if (row < 0 || row >= BoardSize || col < 0 || col >= BoardSize)
				return null;
        
			return _boardCells[row, col];
		}
	}
}