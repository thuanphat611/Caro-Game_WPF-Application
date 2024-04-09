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
        public static void PlayClickSound()
        {
            if (!GlobalVariables.SoundOn)
                return;

            MediaPlayer mediaPlayer = new MediaPlayer();
            string soundFilePath = Path.Combine(Environment.CurrentDirectory, "Sound", "click_sound.wav");

            if (!File.Exists(soundFilePath))
            {
                return;
            }

            try
            {
                mediaPlayer.Open(new Uri(soundFilePath, UriKind.RelativeOrAbsolute));
                mediaPlayer.Play();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error playing sound: " + ex.Message);
            }
        }

        public static void PlayVictorySound()
        {
            if (!GlobalVariables.SoundOn)
                return;

            MediaPlayer mediaPlayer = new MediaPlayer();
            string soundFilePath = Path.Combine(Environment.CurrentDirectory, "Sound", "endgame.mp3");

            if (!File.Exists(soundFilePath))
            {
                return;
            }

            try
            {
                mediaPlayer.Open(new Uri(soundFilePath, UriKind.RelativeOrAbsolute));
                mediaPlayer.Play();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error playing sound: " + ex.Message);
            }
        }

    }

    public class BackgroundMusic
    {
        MediaPlayer mediaPlayer;
        bool canPlay = true;
        string soundFilePath = Path.Combine(Environment.CurrentDirectory, "Sound", "background_music.mp3");

        public BackgroundMusic()
        {
            mediaPlayer = new MediaPlayer();
            mediaPlayer.MediaEnded += MediaPlayer_MediaEnded;

            if (!File.Exists(soundFilePath))
            {
                canPlay = false;
            }
            else
            {
                mediaPlayer.Open(new Uri(soundFilePath, UriKind.RelativeOrAbsolute));
            }
        }

        public void Play()
        {
            if (!GlobalVariables.SoundOn || !canPlay)
                return;

            try
            {
                mediaPlayer.Play();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error playing sound: " + ex.Message);
            }
        }

        public void Stop()
        {
            mediaPlayer?.Stop();
        }

        private void MediaPlayer_MediaEnded(object sender, EventArgs e)
        {
            mediaPlayer.Position = TimeSpan.Zero;
            mediaPlayer.Play();
        }
    }
}
