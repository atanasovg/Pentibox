using System;

namespace Pentibox.Engine
{
    public class LinesCompletedEventArgs : EventArgs
    {
        private int[] lines;

        internal LinesCompletedEventArgs(int[] lines)
        {
            this.lines = lines;
        }

        public int[] Lines
        {
            get
            {
                return this.lines;
            }
        }
    }
}
