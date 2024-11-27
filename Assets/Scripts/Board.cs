using System.Collections.Generic;
using UnityEngine;

namespace NikitaKirakosyan.Minesweeper
{
    public class Board : MonoBehaviour
    {
        [SerializeField] private RectTransform _rectTransform;
        [SerializeField] private RectTransform _gameFieldGrid;
        [SerializeField] private Cell _cellPrefab;
        [SerializeField] private BoardHeader _boardHeader;

        private List<Cell> _cellInstances;


        private void Awake()
        {
            Game.Instance.OnGameStarted += Setup;
            Game.Instance.OnGameSettingsChanged += Setup;
        }

        private void OnDestroy()
        {
            Game.Instance.OnGameStarted -= Setup;
            Game.Instance.OnGameSettingsChanged -= Setup;
        }
        

        public void Setup(GameSettingsData gameSettings)
        {
            SetSize(gameSettings.GameWindowSize);
            FillGameField(gameSettings);
            _boardHeader.SetBombsCounter(gameSettings.BombsCount);
            _boardHeader.LaunchTimer();
        }
        
        
        private void SetSize(Vector2 newSize)
        {
            _rectTransform.sizeDelta = newSize;
        }
        
        private void FillGameField(GameSettingsData gameSettings)
        {
            var cellsMatrix = CellsGenerator.Generate(
                gameSettings.CellsColumns,
                gameSettings.CellsRows,
                gameSettings.BombsCount,
                (1, 1),
                gameSettings.BombsRandomnicity,
                gameSettings.BombsRandomnDelta);
            
            _cellInstances ??= new List<Cell>(cellsMatrix.Length);
            var cellIndex = 0;
            
            for(var x = 0; x < cellsMatrix.GetLength(0); x++)
            {
                for(var y = 0; y < cellsMatrix.GetLength(1); y++)
                {
                    var hasBomb = cellsMatrix[x, y] == 1;
                    
                    if(cellIndex < _cellInstances.Count)
                    {
                        _cellInstances[cellIndex].Init(hasBomb);
                    }
                    else
                    {
                        var newCell = Instantiate(_cellPrefab, _gameFieldGrid);
                        newCell.Init(hasBomb);
                        _cellInstances.Add(newCell);
                    }

                    cellIndex++;
                }
            }
        }
    }
}