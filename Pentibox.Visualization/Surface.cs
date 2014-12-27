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
    public class Surface : Canvas
    {
        public static DependencyProperty GridLocationProperty = DependencyProperty.RegisterAttached("GridLocation", typeof(GridLocation), typeof(Surface), null);

        private int gridCellLength;
        private Game game;
        private Brush fillBrush;
        private Brush strokeBrush;

        public Surface()
        {
            this.game = new Game();
            this.game.FigureChanged += this.OnFigureChanged;
            this.gridCellLength = 30;

            this.Width = this.game.Grid.ColumnCount * this.gridCellLength;
            this.Height = this.game.Grid.RowCount * this.gridCellLength;

            this.fillBrush = new SolidColorBrush(Colors.Red);
            this.strokeBrush = new SolidColorBrush(Colors.Black);

            this.InitializeRectangles();
        }

        public Game Game
        {
            get
            {
                return this.game;
            }
        }

        private void InitializeRectangles()
        {
            var grid = this.game.Grid;
            Rectangle rect;

            for (int i = 0; i < grid.RowCount; i++)
            {
                for (int j = 0; j < grid.ColumnCount; j++)
                {
                    rect = new Rectangle();

                    // position the rectangle on the surface
                    rect.Width = this.gridCellLength;
                    rect.Height = this.gridCellLength;
                    Canvas.SetLeft(rect, j * this.gridCellLength);
                    Canvas.SetTop(rect, i * this.gridCellLength);

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
    }
}
