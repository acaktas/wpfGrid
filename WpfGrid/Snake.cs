using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;

namespace WpfGrid
{
    public class Snake
    {
        private int _xSpeed;
        private int _ySpeed;
        private readonly List<Cell> _tail = new List<Cell>();
        public CellGrid Grid { get; set; }
        public Cell Current { get; set; }
        public Cell Food { get; set; }

        public Snake(CellGrid grid)
        {
            Grid = grid;
            Current = Grid.Cells[0];
            _xSpeed = 1;
            _ySpeed = 0;
            CreateNewFood();
        }

        private void CreateNewFood()
        {
            var random = new Random();
            Food = new Cell(random.Next(0, Grid.Rows - 1), random.Next(0, Grid.Columns - 1), Grid.Rows, Grid.Columns,
                Current.Grid);
            Food.Highlight(Brushes.MidnightBlue);
        }

        public void KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Up)
            {
                Direction(0, -1);
            }
            else if (e.Key == Key.Down)
            {
                Direction(0, 1);
            }
            else if (e.Key == Key.Right)
            {
                Direction(1, 0);
            }
            else if (e.Key == Key.Left)
            {
                Direction(-1, 0);
            }
        }

        private void Direction(int x, int y)
        {
            _xSpeed = x;
            _ySpeed = y;
        }

        public void Timer_Tick(DispatcherTimer timer)
        {
            Current.Highlight(Brushes.Red);
            Update(timer);
            Show();
        }

        private void Update(DispatcherTimer timer)
        {
            _tail.Add(new Cell(Current.Row, Current.Col, Grid.Rows, Grid.Columns, Current.Grid));
            _tail[0].Highlight(Brushes.Red);
            _tail.RemoveAt(0);

            Current.Col = Current.Col + _xSpeed;
            if (Current.Col < 0)
            {
                Current.Col = 0;
            }
            else if (Current.Col - 1 >= Grid.Columns - 1)
            {
                Current.Col = Grid.Columns - 1;
            }
            Current.Row = Current.Row + _ySpeed;
            if (Current.Row < 0)
            {
                Current.Row = 0;
            }
            else if (Current.Row - 1 >= Grid.Rows - 1)
            {
                Current.Row = Grid.Rows - 1;
            }

            if(IsGameOver()) timer.Stop();
            EatFood();
        }

        private bool IsGameOver()
        {
            if (_tail.Count > 3)
            {
                for (var i = 0; i <= _tail.Count - 4; i++)
                {
                    var cell = _tail[i];
                    if (cell.Row == Current.Row && cell.Col == Current.Col)
                    {
                        MessageBox.Show("Game Over", "Game Over");
                        return true;
                    }
                }
            }
            return false;
        }

        private void EatFood()
        {
            if (Current.Row == Food.Row && Current.Col == Food.Col)
            {
                _tail.Add(new Cell(Current.Row, Current.Col, Grid.Rows, Grid.Columns, Current.Grid));
                CreateNewFood();
            }
        }

        private void Show()
        {
            foreach (Cell cell in _tail)
            {
                cell.Highlight(Brushes.MistyRose);
            }
            Current.Highlight(Brushes.MistyRose);
        }
    }
}
