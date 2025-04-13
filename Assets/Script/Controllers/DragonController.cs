using Script.Enums;
using UnityEngine;

namespace Script.Controllers
{
	public sealed class DragonController : MovingChessPiece
	{
		protected override Actor Actor => Actor.Dragon;

		private readonly struct Cost
		{
			public Cost(int rowCost, int colCost)
			{
				RowCost = rowCost;
				ColumnCost = colCost;
			}

			public readonly int RowCost;
			public readonly int ColumnCost;
			
			public uint AbsoluteCost => (uint)RowCost + (uint)ColumnCost;
		}
		
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
			
			var minCost = new Cost(byte.MaxValue, byte.MaxValue);
			ChessCell bestCell = null;
			var currentRowCost = Mathf.Abs(CurrentCell.Row - playerCell.Row);
			var currentColumCost = Mathf.Abs(CurrentCell.Column - playerCell.Column);
			var shouldDecreaseRowCost = currentRowCost > currentColumCost;

			for (byte i = 0; i < reachableCells.Count; i++)
			{
				var cell = reachableCells[i];
				var costToGoalCell = new Cost(Mathf.Abs(cell.Row - playerCell.Row),  Mathf.Abs(cell.Column - playerCell.Column));

				if (costToGoalCell.AbsoluteCost > minCost.AbsoluteCost) continue;
				if (costToGoalCell.AbsoluteCost == minCost.AbsoluteCost)
				{
					if (shouldDecreaseRowCost && costToGoalCell.RowCost < minCost.RowCost
					    || !shouldDecreaseRowCost && costToGoalCell.ColumnCost < minCost.ColumnCost)
					{
						minCost = costToGoalCell;
						bestCell = cell;
					}
					
					continue;
				}
				
				minCost = costToGoalCell;
				bestCell = cell;
			}

			return bestCell;
		}
	}
}