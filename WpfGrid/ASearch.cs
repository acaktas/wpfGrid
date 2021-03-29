using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;

namespace WpfGrid
{
    public class ASearch
    {
        public CellGrid Grid { get; set; }

        public List<CellSpot> CellGridSpot { get; set; } = new List<CellSpot>();

        private readonly List<CellSpot> _openSet = new List<CellSpot>();
        private readonly List<CellSpot> _closedSet = new List<CellSpot>();
        private readonly CellSpot _end;
        private readonly List<CellSpot> _path = new List<CellSpot>();

        public ASearch(CellGrid grid)
        {
            Grid = grid;

            foreach (var gridCell in Grid.Cells)
            {
                var cellSpot = new CellSpot(gridCell.Row, gridCell.Col, grid.Rows, grid.Columns, gridCell.Grid) {Visited = false, Walls = gridCell.Walls};
                CellGridSpot.Add(cellSpot);
            }
            var start = CellGridSpot[0];
            _end = CellGridSpot[CellGridSpot.Count - 1];

            _openSet.Add(start);
        }

        public void Timer_Tick(DispatcherTimer timer)
        {
            if (_openSet.Count > 0)
            {
                var lowestIndex = 0;
                for (var index = 0; index < _openSet.Count; index++)
                {
                    if (_openSet[index].F < _openSet[lowestIndex].F)
                    {
                        lowestIndex = index;
                    }
                }

                var current = _openSet[lowestIndex];

                if (current == _end)
                {
                    var temp = current;
                    _path.Add(temp);
                    while (temp.Previous != null)
                    {
                        _path.Add(temp.Previous);
                        temp = temp.Previous;
                    }

                    timer.Stop();
                }

                _openSet.Remove(current);
                _closedSet.Add(current);
                var neighbours = current.GetNeighbours(CellGridSpot.ConvertAll(x => (Cell)x), false);
                foreach (var neighbour in neighbours)
                {
                    if (!_closedSet.Contains(neighbour) && CheckWall(current, neighbour))
                    {
                        var cellSpotNeighbour = (CellSpot) neighbour;
                        var tempG = cellSpotNeighbour.G = current.G + 1;
                        if (_openSet.Contains(neighbour))
                        {
                            if (tempG < cellSpotNeighbour.G)
                            {
                                cellSpotNeighbour.G = tempG;
                            }
                        }
                        else
                        {
                            cellSpotNeighbour.G = tempG;
                            _openSet.Add(cellSpotNeighbour);
                        }
                        cellSpotNeighbour.H = Heuristic(cellSpotNeighbour, _end);
                        cellSpotNeighbour.F = cellSpotNeighbour.G + cellSpotNeighbour.H;
                        cellSpotNeighbour.Previous = current;
                    }
                }
            }
            else
            {
                timer.Stop();
            }

            //foreach (var openCell in openSet)
            //{
            //    openCell.Highlight(Brushes.Green);
            //}

            foreach (var closeCell in _closedSet)
            {
                closeCell.Highlight(Brushes.Red);
            }

            foreach (var cellSpot in _path)
            {
                cellSpot.Highlight(Brushes.DarkGoldenrod);
            }
        }

        private static bool CheckWall(Cell current, Cell next)
        {
            var x = current.Row - next.Row;
            if (x == 1)
                return current.Walls[1] == 0 && next.Walls[3] == 0;
            if (x == -1)
            {
                return current.Walls[3] == 0 && next.Walls[1] == 0;
            }

            var y = current.Col - next.Col;
            if (y == 1)
            {
                return current.Walls[0] == 0 && next.Walls[2] == 0;
            }
            if (y == -1)
            {
                return current.Walls[2] == 0 && next.Walls[0] == 0;
            }
            return true;
        }

        private static int Heuristic(Cell neighbour, Cell cellSpot)
        {
            return Math.Abs(neighbour.Col - cellSpot.Col) + Math.Abs(neighbour.Row - cellSpot.Row);
        }
    }

    public class CellSpot : Cell
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int F { get; set; }
        public int G { get; set; }
        public int H { get; set; }
        public CellSpot Previous { get; set; }

        public CellSpot(int row, int col, int rows, int cols, Grid grid) : base(row, col, rows, cols, grid)
        {
            X = col;
            Y = row;
        }
    }
}
