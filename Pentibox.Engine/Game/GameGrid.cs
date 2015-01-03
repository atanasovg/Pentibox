using System;
using System.Collections.Generic;

namespace Pentibox.Engine
{
    internal class GameGrid : IGameGrid
    {
        private bool[,] cells;
        private int rowCount;
        private int columnCount;

        public GameGrid(int rowCount, int columnCount)
        {
            this.rowCount = rowCount;
            this.columnCount = columnCount;
            this.cells = new bool[this.rowCount, this.columnCount];
        }

        public int RowCount
        {
            get
            {
                return this.rowCount;
            }
        }

        public int ColumnCount
        {
            get
            {
                return this.columnCount;
            }
        }

        public bool IsOccupied(GridLocation location)
        {
            return this.IsOccupied(location.Row, location.Column);
        }

        public bool IsOccupied(int row, int column)
        {
            this.CheckLocationBounds(row, column);
            return this.cells[row, column];
        }

        private void CheckLocationBounds(int row, int column)
        {
            if (row >= this.rowCount || column >= this.columnCount)
            {
                throw new ArgumentOutOfRangeException("cell");
            }
        }

        public void Update(IEnumerable<GridLocation> locations, bool occupied)
        {
            foreach (var location in locations)
            {
                this.CheckLocationBounds(location.Row, location.Column);
                this.cells[location.Row, location.Column] = occupied;
            }
        }

        public void Reset()
        {
            this.cells = new bool[this.rowCount, this.columnCount];
        }
    }
}
