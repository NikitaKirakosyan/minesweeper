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

        private Dictionary<Vector2Int, Cell> _cellInstances;
        private int _flagsCount;
        
        public int[,] CellsMatrix { get; private set; }


        private void OnEnable()
        {
            Game.OnGameStarted += Setup;
        }

        private void OnDisable()
        {
            Game.OnGameStarted -= Setup;
        }

        private void OnDestroy()
        {
            foreach(var (position, cell) in _cellInstances)
            {
                cell.OnOpened -= _boardHeader.LaunchTimer;
                cell.OnOpened -= OnCellOpened;
                cell.OnFlagStateChanged -= OnCellFlagStateChanged;
            }
        }


        public void Setup(GameSettingsData gameSettings)
        {
            _flagsCount = 0;
            SetSize(gameSettings.BoardSize);
            FillGameField(gameSettings);
            _boardHeader.SetBombsCounter(gameSettings.BombsCount);
        }

        public Cell GetCell(Vector2Int matrixPosition)
        {
            if(!_cellInstances.IsNullOrEmpty() && _cellInstances.TryGetValue(matrixPosition, out var cell))
                return cell;

            return null;
        }


        private void SetSize(Vector2 newSize)
        {
            _rectTransform.sizeDelta = newSize;
        }

        private int GetHorizontalBombsAround(Vector2Int originCellMatrixPosition)
        {
            var bombsAround = 0;
            var x = originCellMatrixPosition.x;
            var y = originCellMatrixPosition.y;

            if(x == 0)
            {
                bombsAround += CellsMatrix[x + 1, y] == 1 ? 1 : 0;
            }
            else if(x > 0)
            {
                bombsAround += CellsMatrix[x - 1, y] == 1 ? 1 : 0;
                if(x + 1 < CellsMatrix.GetLength(0)) 
                    bombsAround += CellsMatrix[x + 1, y] == 1 ? 1 : 0;
            }

            return bombsAround;
        }
        
        private int GetVerticalBombsAround(Vector2Int originCellMatrixPosition)
        {
            var bombsAround = 0;
            var x = originCellMatrixPosition.x;
            var y = originCellMatrixPosition.y;
            
            if(y == 0)
            {
                bombsAround += CellsMatrix[x, y + 1] == 1 ? 1 : 0;
            }
            else if(y > 0)
            {
                bombsAround += CellsMatrix[x, y - 1] == 1 ? 1 : 0;
                if(y + 1 < CellsMatrix.GetLength(1)) 
                    bombsAround += CellsMatrix[x, y + 1] == 1 ? 1 : 0;
            }

            return bombsAround;
        }
        
        private int GetDiagonalBombsAround(Vector2Int originCellMatrixPosition)
        {
            var bombsAround = 0;
            var x = originCellMatrixPosition.x;
            var y = originCellMatrixPosition.y;
            
            if(x > 0 && y == 0)
            {
                bombsAround += CellsMatrix[x - 1, y + 1] == 1 ? 1 : 0;
                if(x + 1 < CellsMatrix.GetLength(0)) 
                    bombsAround += CellsMatrix[x + 1, y + 1] == 1 ? 1 : 0;
            }
            
            if(y > 0 && x == 0)
            {
                bombsAround += CellsMatrix[x + 1, y - 1] == 1 ? 1 : 0;
                if(y + 1 < CellsMatrix.GetLength(1)) 
                    bombsAround += CellsMatrix[x + 1, y + 1] == 1 ? 1 : 0;
            }
            
            if(x > 0 && y > 0)
            {
                bombsAround += CellsMatrix[x - 1, y - 1] == 1 ? 1 : 0;
                if(y + 1 < CellsMatrix.GetLength(1)) 
                    bombsAround += CellsMatrix[x - 1, y + 1] == 1 ? 1 : 0;

                if(x + 1 < CellsMatrix.GetLength(0))
                {
                    bombsAround += CellsMatrix[x + 1, y - 1] == 1 ? 1 : 0;
                    if(y + 1 < CellsMatrix.GetLength(1)) 
                        bombsAround += CellsMatrix[x + 1, y + 1] == 1 ? 1 : 0;
                }
            }
            
            if(x > 0 && y == CellsMatrix.GetLength(1) - 1)
            {
                bombsAround += CellsMatrix[x - 1, y - 1] == 1 ? 1 : 0;
                if(x + 1 < CellsMatrix.GetLength(0)) 
                    bombsAround += CellsMatrix[x + 1, y - 1] == 1 ? 1 : 0;
            }
            
            if(x == CellsMatrix.GetLength(0) - 1 && y > 0)
            {
                bombsAround += CellsMatrix[x - 1, y - 1] == 1 ? 1 : 0;
                if(y + 1 < CellsMatrix.GetLength(1)) 
                    bombsAround += CellsMatrix[x - 1, y + 1] == 1 ? 1 : 0;
            }

            return bombsAround;
        }
        
        private void FillGameField(GameSettingsData gameSettings)
        {
            CellsMatrix = CellsGenerator.Generate(
                gameSettings.CellsColumns,
                gameSettings.CellsRows,
                gameSettings.BombsCount,
                gameSettings.BombsRandomnicity,
                gameSettings.BombsRandomnDelta);

            if(!_cellInstances.IsNullOrEmpty())
            {
                for(var i = _cellInstances.Count - 1; i >= 0; i--)
                    Destroy(_cellInstances.ElementAt(i).Value.gameObject);
            }
            _cellInstances = new Dictionary<Vector2Int, Cell>(CellsMatrix.Length);
            
            
            for(var x = 0; x < CellsMatrix.GetLength(0); x++)
            {
                for(var y = 0; y < CellsMatrix.GetLength(1); y++)
                {
                    var hasBomb = CellsMatrix[x, y] == 1;
                    
                    var cell = Instantiate(_cellPrefab, _gameFieldGrid);
                    cell.OnOpened += _boardHeader.LaunchTimer;
                    cell.OnOpened += OnCellOpened;
                    cell.OnFlagStateChanged += OnCellFlagStateChanged;
                    _cellInstances.Add(new Vector2Int(x, y), cell);

                    var bombsAround = 0;
                    if(x == 0 && y == 0)
                    {
                        bombsAround += CellsMatrix[x + 1, y] == 1 ? 1 : 0;
                        bombsAround += CellsMatrix[x, y + 1] == 1 ? 1 : 0;
                        bombsAround += CellsMatrix[x + 1, y + 1] == 1 ? 1 : 0;
                    }
                    else
                    {
                        var cellMatrixPosition = new Vector2Int(x, y);
                        bombsAround =
                            GetHorizontalBombsAround(cellMatrixPosition) +
                            GetVerticalBombsAround(cellMatrixPosition) +
                            GetDiagonalBombsAround(cellMatrixPosition);
                    }

                    cell.Init(new Vector2Int(x, y), hasBomb, bombsAround);
                }
            }
        }

        private void OnCellOpened()
        {
            foreach(var (cellPosition, cellInstance) in _cellInstances)
            {
                if(cellInstance.IsActive && !cellInstance.HasBomb && cellInstance.IsOpened && cellInstance.BombsAround == 0)
                {
                    if(cellPosition.x == 0 || cellPosition.x < CellsMatrix.GetLength(0) - 1)
                        GetCell(new Vector2Int(cellPosition.x + 1, cellPosition.y)).Open(true);
                    if(cellPosition.x - 1 >= 0)
                        GetCell(new Vector2Int(cellPosition.x - 1, cellPosition.y)).Open(true);
                    if(cellPosition.y == 0 || cellPosition.y < CellsMatrix.GetLength(1) - 1)
                        GetCell(new Vector2Int(cellPosition.x, cellPosition.y + 1)).Open(true);
                    if(cellPosition.y - 1 >= 0)
                        GetCell(new Vector2Int(cellPosition.x, cellPosition.y - 1)).Open(true);

                    if(cellPosition.y > 0 && cellPosition.y < CellsMatrix.GetLength(1) - 1 && cellPosition.x > 0 && cellPosition.x < CellsMatrix.GetLength(0) - 1)
                    {
                        GetCell(new Vector2Int(cellPosition.x - 1, cellPosition.y - 1)).Open(true);
                        GetCell(new Vector2Int(cellPosition.x + 1, cellPosition.y - 1)).Open(true);
                        GetCell(new Vector2Int(cellPosition.x - 1, cellPosition.y + 1)).Open(true);
                        GetCell(new Vector2Int(cellPosition.x + 1, cellPosition.y + 1)).Open(true);
                    }
                    else
                    {
                        if(cellPosition.x > 0 && cellPosition.x < CellsMatrix.GetLength(0) - 1 && cellPosition.y == 0)
                        {
                            GetCell(new Vector2Int(cellPosition.x - 1, cellPosition.y + 1)).Open(true);
                            GetCell(new Vector2Int(cellPosition.x + 1, cellPosition.y + 1)).Open(true);
                        }

                        if(cellPosition.y > 0 && cellPosition.y < CellsMatrix.GetLength(1) - 1 && cellPosition.x == 0)
                        {
                            GetCell(new Vector2Int(cellPosition.x + 1, cellPosition.y - 1)).Open(true);
                            GetCell(new Vector2Int(cellPosition.x + 1, cellPosition.y + 1)).Open(true);
                        }
                    }
                }
            }
        }

        private void OnCellFlagStateChanged(bool hasFlag)
        {
            _flagsCount += hasFlag ? 1 : -1;
            _boardHeader.SetBombsCounter(Game.Instance.CurrentGameSettings.BombsCount - _flagsCount);
        }
    }
}