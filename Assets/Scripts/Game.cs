using System;
using UnityEngine;

namespace NikitaKirakosyan.Minesweeper
{
    public sealed class Game : MonoBehaviour
    {
        public event Action<GameSettingsData> OnGameStarted;
        public event Action<GameSettingsData> OnGameSettingsChanged;

        [SerializeField] private RectTransform _gameWindow;
        [SerializeField] private RectTransform _gameUIRoot;
        [SerializeField] private Board _board;
        [SerializeField] private GameSettingsData[] _gameSettings;

        private int _currentGameSettingsIndex;

        public static Game Instance { get; private set; }

        public GameSettingsData CurrentGameSettings => _gameSettings[_currentGameSettingsIndex];


        private void Awake()
        {
            if(Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            ZoomController.OnZoomChanged += OnZoomChanged;
        }

        private void OnDestroy()
        {
            ZoomController.OnZoomChanged -= OnZoomChanged;
        }


        public void StartGame()
        {
            OnGameStarted?.Invoke(CurrentGameSettings);
            _gameWindow.sizeDelta = CurrentGameSettings.GameWindowSize;
        }

        public void ChangeGameSettings()
        {
            _currentGameSettingsIndex++;
            if(_currentGameSettingsIndex >= _gameSettings.Length)
                _currentGameSettingsIndex = 0;

            OnGameSettingsChanged?.Invoke(CurrentGameSettings);
            StartGame();
        }

        public void ShowHint()
        {
            if(_board.OpenedCells.IsNullOrEmpty())
                return;

            foreach(var boardOpenedCell in _board.OpenedCells)
            {
                var x = boardOpenedCell.MatrixPosition.x;
                var y = boardOpenedCell.MatrixPosition.x;
                var xPositiveOffset = x + 1;
                var xNegativeOffset = x - 1;
                var yPositiveOffset = y + 1;
                var yNegativeOffset = y - 1;

                var cell = _board.GetCell(new Vector2Int(xPositiveOffset, y));
                if(cell == null)
                    cell = _board.GetCell(new Vector2Int(xNegativeOffset, y));
                if(cell == null)
                    cell = _board.GetCell(new Vector2Int(xPositiveOffset, yPositiveOffset));
                if(cell == null)
                    cell = _board.GetCell(new Vector2Int(xPositiveOffset, yNegativeOffset));
                if(cell == null)
                    cell = _board.GetCell(new Vector2Int(xNegativeOffset, yPositiveOffset));
                if(cell == null)
                    cell = _board.GetCell(new Vector2Int(xNegativeOffset, yNegativeOffset));
                if(cell == null)
                    cell = _board.GetCell(new Vector2Int(x, yPositiveOffset));
                if(cell == null)
                    cell = _board.GetCell(new Vector2Int(x, yNegativeOffset));
                
                if(cell != null && !cell.IsOpened && !cell.HasFlag && cell.HasBomb)
                {
                    cell.SetFlag(true);
                    return;
                }
            }
        }


        private void OnZoomChanged(float zoom)
        {
            var scale = 1f + zoom / 100f;
            if(!Mathf.Approximately(_gameUIRoot.localScale.x, scale))
                _gameUIRoot.localScale = new Vector3(scale, scale, scale);
        }
    }
}