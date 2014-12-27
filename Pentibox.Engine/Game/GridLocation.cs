using System;

namespace Pentibox.Engine
{
    public struct GridLocation
    {
        private int row;
        private int column;

        public GridLocation(int row, int column)
        {
            this.row = row;
            this.column = column;
        }

        public void Offset(int rows, int columns)
        {
            this.row += rows;
            this.column += columns;
        }

        public int Row
        {
            get
            {
                return this.row;
            }
        }

        public int Column
        {
            get
            {
                return this.column;
            }
        }

        public static bool operator ==(GridLocation loc1, GridLocation loc2)
        {
            return loc1.row == loc2.row && loc1.column == loc2.column;
        }

        public static bool operator !=(GridLocation loc1, GridLocation loc2)
        {
            return !(loc1 == loc2);
        }
    }
}
