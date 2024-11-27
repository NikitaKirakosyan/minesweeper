using System;

namespace NikitaKirakosyan.Minesweeper
{
    public static class CellsGenerator
    {
        /// <summary>
        /// The matrix will be filled with the values and 1.
        /// Where 1 is the presence of a bomb on the cell.
        /// </summary>
        /// <param name="cellsColumns"></param>
        /// <param name="cellsRows"></param>
        /// <param name="bombsCount"></param>
        /// <param name="pressedCellIndex"></param>
        /// <returns></returns>
        public static int[,] Generate(int cellsColumns, int cellsRows, int bombsCount, (int xPos, int yPos) pressedCellIndex, int bombsRandomnicity = 100, int bombsRandomnDelta = 3)
        {
            var random = new Random();
            var cellsMatrix = new int[cellsColumns, cellsRows];
            var bombsGeneratedCount = 0;
            var iterationsLeft = cellsMatrix.Length;
            
            for(var x = 0; x < cellsMatrix.GetLength(0); x++)
            {
                for(var y = 0; y < cellsMatrix.GetLength(1); y++)
                {
                    if(x == pressedCellIndex.xPos && y == pressedCellIndex.yPos)
                    {
                        cellsMatrix[x, y] = 0;
                    }
                    else
                    {
                        var hasBomb = false;
                        
                        var requiredBombs = bombsCount - bombsGeneratedCount;
                        if(requiredBombs >= iterationsLeft)
                            hasBomb = true;
                        else if(bombsGeneratedCount < bombsCount)
                            hasBomb = random.Next(0, bombsRandomnicity) < bombsRandomnicity / bombsRandomnDelta;

                        if(hasBomb)
                            bombsGeneratedCount++;
                        
                        cellsMatrix[x, y] = hasBomb ? 1 : 0;
                    }

                    iterationsLeft--;
                }
            }

            return cellsMatrix;
        }
    }
}