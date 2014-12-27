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
            this.CheckLocationBounds(location);
            return this.cells[location.Row, location.Column];
        }

        private void CheckLocationBounds(GridLocation location)
        {
            if (location.Row >= this.rowCount || location.Column >= this.columnCount)
            {
                throw new ArgumentOutOfRangeException("cell");
            }
        }

        public void Update(IEnumerable<GridLocation> locations, bool occupied)
        {
            foreach (var location in locations)
            {
                this.CheckLocationBounds(location);
                this.cells[location.Row, location.Column] = occupied;
            }
        }
    }
}
