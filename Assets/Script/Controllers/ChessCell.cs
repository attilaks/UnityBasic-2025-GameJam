using UnityEngine;

namespace Script.Controllers
{
	public class ChessCell : MonoBehaviour
	{
		public int Row { get; private set; }
		public int Column { get; private set; }
		public string ChessNotation =>  $"{(char)('a' + Column)}{8 - Row}";
    
		public void SetCoordinates(int row, int col)
		{
			Row = row;
			Column = col;
		}
		
		private void OnMouseEnter()
		{
			Debug.Log($"Клетка {ChessNotation} (ряд {Row}, колонка {Column})");
		}
	}
}