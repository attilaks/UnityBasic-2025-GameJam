using Script.Enums;
using UnityEngine;

namespace Script.Controllers
{
	public sealed class DragonController : MovingChessPiece
	{
		protected override Actor Actor => Actor.Dragon;
		
		private void Update()
		{
			if (_isMoving)
			{
				MoveToCurrentCell();
				return;
			}

			if (_isMyTurn)
			{
				var newCell = CalculateNextMove();
				SetCurrentCell(newCell);
			}
		}

		private ChessCell CalculateNextMove()
		{
			var playerCell = _board.GetPlayerCell();
			if (playerCell.Equals(CurrentCell))
				return CurrentCell;
			
			var reachableCells = _board.GetAdjacentCells(CurrentCell);
			
			var minCost = int.MaxValue;
			ChessCell bestCell = null;

			for (byte i = 0; i < reachableCells.Count; i++)
			{
				var cell = reachableCells[i];
				var costToGoalCell = Mathf.Abs(cell.Row - playerCell.Row) + Mathf.Abs(cell.Column - playerCell.Column);

				if (costToGoalCell >= minCost) continue;
				
				minCost = costToGoalCell;
				bestCell = cell;
			}

			return bestCell;
		}
	}
}