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
using System.Windows.Shapes;

namespace Caro_game
{
    /// <summary>
    /// Interaction logic for MenuWindow.xaml
    /// </summary>
    public partial class MenuWindow : Window
    {
        public static class GlobalVariables
        {
            public static bool SoundOn { get; set; }
        }

        public MenuWindow()
        {
            InitializeComponent();
            GlobalVariables.SoundOn = true;
        }

        private void ExitBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void LoadBtn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void PlayBtn_Click(object sender, RoutedEventArgs e)
        {
            SizeInputDialog dialog = new SizeInputDialog(this);
            dialog.ShowDialog();
        }

        private void SoundBtn_Click(object sender, RoutedEventArgs e)
        {
            GlobalVariables.SoundOn = !GlobalVariables.SoundOn;
            if (GlobalVariables.SoundOn)
            {
                SoundBtn.Content = "Sound: On";
            }
            else
            {
                SoundBtn.Content = "Sound: Off";
            }
        }
    }
}
