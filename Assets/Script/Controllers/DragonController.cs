using UnityEngine;

namespace Script.Controllers
{
	public sealed class DragonController : MovingChessPiece
	{
		private void Update()
		{
			if (!_isMoving) return;
			MoveToCurrentCell();
		}
	}
}