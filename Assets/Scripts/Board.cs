using System.Collections.Generic;
using UnityEngine;

namespace NikitaKirakosyan.Minesweeper
{
    public class Board : MonoBehaviour
    {
        [SerializeField] private RectTransform _rectTransform;
        [SerializeField] private RectTransform _gameFieldGrid;
        [SerializeField] private Cell _cellPrefab;

        private List<Cell> _cellInstances;


        public void SetSize(Vector2 newSize)
        {
            _rectTransform.sizeDelta = newSize;
        }

        public void FillGameField(int[,] cellsMatrix)
        {
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