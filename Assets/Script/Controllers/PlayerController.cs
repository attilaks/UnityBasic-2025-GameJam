using UnityEngine;

namespace Script.Controllers
{
	public class PlayerController : MonoBehaviour
	{
        [Header("Настройки")]
        [SerializeField] private float _smoothMoveSpeed = 5f;
        [SerializeField] private KeyCode _upKey = KeyCode.UpArrow;
        [SerializeField] private KeyCode _downKey = KeyCode.DownArrow;
        [SerializeField] private KeyCode _leftKey = KeyCode.LeftArrow;
        [SerializeField] private KeyCode _rightKey = KeyCode.RightArrow;
        
        private Board _board;
        private ChessCell _currentCell;
        
        private bool _isMoving;
        private Vector3 _targetPosition;

        void Start()
        {
            // Находим систему координат доски
            _board = FindObjectOfType<Board>();
            if (_board == null)
            {
                Debug.LogError("Board не найден на сцене!");
                return;
            }
            
            // Получаем текущую клетку
            _currentCell = transform.parent.GetComponent<ChessCell>();
            if (!_currentCell)
            {
                Debug.LogError("Компонент ChessCell не найден на игроке!");
                return;
            }
            
            _targetPosition = transform.position;
        }

        private void Update()
        {
            // Плавное перемещение к целевой позиции
            if (_isMoving)
            {
                transform.position = Vector3.MoveTowards(transform.position, _targetPosition, _smoothMoveSpeed * Time.deltaTime);
                if (Vector3.Distance(transform.position, _targetPosition) < 0.001f)
                {
                    transform.position = _targetPosition;
                    _isMoving = false;
                }
                return;
            }
            
            // Обработка ввода только когда не двигаемся
            HandleInput();
        }
        
        private void HandleInput()
        {
            if (Input.GetKeyDown(_upKey))
            {
                TryMove(1, 0); // Вверх (+1 к ряду)
            }
            else if (Input.GetKeyDown(_downKey))
            {
                TryMove(-1, 0); // Вниз (-1 к ряду)
            }
            else if (Input.GetKeyDown(_leftKey))
            {
                TryMove(0, -1); // Влево (-1 к колонке)
            }
            else if (Input.GetKeyDown(_rightKey))
            {
                TryMove(0, 1); // Вправо (+1 к колонке)
            }
        }
        
        private void TryMove(int rowDelta, int colDelta)
        {
            int newRow = _currentCell.Row + rowDelta;
            int newCol = _currentCell.Column + colDelta;
            
            // Проверяем, что новые координаты в пределах доски
            if (newRow < 0 || newRow >= Board.BoardSize || 
                newCol < 0 || newCol >= Board.BoardSize)
            {
                Debug.Log("Нельзя выйти за пределы доски!");
                return;
            }
            
            // Получаем целевую клетку
            Transform targetCell = _board.GetCell(newRow, newCol);
            if (targetCell == null)
            {
                Debug.LogError("Целевая клетка не найдена!");
                return;
            }
            
            // Можно добавить дополнительные проверки (например, занята ли клетка)
            
            // Обновляем позицию
            _currentCell = targetCell.GetComponent<ChessCell>();
            _targetPosition = targetCell.position;
            _isMoving = true;
            
            Debug.Log($"Переместились на {_currentCell.ChessNotation}");
        }
        
        // Метод для принудительной установки позиции (например, при старте игры)
        public void SetPosition(int row, int col)
        {
            if (!_board)
                _board = FindObjectOfType<Board>();
            
            Transform cell = _board.GetCell(row, col);
            if (cell != null)
            {
                _currentCell = cell.GetComponent<ChessCell>();
                transform.localPosition = cell.localPosition + new Vector3(0, 1, 0);
                _targetPosition = cell.position;
                _isMoving = false;
            }
        }
        
        // Метод для принудительной установки позиции по шахматной нотации
        public void SetPosition(string chessNotation)
        {
            Transform cell = _board.GetCell(chessNotation);
            if (cell != null)
            {
                _currentCell = cell.GetComponent<ChessCell>();
                transform.position = cell.position;
                _targetPosition = cell.position;
                _isMoving = false;
            }
        }
	}
}