using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Script.Enums;
using Script.Interfaces;
using ScriptableObjects;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

namespace Script.Controllers
{
	[RequireComponent(typeof(AudioSource))]
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
		private readonly List<ChessPiece> _speedBoots = new (2);
		private readonly List<ChessPiece> _bombs = new (2);
		private readonly List<ChessPiece> _portals = new (2);
		
		private readonly Dictionary<int, ChessCell> _cellsDict = new();
		private AudioSource _audioSource;
		
		public event Action<Actor> NextTurnEvent = delegate { };

		private void Awake()
		{
			if (SceneManager.GetActiveScene().buildIndex != 1) return;
			
			_boardCells = new ChessCell[BoardSize, BoardSize];
			_audioSource = GetComponent<AudioSource>();
			InitializeBoard();
			
			_player = SpawnPlayer();
			_dragon = SpawnDragon();
			_treasureChest = SpawnTreasure();
			SpawnBombs();
			SpawnSpeedBoots();
			SpawnPortals();
			
			NextTurnEvent.Invoke(Actor.Player);
		}

		private void OnDestroy()
		{
			if (SceneManager.GetActiveScene().buildIndex != 1) return;

			if (_player)
			{
				_player.EndOfTurnEvent -= HandleEndOfTurn;
			}

			if (_dragon)
			{
				_dragon.EndOfTurnEvent -= HandleEndOfTurn;
			}
		}

		private void HandleEndOfTurn(Actor actorSide)
		{
			if (_dragon.CurrentCell.Equals(_player.CurrentCell))
			{
				_audioSource.PlayOneShot(_boardData.DragonAtePlayerSound);
				PlayerIsDefeated();
				return;
			}
			
			if (_player.CurrentCell.Equals(_treasureChest.CurrentCell))
			{
				_audioSource.PlayOneShot(_boardData.WinSound);
				PlayerWon();
				return;
			}

			if (_speedBoots.Count > 0)
			{
				var speedBoot = _speedBoots.FirstOrDefault(x => _player.CurrentCell.Equals(x.CurrentCell) 
				                                                || _dragon.CurrentCell.Equals(x.CurrentCell));
				if (speedBoot)
				{
					_audioSource.PlayOneShot(_boardData.SpeedBootsActivationSound);
					_speedBoots.Remove(speedBoot);
					Destroy(speedBoot.gameObject);
					NextTurnEvent.Invoke(actorSide);
					return;
				}
			}

			if (_bombs.Count > 0)
			{
				if (_bombs.Any(x => _player.CurrentCell.Equals(x.CurrentCell)))
				{
					_audioSource.PlayOneShot(_boardData.BombSound);
					PlayerIsDefeated();
					return;
				}

				var bombUnderDragon = _bombs.FirstOrDefault(x => _dragon.CurrentCell.Equals(x.CurrentCell));
				if (bombUnderDragon)
				{
					_audioSource.PlayOneShot(_boardData.BombSound);
					_dragon = SpawnDragon();
					_bombs.Remove(bombUnderDragon);
					Destroy(bombUnderDragon.gameObject);
				}
			}
			
			if (_portals.Count > 1 && actorSide == Actor.Player && _portals.Any(x => _player.CurrentCell.Equals(x.CurrentCell)))
			{
				_audioSource.PlayOneShot(_boardData.PortalSound);
				var portal = _portals.First(x => !_player.CurrentCell.Equals(x.CurrentCell));
				_player = SpawnPlayer(portal.CurrentCell);
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

		private PlayerController SpawnPlayer([CanBeNull] ChessCell cell = null)
		{
			if (_player) Destroy(_player.gameObject);
			
			var spawnCell = cell ? cell : GetCell(PlayerStartRow, Random.Range(1, BoardSize - 1));
			if (spawnCell)
			{
				var player = Instantiate(_boardData.Player, spawnCell.transform);
				player.EndOfTurnEvent += HandleEndOfTurn;
				return player;
			}
			
			throw new Exception("Не удалось найти клетку для спавна игрока!");
		}

		private DragonController SpawnDragon([CanBeNull] ChessCell cell = null)
		{
			if (_dragon) Destroy(_dragon.gameObject);
			
			var spawnCell = cell ? cell : GetCell(DragonStartRow, Random.Range(1, BoardSize - 1));
			if (spawnCell)
			{
				var dragon = Instantiate(_boardData.Dragon, spawnCell.transform);
				dragon.EndOfTurnEvent += HandleEndOfTurn;
				return dragon;
			}
			
			throw new Exception("Не удалось найти клетку для спавна дракона!");
		}
		
		private ChessPiece SpawnTreasure()
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
				var spawnCell = GetCell(row, Random.Range(1, BoardSize - 1));
				if (spawnCell)
				{
					_bombs.Add(Instantiate(_boardData.Bomb, spawnCell.transform));
				} 
			}
		}
		
		private void SpawnSpeedBoots()
		{
			for (var i = 0; i < EachItemCount; i++)
			{
				var row = i % 2 == 0 ? SpeedBootsRow : BoardSize - 1 - SpeedBootsRow;
				var spawnCell = GetCell(row, Random.Range(1, BoardSize - 1));
				if (spawnCell)
				{
					_speedBoots.Add(Instantiate(_boardData.SpeedBoot, spawnCell.transform));
				}
			}
		}

		private void SpawnPortals()
		{
			for (var i = 0; i < EachItemCount; i++)
			{
				var column = i % 2 == 0 ? 0 : BoardSize - 1;
				var spawnCell = GetCell(Random.Range(0, BoardSize), column);
				if (spawnCell)
				{
					_portals.Add(Instantiate(_boardData.Portal, spawnCell.transform));
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