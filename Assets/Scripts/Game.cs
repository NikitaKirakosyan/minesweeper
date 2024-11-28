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
        public bool IsGameStarted { get; private set; }
        public bool IsOpenAndFlagInversed { get; private set; }


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
            IsGameStarted = true;
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

        public void InverseOpenAndFlag()
        {
            IsOpenAndFlagInversed = !IsOpenAndFlagInversed;
        }
        
        public void ShowHint()
        {
            if(!IsGameStarted)
                return;
            
            for(var x = 0; x < _board.CellsMatrix.GetLength(0); x++)
            for(var y = 0; y < _board.CellsMatrix.GetLength(1); y++)
            {
                var cellInMatrix = _board.GetCell(new Vector2Int(x, y));
                if(!cellInMatrix.IsOpened || cellInMatrix.HasFlag)
                    continue;

                var xPositiveOffset = x + 1;
                var xNegativeOffset = x - 1;
                var yPositiveOffset = y + 1;
                var yNegativeOffset = y - 1;
                var possiblePositions = new Vector2Int[]
                {
                    new (xPositiveOffset, yPositiveOffset),
                    new (xPositiveOffset, yNegativeOffset),
                    new (xNegativeOffset, yPositiveOffset),
                    new (xNegativeOffset, yNegativeOffset),
                    new (x, yPositiveOffset),
                    new (x, yNegativeOffset),
                    new (xPositiveOffset, y),
                    new (xNegativeOffset, y)
                };
                
                foreach(var possiblePosition in possiblePositions)
                {
                    var cellToHint = _board.GetCell(possiblePosition);
                    if(TryShowHint(cellToHint))
                        return;
                }
            }
        }


        private bool TryShowHint(Cell cellToHint)
        {
            if(cellToHint != null && !cellToHint.IsOpened && !cellToHint.HasFlag && cellToHint.HasBomb)
            {
                cellToHint.SetFlag(true);
                return true;
            }

            return false;
        }

        private void OnZoomChanged(float zoom)
        {
            var scale = 1f + zoom / 100f;
            if(!Mathf.Approximately(_gameUIRoot.localScale.x, scale))
                _gameUIRoot.localScale = new Vector3(scale, scale, scale);
        }
    }
}