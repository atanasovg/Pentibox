using System;
using System.Collections.Generic;
using System.Linq;

namespace Pentibox.Engine
{
    public class Game
    {
        private IGameGrid grid;
        private Figure currentFigure;
        private bool isRunning;
        private bool isFinished;

        public Game()
        {
            this.grid = new GameGrid(20, 8);
        }

        public event EventHandler<FigureChangedEventArgs> FigureChanged;

        public event EventHandler<LinesCompletedEventArgs> LinesCompleted;

        public event EventHandler<EventArgs> Finished;

        public IGameGrid Grid
        {
            get
            {
                return this.grid;
            }
        }

        public bool IsRunning
        {
            get
            {
                return this.isRunning;
            }
        }

        public bool IsFinished
        {
            get
            {
                return this.isFinished;
            }
        }

        public void Start()
        {
            if (this.isRunning)
            {
                return;
            }

            this.isRunning = true;
            this.isFinished = false;
        }

        public void Stop()
        {
            if (!this.isRunning)
            {
                return;
            }
        }

        public void OnTick()
        {
            this.VerifyRunning();

            if (this.currentFigure == null)
            {
                this.CreateFigure();
            }
            else
            {
                var success = this.DoMove(this.currentFigure.MoveDown);

                if (!success)
                {
                    this.CheckLines();
                    this.CreateFigure();
                }
            }
        }

        public void MoveDown()
        {
            this.VerifyRunning();
            this.DoMove(this.currentFigure.MoveDown);
        }

        public void Rotate(bool clockWise)
        {
            this.VerifyRunning();

            Func<bool> method;
            if (clockWise)
            {
                method = this.currentFigure.RotateClockwise;
            }
            else
            {
                method = this.currentFigure.RotateCounterClockwise;
            }
            this.DoMove(method);
        }

        public void MoveLeft()
        {
            this.VerifyRunning();
            this.DoMove(this.currentFigure.MoveLeft);
        }

        public void MoveRight()
        {
            this.VerifyRunning();
            this.DoMove(this.currentFigure.MoveRight);
        }

        protected virtual void OnFigureChanged(FigureChangedEventArgs e)
        {
            var eh = this.FigureChanged;
            if (eh != null)
            {
                eh(this, e);
            }
        }

        protected virtual void OnFinished(EventArgs e)
        {
            var eh = this.Finished;
            if (eh != null)
            {
                eh(this, e);
            }
        }

        protected virtual void OnLinesCompleted(LinesCompletedEventArgs e)
        {
            var eh = this.LinesCompleted;
            if (eh != null)
            {
                eh(this, e);
            }
        }

        private bool DoMove(Func<bool> method)
        {
            var prevLocations = this.currentFigure.Locations.ToArray();

            // clear the current locations to enable valid new locations check
            this.grid.Update(prevLocations, false);

            // call the move method
            bool success = method();

            // occupy the current locations again
            this.grid.Update(this.currentFigure.Locations, true);

            if (success)
            {
                var args = new FigureChangedEventArgs(prevLocations, this.currentFigure.Locations);
                this.OnFigureChanged(args);
            }

            return success;
        }

        private void DoFinish()
        {
            this.isRunning = false;
            this.isFinished = true;
            this.grid.Reset();

            this.OnFinished(EventArgs.Empty);
        }

        private void CreateFigure()
        {
            this.currentFigure = new Figure(new Layouts.FigureLayout1());
            if (!this.currentFigure.Initialize(this.grid))
            {
                this.DoFinish();
            }
            else
            {
                this.OnFigureChanged(new FigureChangedEventArgs(null, this.currentFigure.Locations));
            }
        }

        private void VerifyRunning()
        {
            if (!this.isRunning)
            {
                throw new InvalidOperationException("Game is not running");
            }
        }

        private void CheckLines()
        {
            List<int> completedLines = new List<int>();

            for (int row = 0; row < this.grid.RowCount; row++)
            {
                var hasLine = true;
                for (int column = 0; column < this.grid.ColumnCount; column++)
                {
                    if (!this.grid.IsOccupied(row, column))
                    {
                        hasLine = false;
                        break;
                    }
                }

                if (hasLine)
                {
                    completedLines.Add(row);
                }
            }

            if (completedLines.Count > 0)
            {
                // TODO: Clear the line and scroll down other occupied locations
                this.OnLinesCompleted(new LinesCompletedEventArgs(completedLines.ToArray()));
            }
        }
    }
}
