using System;

namespace Pentibox.Engine
{
    public interface IFigureLayout
    {
        /// <summary>
        /// Gets the array with all the locations occupied for the provided rotation angle.
        /// </summary>
        /// <param name="location">The top-left location of the current box.</param>
        /// <param name="angle">The current rotation angle.</param>
        /// <returns></returns>
        GridLocation[] GetLocations(GridLocation location, RotationAngle angle);

        int InitialRowSpan
        {
            get;
        }

        int InitialColumnSpan
        {
            get;
        }
    }
}
