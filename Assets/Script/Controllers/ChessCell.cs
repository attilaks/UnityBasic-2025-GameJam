using System;
using Script.Enums;
using UnityEngine;

namespace Script.Controllers
{
	public class ChessCell : MonoBehaviour, IEquatable<ChessCell>
	{
		public byte Row { get; private set; }
		public byte Column { get; private set; }
		
		public event Action<Actor> OnCellEnter = delegate { };
		
		private string ChessNotation =>  $"{(char)('a' + Column)}{8 - Row}";
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
		
		private void OnMouseEnter()
		{
			Debug.Log($"Клетка {ChessNotation} (ряд {Row}, колонка {Column})");
		}

		public bool Equals(ChessCell other)
		{
			if (!other) return false;
			if (ReferenceEquals(this, other)) return true;
			return Row == other.Row && Column == other.Column;
		}
	}
}