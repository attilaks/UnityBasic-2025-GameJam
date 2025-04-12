using System;
using System.Collections.Generic;
using Script.Enums;
using Script.Interfaces;
using ScriptableObjects;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

namespace Script.Controllers
{
	public sealed class Board : MonoBehaviour, IBoard
	{
		[SerializeField] private BoardData _boardData;
		[SerializeField] private Transform[] _rows;
		
		public event Action<bool> OnEndOfGame = delegate { };
		
		private const byte EachItemCount = 2;
		private const byte MaxNeighboursToCell = 4;
		
		private const byte PlayerStartRow = 0;
		private const byte SpeedBootsRow = 1;
		private const byte BombsRow = 2;
		
		public int BoardSize => _rows.Length;
		private int MinTreasureStartCol => BoardSize / 2 - 1;
		private int MaxTreasureStartCol => BoardSize / 2;
		private int MinTreasureStartRow => BoardSize / 2 - 1;
		private int MaxTreasureStartRow => BoardSize / 2;
		private int DragonStartRow => BoardSize - 1;
		
		private ChessCell[,] _boardCells;
		private PlayerController _player;
		private DragonController _dragon;
		private ChessPiece _treasureChest;
		
		private readonly ChessPiece[] _speedBoots = new ChessPiece[2];
		private readonly Dictionary<int, ChessCell> _cellsDict = new();
		
		public event Action<Actor> NextTurnEvent = delegate { };

		private void Awake()
		{
			if (SceneManager.GetActiveScene().buildIndex != 1) return;
			
			_boardCells = new ChessCell[BoardSize, BoardSize];
			InitializeBoard();
			
			_player = SpawnPlayer();
			_dragon = SpawnDragon();
			_treasureChest = SpawnerTreasure();
			SpawnBombs();
			SpawnSpeedBoots();

			_player.EndOfTurnEvent += HandleEndOfTurn;
			_dragon.EndOfTurnEvent += HandleEndOfTurn;
			
			NextTurnEvent.Invoke(Actor.Player);
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

		private void HandleEndOfTurn(Actor actorSide)
		{
			if (_dragon.CurrentCell.Equals(_player.CurrentCell))
			{
				PlayerIsDefeated();
				return;
			}
			
			if (_player.CurrentCell.Equals(_treasureChest.CurrentCell))
			{
				PlayerWon();
				return;
			}
			
			switch (actorSide)
			{
				case Actor.Player:
					NextTurnEvent.Invoke(Actor.Dragon);
					break;
				case Actor.Dragon:
					NextTurnEvent.Invoke(Actor.Player);
					break;
				default:
					throw new ArgumentOutOfRangeException(nameof(actorSide), actorSide, null);
			}
		}

		private void PlayerWon()
		{
			OnEndOfGame.Invoke(true);
			
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
					var cellTransform = _rows[row].GetChild(col);
					var cellComponent = cellTransform.gameObject.AddComponent<ChessCell>();
					cellComponent.SetCoordinates(row, col);
					
					_boardCells[row, col] = cellComponent;
					_cellsDict[cellComponent.GetInstanceID()] = cellComponent;
				}
			}
		}
		
		private PlayerController SpawnPlayer()
		{
			var spawnCell = GetCell(PlayerStartRow, Random.Range(0, BoardSize));
			if (spawnCell)
			{
				return Instantiate(_boardData.Player, spawnCell.transform);
			}
			
			Debug.LogError("Не удалось найти клетку для спавна игрока!");
			return null;

		}

		private DragonController SpawnDragon()
		{
			var spawnCell = GetCell(DragonStartRow, Random.Range(0, BoardSize));
			if (spawnCell)
			{
				return Instantiate(_boardData.Dragon, spawnCell.transform);
			}
			
			Debug.LogError("Не удалось найти клетку для спавна дракона!");
			return null;
		}
		
		private ChessPiece SpawnerTreasure()
		{
			var spawnCell = GetCell(Random.Range(MinTreasureStartRow, MaxTreasureStartRow + 1),
				Random.Range(MinTreasureStartCol, MaxTreasureStartCol + 1));
			if (spawnCell)
			{
				return Instantiate(_boardData.TreasureChest, spawnCell.transform);
			}

			Debug.LogError("Не удалось найти клетку для спавна сокровища!");
			return null;
		}

		private void SpawnBombs()
		{
			for (byte i = 0; i < EachItemCount; ++i)
			{
				var row = i % 2 == 0 ? BombsRow : BoardSize - 1 - BombsRow;
				var spawnCell = GetCell(row, Random.Range(0, BoardSize));
				if (spawnCell)
				{
					Instantiate(_boardData.Bomb, spawnCell.transform);
				} 
			}
		}
		
		private void SpawnSpeedBoots()
		{
			for (var i = 0; i < EachItemCount; i++)
			{
				var row = i % 2 == 0 ? SpeedBootsRow : BoardSize - 1 - SpeedBootsRow;
				var spawnCell = GetCell(row, Random.Range(0, BoardSize));
				if (spawnCell)
				{
					_speedBoots[i] = Instantiate(_boardData.SpeedBoot, spawnCell.transform);
				}
			}
		}
		
		public ChessCell GetCell(int row, int col)
		{
			if (row < 0 || row >= BoardSize || col < 0 || col >= BoardSize)
				return null;
		
			var cellId = _boardCells[row, col].GetInstanceID();
			return _cellsDict[cellId];
		}

		public ChessCell GetPlayerCell() => _player.CurrentCell;

		public List<ChessCell> GetAdjacentCells(ChessCell cell)
		{
			var adjacentCells = new List<ChessCell>(MaxNeighboursToCell);
			
			var leftCell = GetCell(cell.Row, cell.Column - 1);
			if (leftCell) adjacentCells.Add(_cellsDict[leftCell.GetInstanceID()]);
			
			var rightCell = GetCell(cell.Row, cell.Column + 1);
			if (rightCell) adjacentCells.Add(_cellsDict[rightCell.GetInstanceID()]);
			
			var upperCell = GetCell(cell.Row + 1, cell.Column);
			if (upperCell) adjacentCells.Add(_cellsDict[upperCell.GetInstanceID()]);
			
			var lowerCell = GetCell(cell.Row - 1, cell.Column);
			if (lowerCell) adjacentCells.Add(_cellsDict[lowerCell.GetInstanceID()]);
			
			return adjacentCells;
		}
	}
}