using System;

namespace Pentibox.Engine.Layouts
{
    internal class FigureLayout1 : IFigureLayout
    {
        /*
         * This figure looks like:
         * ----
         * Deg0:
         * [][]
         * [][]
         * []
         * ----
         * Deg90:
         * [][]
         * [][][]
         * ----
         * Deg180:
         *   []
         * [][]
         * [][]
         * ----
         * Deg270:
         * [][][]
         *   [][]
         */
        public GridLocation[] GetLocations(GridLocation location, RotationAngle angle)
        {
            var cells = new GridLocation[5];

            switch (angle)
            {
                case RotationAngle.Deg0:
                    cells[0] = new GridLocation(location.Row, location.Column);
                    cells[1] = new GridLocation(location.Row, location.Column + 1);
                    cells[2] = new GridLocation(location.Row + 1, location.Column);
                    cells[3] = new GridLocation(location.Row + 1, location.Column + 1);
                    cells[4] = new GridLocation(location.Row + 2, location.Column);
                    break;
                case RotationAngle.Deg90:
                    cells[0] = new GridLocation(location.Row, location.Column);
                    cells[1] = new GridLocation(location.Row, location.Column + 1);
                    cells[2] = new GridLocation(location.Row + 1, location.Column);
                    cells[3] = new GridLocation(location.Row + 1, location.Column + 1);
                    cells[4] = new GridLocation(location.Row + 1, location.Column + 2);
                    break;
                case RotationAngle.Deg180:
                    cells[0] = new GridLocation(location.Row, location.Column + 1);
                    cells[1] = new GridLocation(location.Row + 1, location.Column);
                    cells[2] = new GridLocation(location.Row + 1, location.Column + 1);
                    cells[3] = new GridLocation(location.Row + 2, location.Column);
                    cells[4] = new GridLocation(location.Row + 2, location.Column + 1);
                    break;
                case RotationAngle.Deg270:
                    cells[0] = new GridLocation(location.Row, location.Column);
                    cells[1] = new GridLocation(location.Row, location.Column + 1);
                    cells[2] = new GridLocation(location.Row, location.Column + 2);
                    cells[3] = new GridLocation(location.Row + 1, location.Column + 1);
                    cells[4] = new GridLocation(location.Row + 1, location.Column + 2);
                    break;
            }

            return cells;
        }

        public int InitialRowSpan
        {
            get
            {
                return 3;
            }
        }

        public int InitialColumnSpan
        {
            get
            {
                return 2;
            }
        }
    }
}
