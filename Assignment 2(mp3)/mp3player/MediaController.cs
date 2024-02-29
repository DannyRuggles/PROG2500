using System.Windows.Media;

namespace mp3player
{
    public class MediaController
    {
        private readonly MediaPlayer mediaPlayer;

        public MediaController()
        {
            mediaPlayer = new MediaPlayer();
        }

        public void Open(string filePath)
        {
            if (string.IsNullOrEmpty(filePath))
            {
                throw new ArgumentException("File path cannot be null or empty.", nameof(filePath));
            }

            mediaPlayer.Open(new Uri(filePath, UriKind.Absolute));
        }

        public void Play()
        {
            mediaPlayer.Play();
        }

        public void Pause()
        {
            if (mediaPlayer.CanPause)
            {
                mediaPlayer.Pause();
            }
        }

        public void Stop()
        {
            mediaPlayer.Stop();
        }

        public void Close()
        {
            mediaPlayer.Close(); // this should release the file for metadata rewrite
        }


    }
}
