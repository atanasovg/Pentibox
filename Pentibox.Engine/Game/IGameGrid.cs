using System;
using System.Collections.Generic;

namespace Pentibox.Engine
{
    public interface IGameGrid
    {
        int RowCount
        {
            get;
        }

        int ColumnCount
        {
            get;
        }

        void Reset();

        void Update(IEnumerable<GridLocation> locations, bool occupied);

        bool IsOccupied(GridLocation location);

        bool IsOccupied(int row, int column);
    }
}
