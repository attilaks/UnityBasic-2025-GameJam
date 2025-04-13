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
			if (InterSceneObjects.ChosenBoard)
			{
				InterSceneObjects.ChosenBoard.gameObject.SetActive(true);
				_currentBoard = Instantiate(InterSceneObjects.ChosenBoard, new Vector3(0,0,0), Quaternion.identity, transform);
				InterSceneObjects.ChosenBoard.gameObject.SetActive(false);
			}
			else
			{
				_currentBoard = Instantiate(_defaultBoard, new Vector3(0,0,0), Quaternion.identity, transform);
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