using System.Collections.Generic;
using System.Linq;
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
        
        public int[,] CellsMatrix { get; private set; }


        private void OnEnable()
        {
            Game.OnGameStarted += Setup;
        }

        private void OnDisable()
        {
            Game.OnGameStarted -= Setup;
        }


        public void Setup(GameSettingsData gameSettings)
        {
            SetSize(gameSettings.BoardSize);
            FillGameField(gameSettings);
            _boardHeader.SetBombsCounter(gameSettings.BombsCount);
        }

        public Cell GetCell(Vector2Int matrixPosition)
        {
            if(!_cellInstances.IsNullOrEmpty())
            {
                foreach(var cellInstance in _cellInstances)
                {
                    if(cellInstance.IsActive && cellInstance.MatrixPosition == matrixPosition)
                        return cellInstance;
                }
            }

            return null;
        }


        private void SetSize(Vector2 newSize)
        {
            _rectTransform.sizeDelta = newSize;
        }

        private void FillGameField(GameSettingsData gameSettings)
        {
            CellsMatrix = CellsGenerator.Generate(
                gameSettings.CellsColumns,
                gameSettings.CellsRows,
                gameSettings.BombsCount,
                gameSettings.BombsRandomnicity,
                gameSettings.BombsRandomnDelta);

            _cellInstances ??= new List<Cell>(CellsMatrix.Length);

            var cellIndex = 0;
            for(var x = 0; x < CellsMatrix.GetLength(0); x++)
            {
                for(var y = 0; y < CellsMatrix.GetLength(1); y++)
                {
                    var hasBomb = CellsMatrix[x, y] == 1;
                    Cell cell;

                    if(cellIndex < _cellInstances.Count)
                    {
                        cell = _cellInstances[cellIndex];
                    }
                    else
                    {
                        cell = Instantiate(_cellPrefab, _gameFieldGrid);
                        cell.OnOpened += () => _boardHeader.LaunchTimer();
                        _cellInstances.Add(cell);
                    }

                    cell.Init(new Vector2Int(x, y), hasBomb);
                    cellIndex++;
                }
            }

            var cellsDelta = _cellInstances.Count - 1 - Mathf.Abs(cellIndex - _cellInstances.Count);
            for(var i = _cellInstances.Count - 1; i > cellsDelta; i--)
                _cellInstances[i].SetActive(false);
        }
    }
}