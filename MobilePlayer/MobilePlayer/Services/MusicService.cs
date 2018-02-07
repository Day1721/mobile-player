using Android.App;
using Android.Content;
using Android.Media;
using Android.OS;

namespace MobilePlayer.Services
{
    public class MusicService : Service, IMusicService
    {
        private MediaPlayer _player;

        public bool IsPlaying => _player.IsPlaying;
        public int CurrentPosition => _player.CurrentPosition;
        
        public override IBinder OnBind(Intent intent)
        {
            _player = new MediaPlayer();
            return null;
        }

        public void Play(string path)
        {
            _player.SetDataSource(path);
            _player.Start();
        }

        public void Play()
        {
            _player.Start();
        }

        public void Pause()
        {
            _player.Pause();
        }
    }
}