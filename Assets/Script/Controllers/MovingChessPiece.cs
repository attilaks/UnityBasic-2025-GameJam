using System;
using Script.Enums;
using UnityEngine;
// ReSharper disable InconsistentNaming

namespace Script.Controllers
{
	public class MovingChessPiece : ChessPiece
	{
		[Header("Настройки")]
		[SerializeField] protected float _smoothMoveSpeed = 5f;
		
		public event Action<Actor> EndOfTurnEvent = delegate { };
		
		protected bool _isMoving;
		protected bool _isMyTurn;
		protected virtual Actor Actor => Actor.Dragon;
		
		protected override void Awake()
		{
			base.Awake();
			_isMoving = false;

			_board.NextTurnEvent += OnNextTurn;
			_board.OnEndOfGame += OnEndOfGame;
		}

		private void OnDestroy()
		{
			_board.NextTurnEvent -= OnNextTurn;
			_board.OnEndOfGame -= OnEndOfGame;
		}

		private void OnNextTurn(Actor nextActorSide)
		{
			_isMyTurn = nextActorSide == Actor;
		}
		
		private void OnEndOfGame(bool playerWon)
		{
			_isMoving = false;
			_isMyTurn = false;
		}

		protected void SetCurrentCell(ChessCell newCell)
		{
			CurrentCell = newCell;
			transform.parent = newCell.transform;
			_isMoving = true;
		}

		protected void MoveToCurrentCell()
		{
			transform.localPosition = Vector3.MoveTowards(transform.localPosition, _aboveCellPosition, 
				_smoothMoveSpeed * Time.deltaTime);
			if (Vector3.Distance(transform.localPosition, _aboveCellPosition) < 0.001f)
			{
				transform.localPosition = _aboveCellPosition;
				_isMoving = false;
				EndOfTurnEvent.Invoke(Actor);
			}
		}
	}
}