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
            string baseDir = System.AppDomain.CurrentDomain.BaseDirectory;
            menuMusicPath = Path.Combine(baseDir, "Resources", "Music", "MainMenuMusic.mp3");
            gameMusicPath = Path.Combine(baseDir, "Resources", "Music", "GameMusic.mp3");

            mediaPlayer.MediaEnded += (s, e) =>
            {
                mediaPlayer.Position = System.TimeSpan.Zero;
                mediaPlayer.Play();
            };
        }

        public void PlayMenuMusic()
        {
            bool shouldSkipPlayback = !isGameMusicPlaying && mediaPlayer.Source != null;
            if (shouldSkipPlayback)
            {
                return;
            }

            PlayFile(menuMusicPath);
            isGameMusicPlaying = false;
        }

        public void PlayGameMusic()
        {
            bool shouldSkipPlayback = isGameMusicPlaying && mediaPlayer.Source != null;
            if (shouldSkipPlayback)
            {
                return;
            }

            PlayFile(gameMusicPath);
            isGameMusicPlaying = true;
        }

        private void PlayFile(string path)
        {
            try
            {
                bool fileExists = File.Exists(path);
                if (fileExists)
                {
                    mediaPlayer.Open(new System.Uri(path));
                    mediaPlayer.Play();
                }
                else
                {
                    LogMissingFile(path);
                }
            }
            catch (IOException ioException)
            {
                LogPlaybackError(ioException.Message);
            }
            catch (UnauthorizedAccessException accessException)
            {
                LogPlaybackError(accessException.Message);
            }
            catch (System.NotSupportedException notSupportedException)
            {
                LogPlaybackError(notSupportedException.Message);
            }
        }

        public void SetVolume(double volume)
        {
            const double maxRealVolume = 0.1;
            double calculatedVolume = (volume / 100.0) * maxRealVolume;
            mediaPlayer.Volume = calculatedVolume;
        }

        public void StopMusic()
        {
            mediaPlayer.Stop();
        }

        private void LogMissingFile(string path)
        {
            System.Diagnostics.Debug.WriteLine($"Archivo de música no encontrado: {path}");
        }

        private void LogPlaybackError(string errorMessage)
        {
            System.Diagnostics.Debug.WriteLine($"Error al reproducir música: {errorMessage}");
        }
    }
}