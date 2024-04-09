using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.IO;
using static Caro_game.MenuWindow;

namespace Caro_game
{
    internal class SoundManager
    {
        MediaPlayer mediaPlayer;
        public SoundManager() {
            mediaPlayer = new MediaPlayer();
        }

        public void PlayClickSound()
        {
            if (!GlobalVariables.SoundOn)
                return;

            string soundFilePath = Path.Combine(Environment.CurrentDirectory, "Sound", "click_sound.wav");

            try
            {
                if (File.Exists(soundFilePath))
                {
                    mediaPlayer.Open(new Uri(soundFilePath, UriKind.RelativeOrAbsolute));
                    mediaPlayer.Play();
                }
                else
                {
                    MessageBox.Show("Sound not found: " + soundFilePath);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi phát âm thanh: " + ex.Message);
            }
        }
    }
}
