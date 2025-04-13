using System;
using UnityEngine;

namespace Script.Controllers
{
	public class ChessCell : MonoBehaviour, IEquatable<ChessCell>
	{
		public byte Row { get; private set; }
		public byte Column { get; private set; }
		
		private bool _isInitialized;
    
		public void SetCoordinates(byte row, byte col)
		{
			if (_isInitialized)
			{
				Debug.LogWarning("Координаты уже заданы!");
				return;
			}
			
			Row = row;
			Column = col;
			_isInitialized = true;
		}

		public bool Equals(ChessCell other)
		{
			if (!other) return false;
			if (ReferenceEquals(this, other)) return true;
			return Row == other.Row && Column == other.Column;
		}
	}
}