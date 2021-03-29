using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace WpfGrid
{
    public class Cell
    {
        public int Row { get; set; }
        public int Col { get; set; }
        private readonly int _cols;
        private readonly int _rows;
        public Grid Grid { get; }
        private int[] _walls;
        public int[] Walls
        {
            get => _walls ?? (_walls = new[] { 1, 1, 1, 1 });
            set
            {
                _walls = value;
                RedrawWalls();
            }
        }

        private bool _visited;
        public bool Visited
        {
            get => _visited;
            set
            {
                if (value)
                {
                    Highlight(Brushes.CadetBlue);
                }
                else
                {
                    Highlight(Brushes.White);
                }
                _visited = value;
            }
        }

        public bool Live { get; set; }

        public Cell(int row, int col, int rows, int cols, Grid grid)
        {
            Row = row;
            Col = col;
            _rows = rows;
            _cols = cols;
            Grid = grid;
        }

        private int Index(int row, int col)
        {
            if (row < 0 || col < 0 || col > _cols - 1 || row > _rows - 1)
            {
                return -1;
            }
            return row + col * _cols;
        }
        
        public Cell CheckNeighbours(List<Cell> cells, bool diagonals)
        {
            var neighbours = GetNeighbours(cells, diagonals);

            if (neighbours.Count <= 0) return null;

            var rnd = new Random();
            var index = rnd.Next(0, neighbours.Count);

            return neighbours[index];
        }

        public List<Cell> GetNeighbours(List<Cell> cells, bool diagonals)
        {
            var neighbours = new List<Cell>();

            var top = GetNeighbour(cells, Col, Row - 1);
            var right = GetNeighbour(cells, Col + 1, Row);
            var left = GetNeighbour(cells, Col - 1, Row);
            var bottom = GetNeighbour(cells, Col, Row + 1);


            if (top != null && !top.Visited)
            {
                neighbours.Add(top);
            }

            if (right != null && !right.Visited)
            {
                neighbours.Add(right);
            }

            if (bottom != null && !bottom.Visited)
            {
                neighbours.Add(bottom);
            }

            if (left != null && !left.Visited)
            {
                neighbours.Add(left);
            }

            if (!diagonals) return neighbours;

            var topLeft = GetNeighbour(cells, Col - 1, Row - 1);
            var topRight = GetNeighbour(cells, Col + 1, Row - 1);
            var bottomLeft = GetNeighbour(cells, Col - 1, Row + 1);
            var bottomRight = GetNeighbour(cells, Col + 1, Row + 1);


            if (topLeft != null && !topLeft.Visited)
            {
                neighbours.Add(topLeft);
            }

            if (topRight != null && !topRight.Visited)
            {
                neighbours.Add(topRight);
            }

            if (bottomLeft != null && !bottomLeft.Visited)
            {
                neighbours.Add(bottomLeft);
            }

            if (bottomRight != null && !bottomRight.Visited)
            {
                neighbours.Add(bottomRight);
            }

            return neighbours;
        }

        public List<Cell> GetLiveNeighbours(List<Cell> cells, bool diagonals)
        {
            var neighbours = new List<Cell>();

            var top = GetNeighbour(cells, Col, Row - 1);
            var right = GetNeighbour(cells, Col + 1, Row);
            var left = GetNeighbour(cells, Col - 1, Row);
            var bottom = GetNeighbour(cells, Col, Row + 1);


            if (top != null && top.Live)
            {
                neighbours.Add(top);
            }

            if (right != null && right.Live)
            {
                neighbours.Add(right);
            }

            if (bottom != null && bottom.Live)
            {
                neighbours.Add(bottom);
            }

            if (left != null && left.Live)
            {
                neighbours.Add(left);
            }

            if (!diagonals) return neighbours;

            var topLeft = GetNeighbour(cells, Col - 1, Row - 1);
            var topRight = GetNeighbour(cells, Col + 1, Row - 1);
            var bottomLeft = GetNeighbour(cells, Col - 1, Row + 1);
            var bottomRight = GetNeighbour(cells, Col + 1, Row + 1);


            if (topLeft != null && topLeft.Live)
            {
                neighbours.Add(topLeft);
            }

            if (topRight != null && topRight.Live)
            {
                neighbours.Add(topRight);
            }

            if (bottomLeft != null && bottomLeft.Live)
            {
                neighbours.Add(bottomLeft);
            }

            if (bottomRight != null && bottomRight.Live)
            {
                neighbours.Add(bottomRight);
            }

            return neighbours;
        }

        private Cell GetNeighbour(IReadOnlyList<Cell> cells, int row, int col)
        {
            Cell cell = null;
            var index = Index(row, col);
            if (index >= 0 && index < cells.Count)
            {
                cell = cells[index];
            }
            return cell;
        }

        public void Show()
        {
            var dockPanel = new DockPanel
            {
                Width = 30,
                Height = 30
            };
            Grid.Children.Add(dockPanel);
            Grid.SetColumn(dockPanel, Col);
            Grid.SetRow(dockPanel, Row);

            var border = new Border
            {
                BorderThickness = new Thickness(Walls[0], Walls[1], Walls[2], Walls[3]),
                BorderBrush = new SolidColorBrush(Colors.Black)
            };
            Grid.Children.Add(border);
            Grid.SetColumn(border, Col);
            Grid.SetRow(border, Row);
        }

        public void RedrawWalls()
        {
            var element = (Border)Grid.Children.Cast<UIElement>().Last(e => Grid.GetRow(e) == Row && Grid.GetColumn(e) == Col);
            element.BorderThickness = new Thickness(Walls[0], Walls[1], Walls[2], Walls[3]);
        }

        public void Highlight(Brush brush)
        {
            var element = (DockPanel)Grid.Children.Cast<UIElement>().First(e => Grid.GetRow(e) == Row && Grid.GetColumn(e) == Col);
            element.Background = brush;
        }
    }
}