using System.Collections.ObjectModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Caro_game
{
    public enum Player
    {
        PlayerX, PlayerO, None
    }

    public enum ControlMode
    {
        Mouse, Keyboard
    }

    public class BoardCell
    {
        public int x;
        public int y;
        public Player player;
        public BoardCell(int x, int y, Player type)
        {
            this.x = x;
            this.y = y;
            this.player = type;
        }
    }

    public partial class GamePlayWindow : Window
    {
        const int STROKE_THICKNESS = 1;
        int size = 12;
        int numToWin = 3;
        int boardBorderThickness = 3;

        BoardCell[,] BoardCellMap = null;
        int cellLeft;
        double cellWidth;
        Player playerTurn;
        bool[,] boardbool;

        Shape seletor;
        int selectorX;
        int selectorY;
        SolidColorBrush selectorColor = Brushes.LightGray;

        ControlMode control = ControlMode.Mouse;

        public GamePlayWindow(/*int size*/)
        {
            /*this.size = size;*/
            InitializeComponent();
            TopSpace.Loaded += TopSpace_Loaded;
            TopSpace.SizeChanged += TopSpace_SizeChanged;
        }

        private void TopSpace_Loaded(object sender, RoutedEventArgs e)
        {
            Board.Width = 10;
            Board.Height = 10;
            double boardSize = Math.Min(TopSpace.ActualHeight, TopSpace.ActualWidth);
            Board.Width = boardSize - boardBorderThickness * 2;
            Board.Height = boardSize - boardBorderThickness * 2;
            BoardBorder.Width = boardSize;
            BoardBorder.Height = boardSize;

            cellWidth = Board.ActualHeight / size;
            RestartGame();
        }

        private void TopSpace_SizeChanged(object sender, RoutedEventArgs e)
        {
            double boardSize = Math.Min(TopSpace.ActualHeight, TopSpace.ActualWidth);
            Board.Width = boardSize - boardBorderThickness * 2;
            Board.Height = boardSize - boardBorderThickness * 2;
            BoardBorder.Width = boardSize;
            BoardBorder.Height = boardSize;

            Board.Children.Clear();
            RedrawBoard();
        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            double boardSize = Math.Min(TopSpace.ActualHeight, TopSpace.ActualWidth);
            Board.Width = boardSize - boardBorderThickness * 2;
            Board.Height = boardSize - boardBorderThickness * 2;
            BoardBorder.Width = boardSize;
            BoardBorder.Height = boardSize;

            Board.Children.Clear();
            RedrawBoard();
        }

        private void DrawBoard()
        {
            int lineMargin = 0;//khoảng cách giữa các đường kẻ với border

            Rectangle rectangle = new Rectangle();
            double minValue = Math.Min(TopSpace.ActualHeight, TopSpace.ActualWidth);
            double sizeToSet = minValue - boardBorderThickness * 2;
            rectangle.Width = sizeToSet;
            rectangle.Height = sizeToSet;
            rectangle.Fill = Brushes.White;
            Board.Children.Add(rectangle);

            for (int i = 0; i < size - 1; i++)
            {
                Line verticalLine = new Line();
                verticalLine.Stroke = System.Windows.Media.Brushes.Black;
                verticalLine.X1 = sizeToSet / size * (i + 1);
                verticalLine.Y1 = 0 + lineMargin;
                verticalLine.X2 = sizeToSet / size * (i + 1);
                verticalLine.Y2 = sizeToSet - lineMargin;
                verticalLine.HorizontalAlignment = HorizontalAlignment.Center;
                verticalLine.VerticalAlignment = VerticalAlignment.Center;
                verticalLine.StrokeThickness = STROKE_THICKNESS;
                Board.Children.Add(verticalLine);
            }
            for (int i = 0; i < size - 1; i++)
            {
                Line horizontalLine = new Line();
                horizontalLine.Stroke = System.Windows.Media.Brushes.Black;
                horizontalLine.X1 = 0 + lineMargin;
                horizontalLine.Y1 = sizeToSet / size * (i + 1);
                horizontalLine.X2 = sizeToSet - lineMargin;
                horizontalLine.Y2 = sizeToSet / size * (i + 1);
                horizontalLine.HorizontalAlignment = HorizontalAlignment.Center;
                horizontalLine.VerticalAlignment = VerticalAlignment.Center;
                horizontalLine.StrokeThickness = STROKE_THICKNESS;
                Board.Children.Add(horizontalLine);
            }

            Rectangle selectorSquare = new Rectangle();
            selectorSquare.Width = cellWidth - STROKE_THICKNESS > 0 ? cellWidth - STROKE_THICKNESS : 0;
            selectorSquare.Height = cellWidth - STROKE_THICKNESS > 0 ? cellWidth - STROKE_THICKNESS : 0;
            selectorSquare.SetValue(Canvas.LeftProperty, sizeToSet / size * selectorX);
            selectorSquare.SetValue(Canvas.TopProperty, sizeToSet / size * selectorY);
            seletor = selectorSquare;
            Board.Children.Add(selectorSquare);
            if (control == ControlMode.Keyboard)
            {
                selectorSquare.Fill = selectorColor;
            }
            else
            {
                selectorSquare.Fill = Brushes.White;
            }
        }

        private void RedrawBoard()
        {
            Board.Children.Clear();
            cellWidth = Board.ActualHeight / size;
            DrawBoard();

            //Vẽ lại các điểm x, o
            if (BoardCellMap == null)
            {
                return;
            }
            for (int i = 0; i < size; i++)
                for (int j = 0; j < size; j++)
                {
                    BoardCell point = BoardCellMap[i, j];
                    DrawPoint(point.x, point.y, point.player);
                }
        }

        private void DrawPoint(int x, int y, Player turn) // 0: X, 1: O
        {
            if (turn == Player.PlayerX)//X
            {
                Line line1 = new Line();
                line1.Stroke = System.Windows.Media.Brushes.Blue;
                line1.X1 = cellWidth * x + 1;
                line1.Y1 = cellWidth * y + 1;
                line1.X2 = cellWidth * (x + 1) - 1;
                line1.Y2 = cellWidth * (y + 1) - 1;
                line1.HorizontalAlignment = HorizontalAlignment.Center;
                line1.VerticalAlignment = VerticalAlignment.Center;
                line1.StrokeThickness = 5;
                Board.Children.Add(line1);

                Line line2 = new Line();
                line2.Stroke = System.Windows.Media.Brushes.Blue;
                line2.X1 = cellWidth * (x + 1) - 1;
                line2.Y1 = cellWidth * y + 1;
                line2.X2 = cellWidth * x + 1;
                line2.Y2 = cellWidth * (y + 1) - 1;
                line2.HorizontalAlignment = HorizontalAlignment.Center;
                line2.VerticalAlignment = VerticalAlignment.Center;
                line2.StrokeThickness = 5;
                Board.Children.Add(line2);
            }
            else if (turn == Player.PlayerO)
            {
                Ellipse round = new Ellipse();
                int padding = 0;
                int thichness = 5;
                round.Width = cellWidth - padding;
                round.Height = cellWidth - padding;
                round.Stroke = System.Windows.Media.Brushes.Red;
                round.StrokeThickness = thichness;
                round.HorizontalAlignment = HorizontalAlignment.Center;
                round.VerticalAlignment = VerticalAlignment.Center;
                Canvas.SetLeft(round, padding / 2 * (x * 2 + 1) + (x * (cellWidth - padding)));
                Canvas.SetTop(round, padding / 2 * (y * 2 + 1) + (y * (cellWidth - padding)));
                Board.Children.Add(round);
            }
        }

        private void RestartGame()
        {
            cellLeft = size * size;
            playerTurn = Player.PlayerX;
            cellWidth =  Board.ActualHeight / size;
            boardbool = new bool[size, size];
            BoardCellMap = new BoardCell[size, size];

            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    boardbool[i, j] = false;
                    BoardCellMap[i, j] = new BoardCell(j, i, Player.None);
                }
            }

            Board.Children.Clear();
            DrawBoard();
        }

        private void onCellSelected(int x, int y)
        {
            if (boardbool[y, x])
            {
                return;
            }
            else
            {
                boardbool[y, x] = true;
                cellLeft--;
            }

            BoardCellMap[y, x].player = playerTurn;
            DrawPoint(x, y, playerTurn);

            if (EndGameCheck(x, y, playerTurn))
            {
                RestartGame();
                return;
            }

            if (cellLeft == 0)
            {
                MessageBox.Show("Kết quả Hòa", "info", MessageBoxButton.OK, MessageBoxImage.Information);
                RestartGame();
                return;
            }

            if (playerTurn == Player.PlayerX)
            {
                playerTurn = Player.PlayerO;
            }
            else
            {
                playerTurn = Player.PlayerX;
            }
        }

        private void Board_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (control == ControlMode.Keyboard)
            {
                return;
            }

            Point mousePosition = e.GetPosition(sender as IInputElement);
            double mouseX = mousePosition.X;
            double mouseY = mousePosition.Y;

            int x = ((int)mouseX) % ((int)cellWidth) != 0 ? ((int)mouseX) / ((int)cellWidth) : ((int)mouseX) / ((int)cellWidth) + 1;
            int y = ((int)mouseY) % ((int)cellWidth) != 0 ? ((int)mouseY) / ((int)cellWidth) : ((int)mouseY) / ((int)cellWidth) + 1;

            onCellSelected(x, y);
        }

        private bool EndGameCheck(int x, int y, Player player)
        {
            int num;

            //check hàng ngang
            num = 1;
            for (int i  = x - 1; i >= 0 ; i--)
            {
                if (BoardCellMap[y, i].player == player)
                    num++;
                else
                    break;
            }
            for (int i = x + 1; i < size; i++)
            {
                if (BoardCellMap[y, i].player == player)
                    num++;
                else
                    break;
            }
            if (num == numToWin)
            {
                if (player == Player.PlayerX)
                    MessageBox.Show("Kết thúc game: X thắng", "info", MessageBoxButton.OK, MessageBoxImage.Information);
                else
                    MessageBox.Show("Kết thúc game: O thắng", "info", MessageBoxButton.OK, MessageBoxImage.Information);

                return true;
            }

            //check hàng dọc
            num = 1;
            for (int i = y - 1; i >= 0; i--)
            {
                if (BoardCellMap[i, x].player == player)
                    num++;
                else
                    break;
            }
            for (int i = y + 1; i < size; i++)
            {
                if (BoardCellMap[i, x].player == player)
                    num++;
                else
                    break;
            }
            if (num == numToWin)
            {
                if (player == Player.PlayerX)
                    MessageBox.Show("Kết thúc game: X thắng", "info", MessageBoxButton.OK, MessageBoxImage.Information);
                else
                    MessageBox.Show("Kết thúc game: O thắng", "info", MessageBoxButton.OK, MessageBoxImage.Information);

                return true;
            }

            //Kiểm tra đường chéo \
            int n;
            int m;
            num = 1;
            n = y - 1;
            m = x - 1;
            while (n >= 0 && m >= 0)
            {
                if (BoardCellMap[n, m].player == player)
                    num++;
                else
                    break;
                n--;
                m--;
            }
            n = y + 1;
            m = x + 1;
            while (n < size && m < size)
            {
                if (BoardCellMap[n, m].player == player)
                    num++;
                else
                    break;
                n++;
                m++;
            }
            if (num == numToWin)
            {
                if (player == Player.PlayerX)
                    MessageBox.Show("Kết thúc game: X thắng", "info", MessageBoxButton.OK, MessageBoxImage.Information);
                else
                    MessageBox.Show("Kết thúc game: O thắng", "info", MessageBoxButton.OK, MessageBoxImage.Information);

                return true;
            }

            //Kiểm tra đường chéo /
            num = 1;
            n = y - 1;
            m = x + 1;
            while (n >= 0 && m < size)
            {
                if (BoardCellMap[n, m].player == player)
                    num++;
                else
                    break;
                n--;
                m++;
            }
            n = y + 1;
            m = x - 1;
            while (n < size && m >= 0)
            {
                if (BoardCellMap[n, m].player == player)
                    num++;
                else
                    break;
                n++;
                m--;
            }
            if (num == numToWin)
            {
                if (player == Player.PlayerX)
                    MessageBox.Show("Kết thúc game: X thắng", "info", MessageBoxButton.OK, MessageBoxImage.Information);
                else
                    MessageBox.Show("Kết thúc game: O thắng", "info", MessageBoxButton.OK, MessageBoxImage.Information);

                return true;
            }

            return false;
        }

        private void ControlBtn_Click(object sender, RoutedEventArgs e)
        {
            if (control == ControlMode.Mouse)
            {
                control = ControlMode.Keyboard;
                ControlBtn.Content = "Control mode: Keyboard";
            }
            else
            {
                control = ControlMode.Mouse;
                ControlBtn.Content = "Control mode: Mouse";
            }
            RedrawBoard();
        }

        private void Board_KeyDown(object sender, KeyEventArgs e)
        {
            if (control == ControlMode.Mouse)
            {
                return;
            }

            switch (e.Key)
            {
                case Key.Up:
                case Key.W:
                    if (selectorY > 0)
                    {
                        selectorY--;
                    }
                    break;
                case Key.Down:
                case Key.S:
                    if (selectorY < size - 1)
                    {
                        selectorY++;
                    }
                    break;
                case Key.Left:
                case Key.A:
                    if (selectorX > 0)
                    {
                        selectorX--;
                    }
                    break;
                case Key.Right:
                case Key.D:
                    if (selectorX < size - 1)
                    {
                        selectorX++;
                    }
                    break;
                case Key.Enter:
                    {
                        onCellSelected(selectorX, selectorY);
                    }
                    break;
                default:
                    break;
            }
            double minValue = Math.Min(TopSpace.ActualHeight, TopSpace.ActualWidth);
            double sizeToSet = minValue - boardBorderThickness * 2;
            seletor.SetValue(Canvas.LeftProperty, sizeToSet / size * selectorX);
            seletor.SetValue(Canvas.TopProperty, sizeToSet / size * selectorY);
            RedrawBoard();
        }
    }
}