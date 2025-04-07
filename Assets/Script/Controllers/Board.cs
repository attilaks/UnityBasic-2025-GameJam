using System;
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

		public const int BoardSize = 8;
		private const int PlayerStartRow = 0;
		private const int DragonStartRow = BoardSize - 1;

		private Transform[,] _boardCells;
		private PlayerController _player;
		private DragonController _dragon;
		
		public event Action<Turn> NextTurnEvent = delegate { };

		private void Awake()
		{
			if (SceneManager.GetActiveScene().buildIndex != 1) return;
			
			_boardCells = new Transform[BoardSize, BoardSize];
			InitializeBoard();
			
			_player = SpawnPlayer();
			_dragon = SpawnDragon();

			_player.EndOfTurnEvent += PassMove;
			_dragon.EndOfTurnEvent += PassMove;
			
			NextTurnEvent.Invoke(Turn.Player);
		}

		private void OnDestroy()
		{
			if (SceneManager.GetActiveScene().buildIndex != 1) return;
			
			_player.EndOfTurnEvent -= PassMove;
			_dragon.EndOfTurnEvent -= PassMove;
		}

		private void PassMove(Turn turnSide)
		{
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
		
		public Transform GetCell(int row, int col)
		{
			if (row < 0 || row >= BoardSize || col < 0 || col >= BoardSize)
				return null;
        
			return _boardCells[row, col];
		}
	}
}