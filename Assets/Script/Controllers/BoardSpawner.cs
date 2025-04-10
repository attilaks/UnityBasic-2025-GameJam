using Script.GlobalManagers;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

namespace Script.Controllers
{
	public class BoardSpawner : MonoBehaviour
	{
		[Header("Prefabs")]
		[SerializeField] private Board _defaultBoard;

		private void Awake()
		{
			if (BoardManager.ChosenBoard)
			{
				Instantiate(BoardManager.ChosenBoard, new Vector3(0,0,0), Quaternion.identity, transform);
				Destroy(BoardManager.ChosenBoard.gameObject);
			}
			else
			{
				Instantiate(_defaultBoard, transform);
			}
		}
	}
}