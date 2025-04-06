using Script.GlobalManagers;
using UnityEngine;

namespace Script.Controllers
{
	public class BoardController : MonoBehaviour
	{
		[Header("Prefabs")]
		[SerializeField] private Board _defaultBoard;
		
		private Board _board;

		private void Awake()
		{
			_board = Instantiate(BoardManager.ChosenBoard ? BoardManager.ChosenBoard : _defaultBoard, transform);
		}
	}
}