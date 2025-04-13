using System;
using Script.Controllers;
using UnityEngine;

namespace ScriptableObjects
{
	[Serializable]
	[CreateAssetMenu(fileName = "BoardData", menuName = "ScriptableObjects/BoardData")]
	public class BoardData : ScriptableObject
	{
		[Header("Figures")]
		[SerializeField] private PlayerController _player;
		[SerializeField] private DragonController _dragon;
		[SerializeField] private ChessPiece _treasureChest;
		[SerializeField] private ChessPiece _bomb;
		[SerializeField] private ChessPiece _speedBoot;
		[SerializeField] private ChessPiece _portal;
		
		[Header("Audio")]
		[SerializeField] private AudioClip _winSound;
		[SerializeField] private AudioClip _bombSound;
		[SerializeField] private AudioClip _dragonAtePlayerSound;
		[SerializeField] private AudioClip _speedBootsActivationSound;
		[SerializeField] private AudioClip _portalSound;
		
		public PlayerController Player => _player;
		public DragonController Dragon => _dragon;
		public ChessPiece TreasureChest => _treasureChest;
		public ChessPiece Bomb => _bomb;
		public ChessPiece SpeedBoot => _speedBoot;
		public ChessPiece Portal => _portal;
		
		public AudioClip WinSound => _winSound;
		public AudioClip BombSound => _bombSound;
		public AudioClip DragonAtePlayerSound => _dragonAtePlayerSound;
		public AudioClip SpeedBootsActivationSound => _speedBootsActivationSound;
		public AudioClip PortalSound => _portalSound;
	}
}