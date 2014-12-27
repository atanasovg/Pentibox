using System;
using System.Collections.Generic;

namespace Pentibox.Engine
{
    public class FigureChangedEventArgs : EventArgs
    {
        private IEnumerable<GridLocation> previousLocations;
        private IEnumerable<GridLocation> currentLocations;

        internal FigureChangedEventArgs(IEnumerable<GridLocation> prevLocations, IEnumerable<GridLocation> currLocations)
        {
            this.previousLocations = prevLocations;
            this.currentLocations = currLocations;
        }

        public IEnumerable<GridLocation> EmptyLocations
        {
            get 
            {
                if (this.previousLocations == null)
                {
                    yield break;
                }

                // these two loops make a m * n complexity which generally is an expensive one and in theory may lead to performance issues
                // still, assuming the locations are actually of constant length - 5 in the Penta-box scenario - the complexity is constant - O(5 * 5)
                foreach (var prevLoc in this.previousLocations)
                {
                    bool found = false;
                    foreach (var currLoc in this.currentLocations)
                    {
                        if (currLoc == prevLoc)
                        {
                            found = true;
                            break;
                        }
                    }

                    if (!found)
                    {
                        yield return prevLoc;
                    }
                }
            }
        }

        public IEnumerable<GridLocation> FilledLocations
        {
            get
            {
                foreach (var currLoc in this.currentLocations)
                {
                    bool found = false;

                    // TODO: Do not check for null upon each iteration
                    if (this.previousLocations != null)
                    {
                        foreach (var prevLoc in this.previousLocations)
                        {
                            if (prevLoc == currLoc)
                            {
                                found = true;
                                break;
                            }
                        }
                    }

                    if (!found)
                    {
                        yield return currLoc;
                    }
                }
            }
        }
    }
}
