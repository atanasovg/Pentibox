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

        void Update(IEnumerable<GridLocation> locations, bool occupied);

        bool IsOccupied(GridLocation location);
    }
}
