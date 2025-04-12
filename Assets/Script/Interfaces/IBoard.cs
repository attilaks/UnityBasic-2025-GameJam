using System;

namespace Script.Interfaces
{
	public interface IBoard
	{
		event Action<bool> OnEndOfGame;
	}
}