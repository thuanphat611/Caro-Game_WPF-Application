using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Caro_game
{
    public partial class SizeInputDialog : Window
    {
        MenuWindow menu;
        public SizeInputDialog(MenuWindow menu)
        {
            InitializeComponent();
            this.menu = menu;
        }

        private void OkayBtn_Click(object sender, RoutedEventArgs e)
        {
            string input = SizeInputBox.Text;
            Regex regex = new Regex(@"^\d+$");

            if (!regex.IsMatch(input))
            {
                MessageBox.Show("Vui lòng nhập số nguyên dương");
                SizeInputBox.Text = "";
                return;
            }
            if (int.Parse(input) < 5)
            {
                MessageBox.Show("Kích thước nhỏ nhất bằng 5");
                SizeInputBox.Text = "";
                return;
            }
            if (regex.IsMatch(input))
            {
                GamePlayWindow game = new GamePlayWindow(/*int.Parse(input)*/);
                game.Show();
                this.Close();
                menu.Close();
            }
        }
    }
}
