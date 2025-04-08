using System;
using Script.Controllers;
using UnityEngine;

namespace ScriptableObjects
{
	[Serializable]
	[CreateAssetMenu(fileName = "BoardData", menuName = "ScriptableObjects/BoardData")]
	public class BoardData : ScriptableObject
	{
		[SerializeField] private PlayerController _player;
		[SerializeField] private DragonController _dragon;
		[SerializeField] private GameObject _treasureChest;
		[SerializeField] private GameObject _bomb;
		
		public PlayerController Player => _player;
		public DragonController Dragon => _dragon;
		public GameObject TreasureChest => _treasureChest;
		public GameObject Bomb => _bomb;
	}
}