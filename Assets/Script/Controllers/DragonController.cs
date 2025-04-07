using Script.Enums;
using UnityEngine;

namespace Script.Controllers
{
	public sealed class DragonController : MovingChessPiece
	{
		protected override Turn _turn => Turn.Dragon;
		
		private void Update()
		{
			if (_isMoving)
			{
				MoveToCurrentCell();
				return;
			}

			if (_isMyTurn)
			{
				CalculateNextMove();
				MoveToCurrentCell();
			}
		}

		private void CalculateNextMove()
		{
			//todo
			Debug.Log("Ход дракона");
		}
	}
}