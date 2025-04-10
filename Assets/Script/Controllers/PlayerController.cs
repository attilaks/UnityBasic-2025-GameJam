using System;
using Script.Enums;
using UnityEngine;

namespace Script.Controllers
{
	public sealed class PlayerController : MovingChessPiece
	{
        [Header("Управление")]
        [SerializeField] private KeyCode _upKey = KeyCode.UpArrow;
        [SerializeField] private KeyCode _downKey = KeyCode.DownArrow;
        [SerializeField] private KeyCode _leftKey = KeyCode.LeftArrow;
        [SerializeField] private KeyCode _rightKey = KeyCode.RightArrow;

        protected override Turn _turn => Turn.Player;

        private void Update()
        {
            if (_isMoving)
            {
                MoveToCurrentCell();
                return;
            }
            
            if (_isMyTurn)
                HandleInput();
        }
        
        private void HandleInput()
        {
            if (Input.GetKeyDown(_upKey))
            {
                TryMove(1, 0);
            }
            else if (Input.GetKeyDown(_downKey))
            {
                TryMove(-1, 0);
            }
            else if (Input.GetKeyDown(_leftKey))
            {
                TryMove(0, -1);
            }
            else if (Input.GetKeyDown(_rightKey))
            {
                TryMove(0, 1);
            }
        }
        
        private void TryMove(int rowDelta, int colDelta)
        {
            var newRow = CurrentCell.Row + rowDelta;
            var newCol = CurrentCell.Column + colDelta;
            
            if (newRow < 0 || newRow >= Board.BoardSize || 
                newCol < 0 || newCol >= Board.BoardSize)
            {
                throw new IndexOutOfRangeException("Нельзя выйти за пределы доски!");
            }
            
            var targetCell = _board.GetCell(newRow, newCol);
            if (!targetCell)
            {
                throw new Exception("Целевая клетка не найдена!");
            }
            
            SetCurrentCell(targetCell);
        }
	}
}