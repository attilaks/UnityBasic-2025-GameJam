using Script.GlobalManagers;
using UnityEngine;

namespace Script.Controllers
{
	public class BoardController : MonoBehaviour
	{
		[Header("Prefabs")]
		[SerializeField] private GameObject _defaultBoard;
		[SerializeField] private GameObject _player;
		[SerializeField] private GameObject _dragon;
		[SerializeField] private GameObject _treasureChest;
		[SerializeField] private GameObject _bomb;
		
		private GameObject _board;

		private void Awake()
		{
			_board = Instantiate(BoardManager.ChosenBoard ? BoardManager.ChosenBoard : _defaultBoard, transform);
		}
	}
}