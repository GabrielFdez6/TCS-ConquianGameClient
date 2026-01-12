using System;
using System.IO;
using System.Windows.Media;

namespace ConquiánCliente.Utilities
{
    public class AudioManager
    {
        private static AudioManager instance;
        public static AudioManager Instance => instance ?? (instance = new AudioManager());

        private readonly MediaPlayer mediaPlayer;
        private readonly string menuMusicPath;
        private readonly string gameMusicPath;
        private bool isGameMusicPlaying = false;

        private AudioManager()
        {
            mediaPlayer = new MediaPlayer();

            string baseDir = AppDomain.CurrentDomain.BaseDirectory;
            menuMusicPath = Path.Combine(baseDir, "Resources", "Music", "MainMenuMusic.mp3");
            gameMusicPath = Path.Combine(baseDir, "Resources", "Music", "GameMusic.mp3");

            mediaPlayer.MediaEnded += (s, e) =>
            {
                mediaPlayer.Position = TimeSpan.Zero;
                mediaPlayer.Play();
            };
        }

        public void PlayMenuMusic()
        {
            if (!isGameMusicPlaying && mediaPlayer.Source != null) return;

            PlayFile(menuMusicPath);
            isGameMusicPlaying = false;
        }

        public void PlayGameMusic()
        {
            if (isGameMusicPlaying && mediaPlayer.Source != null) return;

            PlayFile(gameMusicPath);
            isGameMusicPlaying = true;
        }

        private void PlayFile(string path)
        {
            try
            {
                if (File.Exists(path))
                {
                    mediaPlayer.Open(new Uri(path));
                    mediaPlayer.Play();
                }
                else
                {
                    Console.WriteLine($"Archivo de música no encontrado: {path}");
                }
            }
            catch (Exception)
            {
                Console.WriteLine("Error al reproducir música");
            }
        }

        public void SetVolume(double volume)
        {
            double maxRealVolume = 0.1;

            mediaPlayer.Volume = (volume / 100.0) * maxRealVolume;
        }

        public void StopMusic()
        {
            mediaPlayer.Stop();
        }
    }
}
