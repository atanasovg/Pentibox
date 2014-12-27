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

        public Game()
        {
            this.grid = new GameGrid(30, 15);
        }

        public event EventHandler<FigureChangedEventArgs> FigureChanged;

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

        public void Start()
        {
            if (this.isRunning)
            {
                return;
            }

            this.currentFigure = this.CreateFigure();
            this.OnFigureChanged(null, this.currentFigure.Locations);
            this.isRunning = true;
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
            this.DoTransaction(this.currentFigure.MoveDown, true);
        }

        public void OnRotate(bool clockWise)
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
            this.DoTransaction(method, false);
        }

        protected virtual void OnFigureChanged(IEnumerable<GridLocation> previousLocations, IEnumerable<GridLocation> currentLocations)
        {
            var eh = this.FigureChanged;
            if (eh == null)
            {
                return;
            }

            var args = new FigureChangedEventArgs(previousLocations, currentLocations);
            eh(this, args);
        }

        private void DoTransaction(Func<bool> method, bool checkLines)
        {
            var prevLocation = this.currentFigure.Locations.ToArray();
            if (method())
            {
                this.OnFigureChanged(prevLocation, this.currentFigure.Locations);
            }
            else if (checkLines)
            {
                // TODO: Check for comleted lines and start a new figure
            }
        }

        private Figure CreateFigure()
        {
            var figure = new Figure(new Layouts.FigureLayout1());
            figure.Initialize(this.grid);

            return figure;
        }

        private void VerifyRunning()
        {
            if (!this.isRunning)
            {
                throw new InvalidOperationException("Game is not running");
            }
        }
    }
}
