using Script.GlobalManagers;
using UnityEngine;

namespace Script.Controllers
{
	public class BoardSpawner : MonoBehaviour
	{
		[Header("Prefabs")]
		[SerializeField] private Board _defaultBoard;

		private void Awake()
		{
			Instantiate(BoardManager.ChosenBoard ? BoardManager.ChosenBoard : _defaultBoard, transform);
		}
	}
}