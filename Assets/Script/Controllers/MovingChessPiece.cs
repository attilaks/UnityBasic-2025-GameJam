using UnityEngine;
// ReSharper disable InconsistentNaming

namespace Script.Controllers
{
	public class MovingChessPiece : ChessPiece
	{
		[Header("Настройки")]
		[SerializeField] protected float _smoothMoveSpeed = 5f;
		
		protected bool _isMoving;
		
		protected override void Awake()
		{
			base.Awake();
			_isMoving = false;
		}

		protected void MoveToCurrentCell()
		{
			transform.localPosition = Vector3.MoveTowards(transform.localPosition, _aboveCellPosition, 
				_smoothMoveSpeed * Time.deltaTime);
			if (Vector3.Distance(transform.localPosition, _aboveCellPosition) < 0.001f)
			{
				transform.localPosition = _aboveCellPosition;
				_isMoving = false;
			}
		}
	}
}