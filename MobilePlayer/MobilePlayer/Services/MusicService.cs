using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Media;
using Android.OS;
using MobilePlayer.Models.Extentions;
using MobilePlayer.Models.Music;

namespace MobilePlayer.Services
{
    [Service(Label = "Music Player")]
    public class MusicService : MusicServiceAbstract
    {
        private const int NotificationId = 6325;
        private Notification.Action.Builder _pausePlayActionBuilder;
        private Notification.Builder _notificationBuilder;

        private MediaPlayer _player;
        private IBinder _musicBinder;

        private bool _initialized = false;

        private IList<Song> _playlist;
        public override IList<Song> Playlist => _playlist;
        private void SetPlaylist(ICollection<Song> playlist)
        {
            _playlist = new List<Song>(playlist);
            _current = 0;
        }

        private int _current;
        public override int Current => _current;
        private void SetCurrent(int current)
        {
            if (current < 0 || current >= _playlist.Count)
            {
                throw new ArgumentOutOfRangeException(nameof(current));
            }
            _current = current;
        }
        
        public override bool IsPlaying => _player.IsPlaying;

        public override void OnCreate()
        {
            base.OnCreate();

            _player = new MediaPlayer {Looping = true};
            _player.SetOnPreparedListener(this);
            _musicBinder = new Binder(this);
            _notificationBuilder = new Notification.Builder(this);
        }

        public override StartCommandResult OnStartCommand(Intent intent, StartCommandFlags flags, int startId) => StartCommandResult.Sticky;

        public override IBinder OnBind(Intent intent) => _musicBinder;

        public override void Init() => _initialized = true;

        public override void Init(IList<Song> playlist, int index)
        {
            SetPlaylist(playlist);
            SetCurrent(index);
            _player.SetDataSource(_playlist[index].Path.AbsolutePath);
            _player.PrepareAsync();
        }

        public override async Task PlayList(IList<Song> playlist, int index = 0)
        {
            _player.Reset();
            SetPlaylist(playlist);
            SetCurrent(index);
            await _player.SetDataSourceAsync(_playlist[index].Path.AbsolutePath);
            _player.PrepareAsync();
        }

        public override async Task Play(int index)
        {
            _player.Reset();
            SetCurrent(index);
            await _player.SetDataSourceAsync(_playlist[index].Path.AbsolutePath);
            _player.PrepareAsync();
        }

        public override void Play()
        {
            _player.Start();
        }

        public override void Pause() => _player.Pause();

        public override async Task Next() => await Play((_current + 1).Mod(_playlist.Count));

        public override async Task Previous() => await Play((_current - 1).Mod(_playlist.Count));

            
        public override void OnPrepared(MediaPlayer mp)
        {
            var notificationIntent = new Intent(this, typeof(MainActivity));
            var pendingIntent = PendingIntent.GetActivity(this, 0, notificationIntent, 0);

            _notificationBuilder.SetContentTitle(GetText(Resource.String.MusicService_NotificationTitle))
                .SetContentText(Playlist[Current].ToString())
                .SetContentIntent(pendingIntent);

            var notification = _notificationBuilder.Build();
                        
            StartForeground(NotificationId, notification);
            if (_initialized)
            {
                mp.Start();
            }
            else
            {
                _initialized = true;
            }
        }


        public class Binder : Android.OS.Binder 
        {
            public MusicService Service { get; }
            
            public Binder(MusicService context)
            {
                Service = context;
            }
        }

        public class Connection : Java.Lang.Object, IServiceConnection
        {
            public delegate void ServiceDisconnectedHandler(ComponentName name);
            public event ServiceDisconnectedHandler ServiceDisconnected;
            
            public delegate void ServiceConnectedHandler(ComponentName name, IBinder binder);
            public event ServiceConnectedHandler ServiceConnected;
            
            
            public void OnServiceConnected(ComponentName name, IBinder service)
            {
                ServiceConnected?.Invoke(name, service);
            }

            public void OnServiceDisconnected(ComponentName name)
            {
                ServiceDisconnected?.Invoke(name);
            }
        }
    }
}