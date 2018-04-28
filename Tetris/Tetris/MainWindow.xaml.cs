using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
namespace Tetris
{

    public class Board
    {
        private int Rows;
        private int Cols;
        private int Score;
        private int LinesFilled;
        private Tetramino currTetramino;
        private Label[,] BlockControls;
        static private Brush NoBrush=Brushes.Transparent;
        static private Brush SilverBrush = Brushes.Gray;

        public Board(Grid TetrisGrid)
        {
            Rows = TetrisGrid.RowDefinitions.Count;
            Cols = TetrisGrid.ColumnDefinitions.Count;
            Score = 0;
            LinesFilled = 0;

            BlockControls = new Label[Cols, Rows];
            for(int i=0; i <Cols; i++)
            {
                for (int j=0; j<Rows;j++)
                {
                    BlockControls[i, j] = new Label
                    {
                        Background=NoBrush,
                        BorderBrush = SilverBrush,
                        BorderThickness = new Thickness(1, 1, 1, 1)
                };
                    //BlockControls[i, j].Background = NoBrush;
                    //BlockControls[i, j].BorderBrush = SilverBrush;
                    //BlockControls[i, j].BorderThickness = new Thickness(1,1,1,1);
                    Grid.SetRow(BlockControls[i, j], j);
                    Grid.SetColumn(BlockControls[i, j], i);
                    TetrisGrid.Children.Add(BlockControls[i, j]);
                }
            }
            currTetramino = new Tetramino();
            CurrTetraminoDraw();
        }
        public int GetScore()
        {
            return Score;
        }
        public int GetLines()
        {
            return LinesFilled;
        }

        private void CurrTetraminoDraw()
        {
            Point position = currTetramino.GetCurrPosition();
            Point[] shape = currTetramino.GetCurrShape();
            Brush color = currTetramino.GetCurrColor();
            foreach (Point s in shape)
            {
                BlockControls[(int)(s.X + position.X) + ((Cols / 2) - 1), (int)(s.Y + position.Y) + 2].Background = color;
            }

        }
        private void CurrTetraminoErase()
        {
            Point position = currTetramino.GetCurrPosition();
            Point[] shape = currTetramino.GetCurrShape();
            foreach (Point s in shape)
            {
                BlockControls[(int)(s.X + position.X) + ((Cols / 2) - 1), (int)(s.Y + position.Y) + 2].Background = NoBrush;
            }
        }

        private void CheckRows()
        {
            bool full;
            for(int i=Rows-1; i > 0; i--)
            {
                full = true;
                for (int j=0;j<Cols ;j++)
                {
                    if (BlockControls[j,i].Background==NoBrush)
                    {
                        full = false;
                    }
                }
                if (full)
                {
                    RemoveRow(i);
                    Score += 100;
                    LinesFilled += 1;
                }
            }
            

        }

        private void RemoveRow(int row)
        {
            for (int i=row; i>2; i--)
            {
                for (int j = 0; j <Cols; j++)
                {
                    BlockControls[j, i].Background = BlockControls[j, i-1].Background;
                }
            }
        }

        public void CurrTetraminoMovLeft()
        {
            Point Position = currTetramino.GetCurrPosition();
            Point[] Shape = currTetramino.GetCurrShape();
            bool move = true;
            CurrTetraminoErase();
            foreach (Point s in Shape)
            {
                if (((int)(s.X + Position.X) + ((Cols / 2) - 1) - 1) < 0)
                {
                    move = false;
                }
                else if (BlockControls[((int)(s.X + Position.X) + ((Cols / 2) - 1) - 1),
                    (int)(s.Y + Position.Y)+2].Background!=NoBrush)
                {
                    move = false;
                }
            }
            if (move)
            {
                currTetramino.MovLeft();
                CurrTetraminoDraw();

            }
            else
            {
                CurrTetraminoDraw();
            }
        }
        public void CurrTetraminoMovRight()
        {
            Point Position = currTetramino.GetCurrPosition();
            Point[] Shape = currTetramino.GetCurrShape();
            bool move = true;
            CurrTetraminoErase();
            foreach (Point s in Shape)
            {
                if (((int)(s.X + Position.X) + ((Cols / 2) - 1) + 1) >= Cols)
                {
                    move = false;
                }
                else if (BlockControls[((int)(s.X + Position.X) + ((Cols / 2) - 1) + 1),
                    (int)(s.Y + Position.Y) + 2].Background != NoBrush)
                {
                    move = false;
                }
            }
            if (move)
            {
                currTetramino.MovRight();
                CurrTetraminoDraw();

            }
            else
            {
                CurrTetraminoDraw();
            }
        }
        public void CurrTetraminoMovDown()
        {
            Point Position = currTetramino.GetCurrPosition();
            Point[] Shape = currTetramino.GetCurrShape();
            bool move = true;
            CurrTetraminoErase();

            foreach (Point s in Shape)
            {
                if (((int)(s.Y + Position.Y) + 2 + 1) >= Rows)
                {
                    move = false;
                }
                else if (BlockControls[((int)(s.X + Position.X) + ((Cols / 2) - 1)),
                    (int)(s.Y + Position.Y) + 2 + 1].Background != NoBrush)
                {
                    move = false;
                }
            }
            if (move)
            {
                currTetramino.MovDown();
                CurrTetraminoDraw();

            }
            else
            {
                CurrTetraminoDraw();
                CheckRows();
                currTetramino = new Tetramino();
            }
        }
        public void CurrTetraminoMovRotate()
        {
            Point Position = currTetramino.GetCurrPosition();
            Point[] S = new Point[4];
            Point[] Shape = currTetramino.GetCurrShape();
            bool move = true;

            Shape.CopyTo(S,0);
            CurrTetraminoErase();
            for(int i=0; i < S.Length; i++)
            {
                double x = S[i].X;
                S[i].X = S[i].Y * -1;
                S[i].Y = x;
                if (((int)((S[i].Y+Position.Y)+2))>=Rows)
                {
                    move = false;
                }
                else if (((int)(S[i].X+Position.X)+((Cols/2)-1))<0)
                {
                    move = false;
                }
                else if (((int)(S[i].X + Position.X) + ((Cols / 2) - 1)) > Rows)
                {
                    move = false;
                }
                else if (BlockControls[((int)(S[i].X + Position.X) + ((Cols / 2) - 1)),(int)(S[i].Y+Position.Y)+2].Background!=NoBrush)
                {
                    move = false;
                }
            }
            if (move)
            {
                currTetramino.MovRotate();
                CurrTetraminoDraw();
            }
            else
            {
                CurrTetraminoDraw();
            }
        }

    }

    public class Tetramino
    {
        private Point currPosition;
        private Point[] currShape;
        private Brush currColor;
        private bool rotate;
        public Tetramino()
        {
            currPosition = new Point(0, 0);
            currColor = Brushes.Transparent;
            currShape = SetRandomShape();
        }

        public Brush GetCurrColor()
        {
            return currColor;
        }

        public Point GetCurrPosition()
        {
            return currPosition;
        }

        public Point[] GetCurrShape()
        {
            return currShape;
        }

        public void MovLeft()
        {
            currPosition.X -= 1;
        }
        public void MovRight()
        {
            currPosition.X += 1;
        }
        
        public void MovDown()
        {
            currPosition.Y += 1;
        }
        public void MovRotate()
        {
            if (rotate)
            {
                for(int i=0; i <currShape.Length; i++)
                {
                    double x = currShape[i].X;
                    currShape[i].X = (currShape[i].Y) * (-1);
                    currShape[i].Y = x;
                }
            }
        }

        private Point[] SetRandomShape()
        {
            Random ran = new Random();
            switch (ran.Next() % 7)
            {
                case 0:
                    rotate = true;
                    currColor = Brushes.Cyan;
                    return new Point[] {
                        new Point(0,0),
                        new Point(-1,0),
                        new Point(1,0),
                        new Point(2,0)
                    };
                case 1:
                    rotate = true;
                    currColor = Brushes.Blue;
                    return new Point[] {
                        new Point(1,-1),
                        new Point(-1,0),
                        new Point(0,0),
                        new Point(1,0)
                    };
                case 2:
                    rotate = true;
                    currColor = Brushes.Orange;
                    return new Point[] {
                        new Point(0,0),
                        new Point(-1,0),
                        new Point(1,0),
                        new Point(1,-1)
                    };
                case 3:
                    rotate = false;
                    currColor = Brushes.Yellow;
                    return new Point[] {
                        new Point(0,0),
                        new Point(0,1),
                        new Point(1,0),
                        new Point(1,1)
                    };
                case 4:
                    rotate = true;
                    currColor = Brushes.Green;
                    return new Point[] {
                        new Point(0,0),
                        new Point(-1,0),
                        new Point(0,-1),
                        new Point(1,0)
                    };
                case 5:
                    rotate = true;
                    currColor = Brushes.Purple;
                    return new Point[] {
                        new Point(0,0),
                        new Point(-1,0),
                        new Point(0,-1),
                        new Point(1,0)
                    };
                case 6:
                    rotate = true;
                    currColor = Brushes.Red;
                    return new Point[] {
                        new Point(0,0),
                        new Point(-1,0),
                        new Point(0,1),
                        new Point(1,0)
                    };
                default:
                    return null;
            }
        }
    }
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        DispatcherTimer Timer;
        Board myBoard;
        public MainWindow()
        {

            InitializeComponent();
        }
        void MainWindow_Initilized(object sender,EventArgs e)
        {
            Timer =new DispatcherTimer();
            Timer.Tick +=new EventHandler(Game_Tick);
            Timer.Interval = new TimeSpan(0, 0, 0, 0, 400);
            GameStart();
        }

        private void GameStart()
        {
            MainGrid.Children.Clear();
            myBoard = new Board(MainGrid);
            Timer.Start();
        }

        private void Game_Tick(object sender, EventArgs e)
        {
            Scores.Content = myBoard.GetScore().ToString();
            Lines.Content = myBoard.GetLines().ToString();
            myBoard.CurrTetraminoMovDown();
        }
        private void HandleKeyDown(object sender,KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Left:
                    if (Timer.IsEnabled) myBoard.CurrTetraminoMovLeft();
                    break;
                case Key.Right:
                    if (Timer.IsEnabled) myBoard.CurrTetraminoMovRight();
                    break;
                case Key.Down:
                    if (Timer.IsEnabled) myBoard.CurrTetraminoMovDown();
                    break;
                case Key.Up:
                    if (Timer.IsEnabled) myBoard.CurrTetraminoMovRotate();
                    break;
                case Key.F2:
                    GameStart();
                    break;
                case Key.F3:
                    GamePause();
                    break;
                default:
                    break;

            }
        }

        private void GamePause()
        {
            if (Timer.IsEnabled)
            {
                Timer.Stop();
            }
            else
            {
                Timer.Start();
            }
        }
    }
}
