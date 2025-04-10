using System;
using System.Collections.Generic;
using Script.Enums;
using ScriptableObjects;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

namespace Script.Controllers
{
	public sealed class Board : MonoBehaviour
	{
		[SerializeField] private BoardData _boardData;
		[SerializeField] private Transform[] _rows;
		
		public event Action<bool> OnEndOfGame = delegate { };

		public const int BoardSize = 8;
		private const byte MaxNeighboursToCell = 4;
		private const int PlayerStartRow = 0;
		private const int DragonStartRow = BoardSize - 1;
		private const int TreasureStartRow = 4;
		// private const int BombsRow = 2;

		private Transform[,] _boardCells;
		private PlayerController _player;
		private DragonController _dragon;
		private ChessPiece _treasureChest;
		// private ChessPiece _spawner;

		private Dictionary<int, ChessCell> _cellsDictionary = new();
		
		public event Action<Turn> NextTurnEvent = delegate { };

		private void Awake()
		{
			if (SceneManager.GetActiveScene().buildIndex != 1) return;
			
			_boardCells = new Transform[BoardSize, BoardSize];
			InitializeBoard();
			
			_player = SpawnPlayer();
			_dragon = SpawnDragon();
			_treasureChest = SpawnerTreasure();
			// _spawner = SpawnerBombs();

			_player.EndOfTurnEvent += HandleEndOfTurn;
			_dragon.EndOfTurnEvent += HandleEndOfTurn;
			
			NextTurnEvent.Invoke(Turn.Player);
		}

		private void OnDestroy()
		{
			if (SceneManager.GetActiveScene().buildIndex != 1) return;

			if (_player && _dragon)
			{
				_player.EndOfTurnEvent -= HandleEndOfTurn;
				_dragon.EndOfTurnEvent -= HandleEndOfTurn;
			}
		}

		private void HandleEndOfTurn(Turn turnSide)
		{
			if (_dragon.CurrentCell.Equals(_player.CurrentCell))
			{
				PlayerIsDefeated();
				return;
			}
			
			switch (turnSide)
			{
				case Turn.Player:
					NextTurnEvent.Invoke(Turn.Dragon);
					break;
				case Turn.Dragon:
					NextTurnEvent.Invoke(Turn.Player);
					break;
				default:
					throw new ArgumentOutOfRangeException(nameof(turnSide), turnSide, null);
			}
		}

		private void PlayerIsDefeated()
		{
			OnEndOfGame.Invoke(false);
		}

		private void InitializeBoard()
		{
			for (byte row = 0; row < _rows.Length; row++)
			{
				for (byte col = 0; col < _rows[row].childCount; col++)
				{
					_boardCells[row, col] = _rows[row].GetChild(col);
					
					var cellComponent = _boardCells[row, col].gameObject.AddComponent<ChessCell>();
					cellComponent.SetCoordinates(row, col);
					_cellsDictionary[cellComponent.GetInstanceID()] = cellComponent;
				}
			}
		}
		
		private PlayerController SpawnPlayer()
		{
			var spawnCell = GetCell(PlayerStartRow, Random.Range(0, BoardSize));
			if (spawnCell) 
				return Instantiate(_boardData.Player, spawnCell);
			
			Debug.LogError("Не удалось найти клетку для спавна игрока!");
			return null;

		}

		private DragonController SpawnDragon()
		{
			var spawnCell = GetCell(DragonStartRow, Random.Range(0, BoardSize));
			if (spawnCell) 
				return Instantiate(_boardData.Dragon, spawnCell);
			
			Debug.LogError("Не удалось найти клетку для спавна дракона!");
			return null;
		}
		
		private ChessPiece SpawnerTreasure()
		{
			var spawnCell = GetCell(TreasureStartRow, Random.Range(0, BoardSize));
			if (spawnCell)
				return Instantiate(_boardData.TreasureChest, spawnCell);
			
			Debug.LogError("Не удалось найти клетку для спавна сокровища!");
			return null;
		}

		// private ChessPiece SpawnerBombs()
		// {
		// 	var spawnCell = GetCell(BombsRow, Random.Range(0, BoardSize));
		// 	if (spawnCell) 
		// 		return Instantiate(_boardData.Bomb, spawnCell);
		// 	
		// 	Debug.LogError("Не удалось найти клетку для спавна бомбы!");
		// 	return null;
		// }

		public Transform GetCell(int row, int col)
		{
			if (row < 0 || row >= BoardSize || col < 0 || col >= BoardSize)
				return null;
        
			return _boardCells[row, col];
		}

		public ChessCell GetPlayerCell() => _player.CurrentCell;

		public List<ChessCell> GetAdjacentCells(ChessCell cell)
		{
			var adjacentCells = new List<ChessCell>(MaxNeighboursToCell);
			
			var leftCell = GetCell(cell.Row, cell.Column - 1);
			if (leftCell) adjacentCells.Add(leftCell.GetComponent<ChessCell>());
			
			var rightCell = GetCell(cell.Row, cell.Column + 1);
			if (rightCell) adjacentCells.Add(rightCell.GetComponent<ChessCell>());
			
			var upperCell = GetCell(cell.Row + 1, cell.Column);
			if (upperCell) adjacentCells.Add(upperCell.GetComponent<ChessCell>());
			
			var lowerCell = GetCell(cell.Row - 1, cell.Column);
			if (lowerCell) adjacentCells.Add(lowerCell.GetComponent<ChessCell>());
			
			return adjacentCells;
		}
	}
}