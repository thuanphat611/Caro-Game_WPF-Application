using Microsoft.Win32;
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
            public static bool SoundOn { get; set; } = true;
        }

        public MenuWindow()
        {
            InitializeComponent();
            if (GlobalVariables.SoundOn)
            {
                SoundBtn.Content = "Sound: On";
            }
            else
            {
                SoundBtn.Content = "Sound: Off";
            }
        }

        private void ExitBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void LoadBtn_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Tệp văn bản (*.txt)|*.txt";
            openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

            bool? result = openFileDialog.ShowDialog();

            if (result == true)
            {
                string selectedFilePath = openFileDialog.FileName;
                GamePlayWindow loadedGame = GamePlayWindow.LoadFile(selectedFilePath);
                if (loadedGame != null)
                {
                    this.Close();
                }
            }
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
