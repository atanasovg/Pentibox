using System;
using System.Collections.Generic;

namespace Pentibox.Engine
{
    public class Figure
    {
        // The location that represents the top-left corner of the Box that encapsulates the figure.
        private GridLocation location;
        // The number of rows this figure is spanned onto.
        private int rowSpan;
        // The number of columns this figure is spanned onto.
        private int columnSpan;
        // The squares that are occupied (filled) by the current state of the figure.
        private GridLocation[] locations;
        // The game grid this figure is associated with.
        private IGameGrid grid;
        // The concrete layout that defines the figure's occupied locations for each rotation angle.
        private IFigureLayout layout;
        private RotationAngle angle;

        public Figure(IFigureLayout layout)
        {
            if (layout == null)
            {
                throw new ArgumentNullException("layout");
            }

            this.layout = layout;
            this.angle = RotationAngle.Deg0;
        }

        public bool Initialize(IGameGrid grid)
        {
            if (grid == null)
            {
                throw new ArgumentNullException("grid");
            }

            this.grid = grid;
            // initially the figure is centered horizontally
            this.location = new GridLocation(0, (this.grid.ColumnCount - this.layout.InitialColumnSpan) / 2);
            return this.UpdateOccupiedLocations();
        }

        public IEnumerable<GridLocation> Locations
        {
            get
            {
                this.VerifyInitialized();
                return this.locations;
            }
        }

        public bool MoveDown()
        {
            return this.MoveCore(1, 0);
        }

        public bool MoveLeft()
        {
            return this.MoveCore(0, -1);
        }

        public bool MoveRight()
        {
            return this.MoveCore(0, 1);
        }

        public bool RotateClockwise()
        {
            return this.RotateCore(true);
        }

        public bool RotateCounterClockwise()
        {
            return this.RotateCore(false);
        }

        protected virtual bool MoveCore(int byRows, int byColumns)
        {
            this.VerifyInitialized();

            // check whether we can perform the actual offset
            if (!this.CanOffset(byRows, byColumns))
            {
                return false;
            }

            this.Offset(byRows, byColumns);
            return true;
        }

        private bool UpdateOccupiedLocations()
        {
            this.locations = this.layout.GetLocations(this.location, this.angle);
            if (this.locations == null)
            {
                throw new InvalidOperationException("IFigureLayout.GetLocations returned unexpected value.");
            }

            // update the current row and column span
            int maxRow = 0;
            int maxCol = 0;
            int minRow = this.location.Row;
            int minCol = this.location.Column;

            for (int i = 0; i < this.locations.Length; i++)
            {
                var location = this.locations[i];
                if (location.Row > maxRow)
                {
                    maxRow = location.Row;
                }
                if (location.Row < minRow)
                {
                    minRow = location.Row;
                }

                if (location.Column > maxCol)
                {
                    maxCol = location.Column;
                }
                if (location.Column < minCol)
                {
                    minCol = location.Column;
                }
            }

            this.location = new GridLocation(minRow, minCol);
            this.rowSpan = maxRow - this.location.Row + 1;
            this.columnSpan = maxCol - this.location.Column + 1;

            return this.CanOffset(0, 0);
        }

        protected virtual bool RotateCore(bool clockWise)
        {
            // save the current state to restore it later in case needed
            // this may happen when we attempt to rotate but after the rotation the figure may not be fit within the grid's unoccupied locations
            var state = FigureState.Save(this);

            this.angle = GetNextRotationAngle(this.angle, clockWise);

            // check whether the current state is available
            if (!this.UpdateOccupiedLocations())
            {
                // restore the previous state and give control back to the game engine
                state.Restore(this);
                return false;
            }

            return true;
        }

        private static RotationAngle GetNextRotationAngle(RotationAngle current, bool clockWise)
        {
            RotationAngle newAngle = current;
            switch (current)
            {
                case RotationAngle.Deg0:
                    newAngle = clockWise ? RotationAngle.Deg270 : RotationAngle.Deg90;
                    break;
                case RotationAngle.Deg90:
                    newAngle = clockWise ? RotationAngle.Deg0 : RotationAngle.Deg180;
                    break;
                case RotationAngle.Deg180:
                    newAngle = clockWise ? RotationAngle.Deg90 : RotationAngle.Deg270;
                    break;
                case RotationAngle.Deg270:
                    newAngle = clockWise ? RotationAngle.Deg180 : RotationAngle.Deg0;
                    break;
            }

            return newAngle;
        }

        private bool CanOffset(int byRows, int byColumns)
        {
            // check row bounds
            if (this.location.Row + byRows < 0 || this.location.Row + this.rowSpan + byRows > this.grid.RowCount)
            {
                return false;
            }

            // check column bounds
            if (this.location.Column + byColumns < 0 || this.location.Column + this.columnSpan + byColumns > this.grid.ColumnCount)
            {
                return false;
            }

            // iterate each occupied location, offset it and check availability
            bool canOffset = true;
            foreach (var location in this.locations)
            {
                if (this.grid.IsOccupied(location.Row + byRows, location.Column + byColumns))
                {
                    canOffset = false;
                    break;
                }
            }

            return canOffset;
        }

        private void Offset(int byRows, int byColumns)
        {
            this.location.Offset(byRows, byColumns);

            for (int i = 0; i < this.locations.Length; i++)
            {
                this.locations[i].Offset(byRows, byColumns);
            }
        }

        private void VerifyInitialized()
        {
            if (this.grid == null)
            {
                throw new InvalidOperationException("Figure not attached to a IGameGrid instance");
            }
        }

        protected IGameGrid Grid
        {
            get
            {
                return this.grid;
            }
        }

        protected IFigureLayout Layout
        {
            get
            {
                return this.layout;
            }
        }

        public GridLocation Location
        {
            get
            {
                return this.location;
            }
        }

        private struct FigureState
        {
            public int rowSpan;
            public int columnSpan;
            public RotationAngle angle;
            public GridLocation location;
            public GridLocation[] occupiedLocations;

            public static FigureState Save(Figure figure)
            {
                var state = new FigureState();

                state.location = figure.location;
                state.occupiedLocations = figure.locations;
                state.rowSpan = figure.rowSpan;
                state.columnSpan = figure.columnSpan;
                state.angle = figure.angle;

                return state;
            }

            public void Restore(Figure figure)
            {
                figure.angle = this.angle;
                figure.columnSpan = this.columnSpan;
                figure.location = this.location;
                figure.locations = this.occupiedLocations;
                figure.rowSpan = this.rowSpan;
            }
        }
    }
}
