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
		[SerializeField] private ChessPiece _treasureChest;
		[SerializeField] private ChessPiece _bomb;
		[SerializeField] private ChessPiece _speedBoot;
		[SerializeField] private ChessPiece _portal;
		
		public PlayerController Player => _player;
		public DragonController Dragon => _dragon;
		public ChessPiece TreasureChest => _treasureChest;
		public ChessPiece Bomb => _bomb;
		public ChessPiece SpeedBoot => _speedBoot;
		public ChessPiece Portal => _portal;
	}
}