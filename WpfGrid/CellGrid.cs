using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace WpfGrid
{
    public class CellGrid
    {
        public List<Cell> Cells { get; set; } = new List<Cell>();

        public int Columns { get; set; }

        public int Rows { get; set; }

        public Grid CreateGrid(int rows, int columns, int height)
        {
            Rows = rows;
            Columns = columns;

            var grid = new Grid();

            var isColumnsAdded = false;

            for (var row = 0; row < rows; row++)
            {
                grid.RowDefinitions.Add(
                    new RowDefinition
                    {
                        Height = new GridLength(height)
                    });

                for (var col = 0; col < columns; col++)
                {
                    if (!isColumnsAdded)
                    {
                        grid.ColumnDefinitions.Add(
                            new ColumnDefinition
                            {
                                Width = new GridLength(height)
                            });
                    }

                    var cell = new Cell(row, col, rows, columns, grid);
                    cell.Show();

                    Cells.Add(cell);

                    //var textBox = new TextBox
                    //{
                    //    Width = height,
                    //    Height = height,
                    //    IsEnabled = false,
                    //    Text = row + " - " + col,
                    //    HorizontalContentAlignment = HorizontalAlignment.Center,
                    //    VerticalContentAlignment = VerticalAlignment.Center
                    //};
                    //Grid.Children.Add(textBox);
                    //Grid.SetColumn(textBox, col);
                    //Grid.SetRow(textBox, row);
                }

                isColumnsAdded = true;
            }
            return grid;
        }
    }
}