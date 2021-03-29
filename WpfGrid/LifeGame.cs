using System;
using System.Collections.Generic;
using System.Windows.Threading;

namespace WpfGrid
{
    internal class LifeGame
    {
        public CellGrid Grid { get; set; }

        public LifeGame(CellGrid grid)
        {
            Grid = grid;
            var random = new Random();
            foreach (var gridCell in Grid.Cells)
            {
                gridCell.Live = Convert.ToBoolean(random.Next(2));
            }
        }

        public void Timer_Tick(DispatcherTimer timer)
        {
            foreach (var gridCell in Grid.Cells)
            {
                gridCell.Visited = gridCell.Live;
            }

            var next = new List<Cell>();

            foreach (var gridCell in Grid.Cells)
            {
                var neighboursCount = gridCell.GetLiveNeighbours(Grid.Cells, true).Count;

                if (!gridCell.Live && neighboursCount == 3)
                {
                    gridCell.Live = true;
                }
                else if (gridCell.Live && (neighboursCount < 2 || neighboursCount > 3))
                {
                    gridCell.Live = false;
                }

                next.Add(gridCell);
            }

            Grid.Cells = next;
        }
    }
}