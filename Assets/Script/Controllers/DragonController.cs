using System.Collections.Generic;
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
			return ChooseCell(reachableCells, playerCell);
		}

		private static ChessCell ChooseCell(List<ChessCell> reachableNodes, ChessCell playerCell)
		{
			var minCost = int.MaxValue;
			ChessCell bestCell = null;

			for (var i = 0; i < reachableNodes.Count; i++)
			{
				var cell = reachableNodes[i];
				var costToGoalCell = EstimateDistance(cell, playerCell);

				if (costToGoalCell >= minCost) continue;
				
				minCost = costToGoalCell;
				bestCell = cell;
			}

			return bestCell;
		}

		private static int EstimateDistance(ChessCell cell, ChessCell playerCell)
		{
			return Mathf.Abs(cell.Row - playerCell.Row) + Mathf.Abs(cell.Column - playerCell.Column);
		}
	}
}