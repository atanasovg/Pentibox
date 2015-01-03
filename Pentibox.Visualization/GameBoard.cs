using Pentibox.Engine;
using System;
using System.Collections.Generic;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;

namespace Pentibox.Visualization
{
    public class GameBoard : Canvas
    {
        public static DependencyProperty GridLocationProperty = 
            DependencyProperty.RegisterAttached("GridLocation", typeof(GridLocation), typeof(GameBoard), null);

        private int gridCellLength;
        private Game game;
        private Brush fillBrush;
        private Brush strokeBrush;

        public GameBoard()
        {
            this.gridCellLength = 50;

            this.fillBrush = new SolidColorBrush(Colors.Red);
            this.strokeBrush = new SolidColorBrush(Colors.Black);
        }

        public Game Game
        {
            get
            {
                return this.game;
            }
        }

        public void Attach(Game game)
        {
            if (game == null)
            {
                throw new ArgumentNullException("game");
            }

            if (this.game == game)
            {
                return;
            }

            this.Detach();

            this.game = game;
            this.game.FigureChanged += this.OnFigureChanged;
            this.game.LinesCompleted += this.OnLinesCompleted;
            this.Width = this.game.Grid.ColumnCount * this.gridCellLength;
            this.Height = this.game.Grid.RowCount * this.gridCellLength;

            this.InitializeRectangles();
        }

        public void Detach()
        {
            if (this.game == null)
            {
                return;
            }

            this.game.FigureChanged -= this.OnFigureChanged;
            this.game.LinesCompleted -= this.OnLinesCompleted;
            this.Children.Clear();
        }

        private void InitializeRectangles()
        {
            var grid = this.game.Grid;
            Rectangle rect;

            for (int row = 0; row < grid.RowCount; row++)
            {
                for (int column = 0; column < grid.ColumnCount; column++)
                {
                    rect = new Rectangle();
                    rect.SetValue(GridLocationProperty, new GridLocation(row, column));

                    // position the rectangle on the surface
                    rect.Width = this.gridCellLength;
                    rect.Height = this.gridCellLength;
                    Canvas.SetLeft(rect, column * this.gridCellLength);
                    Canvas.SetTop(rect, row * this.gridCellLength);

                    this.Children.Add(rect);
                }
            }
        }

        private void OnFigureChanged(object sender, FigureChangedEventArgs e)
        {
            var grid = this.game.Grid;
            foreach (var location in e.EmptyLocations)
            {
                var index = location.Row * grid.ColumnCount + location.Column;
                var rect = (Rectangle)this.Children[index];
                rect.Fill = null;
                rect.Stroke = null;
            }

            foreach (var location in e.FilledLocations)
            {
                var index = location.Row * grid.ColumnCount + location.Column;
                var rect = (Rectangle)this.Children[index];
                rect.Fill = this.fillBrush;
                rect.Stroke = this.strokeBrush;
            }
        }

        private void OnLinesCompleted(object sender, LinesCompletedEventArgs e)
        {
            var lines = e.Lines;
            foreach (var rowIndex in lines)
            {
                var startIndex = rowIndex * this.game.Grid.ColumnCount;
                for (int column = 0; column < this.game.Grid.ColumnCount; column++)
                {
                    var rect = (Rectangle)this.Children[startIndex + column];
                    rect.Fill = null;
                    rect.Stroke = null;
                }
            }
        }
    }
}
