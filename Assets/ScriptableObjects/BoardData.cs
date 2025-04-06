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
		[SerializeField] private GameObject _dragon;
		[SerializeField] private GameObject _treasureChest;
		[SerializeField] private GameObject _bomb;
		
		public PlayerController Player => _player;
		public GameObject Dragon => _dragon;
		public GameObject TreasureChest => _treasureChest;
		public GameObject Bomb => _bomb;
	}
}