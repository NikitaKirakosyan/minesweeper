using UnityEngine;

namespace NikitaKirakosyan.Minesweeper
{
    public sealed class Game : MonoBehaviour
    {
        [SerializeField] private GameSettingsData[] _gameSettings;
        [SerializeField] private Board _board;

        private int _currentGameSettingsIndex = 0;

        public static Game Instance { get; private set; }

        private GameSettingsData CurrentGameSettings => _gameSettings[_currentGameSettingsIndex];


        private void Awake()
        {
            if(Instance != null)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
        }
        
        
        public void StartGame()
        {
            var cellsMatrix = CellsGenerator.Generate(
                CurrentGameSettings.CellsColumns,
                CurrentGameSettings.CellsRows,
                CurrentGameSettings.BombsCount,
                (1, 1),
                CurrentGameSettings.BombsRandomnicity,
                CurrentGameSettings.BombsRandomnDelta);
            
            _board.FillGameField(cellsMatrix);
        }

        public void ChangeGameSettings()
        {
            _currentGameSettingsIndex++;
            if(_currentGameSettingsIndex >= _gameSettings.Length)
                _currentGameSettingsIndex = 0;

            _board.SetSize(CurrentGameSettings.GameWindowSize);
        }
    }
}