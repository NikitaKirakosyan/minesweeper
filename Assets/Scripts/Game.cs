using System;
using UnityEngine;

namespace NikitaKirakosyan.Minesweeper
{
    public sealed class Game : MonoBehaviour
    {
        public event Action<GameSettingsData> OnGameStarted; 
        public event Action<GameSettingsData> OnGameSettingsChanged; 
        
        [SerializeField] private GameSettingsData[] _gameSettings;

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
            OnGameStarted?.Invoke(CurrentGameSettings);
        }

        public void ChangeGameSettings()
        {
            _currentGameSettingsIndex++;
            if(_currentGameSettingsIndex >= _gameSettings.Length)
                _currentGameSettingsIndex = 0;

            OnGameSettingsChanged?.Invoke(CurrentGameSettings);
        }
    }
}