using System;
using System.Collections;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace WpfGrid
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private DispatcherTimer _timer;
        private Maze _maze;
        private ASearch _search;
        private CellGrid _cellGrid;
        private LifeGame _lifeGame;
        private Snake _snakeGame;

        public MainWindow()
        {
            InitializeComponent();
            DrawGrid();
        }

        private void Maze_OnClick(object sender, RoutedEventArgs e)
        {
            _maze = new Maze(_cellGrid);
            _maze.Current = _maze.Grid.Cells[0];
            _timer = new DispatcherTimer { Interval = new TimeSpan(100000) };
            _timer.Tick += MazeTimer_Tick;
            _timer.Start();
        }

        private void MazeTimer_Tick(object sender, EventArgs e)
        {
            _maze.Timer_Tick(_timer);
        }

        private void Search_OnClick(object sender, RoutedEventArgs e)
        {
            if (_maze == null) return;

            _search = new ASearch(_maze.Grid);
            _timer = new DispatcherTimer { Interval = new TimeSpan(100000) };
            _timer.Tick += SearchTimer_Tick;
            _timer.Start();
        }

        private void SearchTimer_Tick(object sender, EventArgs e)
        {
            _search.Timer_Tick(_timer);
        }

        private void LifeGame_OnClick(object sender, RoutedEventArgs e)
        {
            _lifeGame = new LifeGame(_cellGrid);
            _timer = new DispatcherTimer { Interval = new TimeSpan(100000) };
            _timer.Tick += LifeGameTimer_Tick;
            _timer.Start();
        }

        private void LifeGameTimer_Tick(object sender, EventArgs e)
        {
            _lifeGame.Timer_Tick(_timer);
        }

        private void SnakeGame_OnClick(object sender, RoutedEventArgs e)
        {
            _snakeGame = new Snake(_cellGrid);
            this.KeyDown += _snakeGame.KeyDown;
            _timer = new DispatcherTimer { Interval = new TimeSpan(1000000) };
            _timer.Tick += SnakeGameTimer_Tick;
            _timer.Start();
        }

        private void SnakeGameTimer_Tick(object sender, EventArgs e)
        {
            _snakeGame.Timer_Tick(_timer);
        }

        private void Restart_OnClick(object sender, RoutedEventArgs e)
        {
            if (_timer != null)
            {
                _timer.Stop();
                _timer.Tick -= MazeTimer_Tick;
                _timer.Tick -= SearchTimer_Tick;
                _timer.Tick -= LifeGameTimer_Tick;
            }
            DrawGrid();
        }

        private void DrawGrid()
        {
            if (Grid.Children.Count > 1)
            {
                Grid.Children.RemoveAt(1);
            }

            _cellGrid = new CellGrid();

            var grid = _cellGrid.CreateGrid(20, 20, 30);
            Grid.Children.Add(grid);
            Grid.SetRow(grid, 0);
        }
    }
}
