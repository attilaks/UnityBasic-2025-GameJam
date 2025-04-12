using System;
using Script.GlobalManagers;
using Script.Interfaces;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

namespace Script.Controllers
{
	public class BoardSpawner : MonoBehaviour
	{
		[Header("Prefabs")]
		[SerializeField] private Board _defaultBoard;
		
		public event Action<bool> OnEndOfGame = delegate { };
		
		private IBoard _currentBoard;

		private void Awake()
		{
			if (BoardManager.ChosenBoard)
			{
				_currentBoard = Instantiate(BoardManager.ChosenBoard, new Vector3(0,0,0), Quaternion.identity, transform);
				Destroy(BoardManager.ChosenBoard.gameObject);
			}
			else
			{
				_currentBoard = Instantiate(_defaultBoard, transform);
			}

			_currentBoard.OnEndOfGame += OnEndOfGameHandler;
		}

		private void OnDestroy()
		{
			_currentBoard.OnEndOfGame -= OnEndOfGameHandler;
		}

		private void OnEndOfGameHandler(bool playerWon)
		{
			OnEndOfGame.Invoke(playerWon);
		}
	}
}