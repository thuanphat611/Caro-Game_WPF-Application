﻿using System.Collections.ObjectModel;
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

    public partial class MainWindow : Window
    {
        const int STROKE_THICKNESS = 1;
        int size = 5;
        int numToWin = 3;

        BoardCell[,] BoardCellMap = null;
        int cellLeft;
        double cellWidth;
        Player playerTurn;
        bool[,] boardbool;

        public MainWindow()
        {
            InitializeComponent();
            TopSpace.Loaded += TopSpace_Loaded;
        }

        private void TopSpace_Loaded(object sender, RoutedEventArgs e)
        {
            Board.Width = 10;
            Board.Height = 10;
            double boardSize = Math.Min(TopSpace.ActualHeight, TopSpace.ActualWidth);
            Board.Width = boardSize;
            Board.Height = boardSize;
            BoardBorder.Width = boardSize;
            BoardBorder.Height = boardSize;

            RestartGame();
        }
        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            double boardSize = Math.Min(TopSpace.ActualHeight, TopSpace.ActualWidth);
            Board.Width = boardSize;
            Board.Height = boardSize;
            BoardBorder.Width = boardSize;
            BoardBorder.Height = boardSize;

            Board.Children.Clear();
            RedrawBoard();
        }

        private void MainWindow_StateChanged(object sender, EventArgs e)
        {
            RedrawBoard();
        }

        private void DrawBoard()
        {
            Rectangle rectangle = new Rectangle();
            double minValue = Math.Min(TopSpace.ActualHeight, TopSpace.ActualWidth);
            double sizeToSet = minValue - 2;
            rectangle.Width = sizeToSet;
            rectangle.Height = sizeToSet;
            rectangle.Fill = Brushes.White;
            Board.Children.Add(rectangle);

            for (int i = 0; i < size - 1; i++)
            {
                Line verticalLine = new Line();
                verticalLine.Stroke = System.Windows.Media.Brushes.Black;
                verticalLine.X1 = Math.Min(TopSpace.ActualHeight, TopSpace.ActualWidth) / size * (i + 1);
                verticalLine.Y1 = 0;
                verticalLine.X2 = Math.Min(TopSpace.ActualHeight, TopSpace.ActualWidth) / size * (i + 1);
                verticalLine.Y2 = Math.Min(TopSpace.ActualHeight, TopSpace.ActualWidth);
                verticalLine.HorizontalAlignment = HorizontalAlignment.Center;
                verticalLine.VerticalAlignment = VerticalAlignment.Center;
                verticalLine.StrokeThickness = STROKE_THICKNESS;
                Board.Children.Add(verticalLine);
            }
            for (int i = 0; i < size - 1; i++)
            {
                Line horizontalLine = new Line();
                horizontalLine.Stroke = System.Windows.Media.Brushes.Black;
                horizontalLine.X1 = 0;
                horizontalLine.Y1 = Math.Min(TopSpace.ActualHeight, TopSpace.ActualWidth) / size * (i + 1);
                horizontalLine.X2 = Math.Min(TopSpace.ActualHeight, TopSpace.ActualWidth);
                horizontalLine.Y2 = Math.Min(TopSpace.ActualHeight, TopSpace.ActualWidth) / size * (i + 1);
                horizontalLine.HorizontalAlignment = HorizontalAlignment.Center;
                horizontalLine.VerticalAlignment = VerticalAlignment.Center;
                horizontalLine.StrokeThickness = STROKE_THICKNESS;
                Board.Children.Add(horizontalLine);
            }
        }

        private void RedrawBoard()
        {
            Board.Children.Clear();
            DrawBoard();

            //Vẽ lại các điểm x, o
            if (BoardCellMap == null)
            {
                return;
            }
            cellWidth = Board.ActualHeight / size;
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
                line1.X1 = cellWidth * x + 10;
                line1.Y1 = cellWidth * y + 10;
                line1.X2 = cellWidth * (x + 1) - 10;
                line1.Y2 = cellWidth * (y + 1) - 10;
                line1.HorizontalAlignment = HorizontalAlignment.Center;
                line1.VerticalAlignment = VerticalAlignment.Center;
                line1.StrokeThickness = 5;
                Board.Children.Add(line1);

                Line line2 = new Line();
                line2.Stroke = System.Windows.Media.Brushes.Blue;
                line2.X1 = cellWidth * (x + 1) - 10;
                line2.Y1 = cellWidth * y + 10;
                line2.X2 = cellWidth * x + 10;
                line2.Y2 = cellWidth * (y + 1) - 10;
                line2.HorizontalAlignment = HorizontalAlignment.Center;
                line2.VerticalAlignment = VerticalAlignment.Center;
                line2.StrokeThickness = 5;
                Board.Children.Add(line2);
            }
            else if (turn == Player.PlayerO)
            {
                Ellipse round = new Ellipse();
                int padding = 10;
                int thichness = 5;
                round.Width = cellWidth - padding;
                round.Height = cellWidth - padding;
                round.Fill = System.Windows.Media.Brushes.Red;
                round.HorizontalAlignment = HorizontalAlignment.Center;
                round.VerticalAlignment = VerticalAlignment.Center;
                Canvas.SetLeft(round, padding / 2 * (x * 2 + 1) + (x * (cellWidth - padding)));
                Canvas.SetTop(round, padding / 2 * (y * 2 + 1) + (y * (cellWidth - padding)));
                Board.Children.Add(round);

                Ellipse round2 = new Ellipse();
                round2.Width = cellWidth - padding - thichness * 2;
                round2.Height = cellWidth - padding - thichness * 2;
                round2.Fill = System.Windows.Media.Brushes.White;
                round2.HorizontalAlignment = HorizontalAlignment.Center;
                round2.VerticalAlignment = VerticalAlignment.Center;
                Canvas.SetLeft(round2, (padding / 2 + thichness) * (x * 2 + 1) + (x * (cellWidth - padding - thichness * 2)));
                Canvas.SetTop(round2, (padding / 2 + thichness) * (y * 2 + 1) + (y * (cellWidth - padding - thichness * 2)));
                Board.Children.Add(round2);
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

        private void Board_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Point mousePosition = e.GetPosition(sender as IInputElement);
            double mouseX = mousePosition.X;
            double mouseY = mousePosition.Y;

            int x = ((int)mouseX) % ((int)cellWidth) != 0 ? ((int)mouseX) / ((int)cellWidth) : ((int)mouseX) / ((int)cellWidth) + 1;
            int y = ((int)mouseY) % ((int)cellWidth) != 0 ? ((int)mouseY) / ((int)cellWidth) : ((int)mouseY) / ((int)cellWidth) + 1;

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

        private bool EndGameCheck(int x, int y, Player player)
        {
            int num;

            //check hàng ngang
            num = 1;
            for (int i  = x - 1; i >= 0 ; i--)
            {
                if (BoardCellMap[y, i].player == player)
                    num++;
            }
            for (int i = x + 1; i < size; i++)
            {
                if (BoardCellMap[y, i].player == player)
                    num++;
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
            }
            for (int i = y + 1; i < size; i++)
            {
                if (BoardCellMap[i, x].player == player)
                    num++;
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
    }
}