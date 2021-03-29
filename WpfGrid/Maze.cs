using System.Collections;
using System.Windows.Media;
using System.Windows.Threading;

namespace WpfGrid
{
    public class Maze
    {
        private readonly Stack _stackCells = new Stack();
        public Cell Current { get; set; }
        public CellGrid Grid { get; set; }

        public Maze(CellGrid grid)
        {
            Grid = grid;
        }

        public void Timer_Tick(DispatcherTimer timer)
        {
            if (Current == null) return;

            Current.Visited = true;
            var next = Current.CheckNeighbours(Grid.Cells, false);
            if (next != null)
            {
                next.Visited = true;
                _stackCells.Push(Current);
                RemoveWalls(Current, next);
                Current = next;
            }
            else if (_stackCells.Count > 0)
            {
                Current = (Cell)_stackCells.Pop();
            }
            else if (Current.Row == 0 && Current.Col == 0)
            {
                timer.Stop();
            }

            Current.Highlight(Brushes.Blue);
        }
        private static void RemoveWalls(Cell current, Cell next)
        {
            var x = current.Row - next.Row;
            if (x == 1)
            {
                current.Walls[1] = 0;
                next.Walls[3] = 0;
            }
            else if (x == -1)
            {
                current.Walls[3] = 0;
                next.Walls[1] = 0;
            }

            var y = current.Col - next.Col;
            if (y == 1)
            {
                current.Walls[0] = 0;
                next.Walls[2] = 0;
            }
            else if (y == -1)
            {
                current.Walls[2] = 0;
                next.Walls[0] = 0;
            }

            current.RedrawWalls();
            next.RedrawWalls();
        }
    }
}
