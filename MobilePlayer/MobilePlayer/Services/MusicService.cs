using System;
using Android.App;
using Android.Bluetooth;
using Android.Content;
using Android.Media;
using Android.OS;
using Java.IO;

namespace MobilePlayer.Services
{
    [Service(Label = "Music Player")]
    public class MusicService : MusicServiceAbstract
    {
        private const int NotificationId = 6325;
        
        
        private MediaPlayer _player;
        private IBinder _musicBinder;

        private File _whatPlays;
        public override File WhatPlays => _whatPlays;
        private void SetWhatPlays(File file)
        {
            _whatPlays = new File(file.ToURI());
        }
        
        public override bool IsPlaying => _player.IsPlaying;
        public int CurrentPosition => _player.CurrentPosition;

        public override void OnCreate()
        {
            base.OnCreate();

            _player = new MediaPlayer {Looping = true};
            _player.SetOnPreparedListener(this);
            _musicBinder = new Binder(this);
        }

        public override StartCommandResult OnStartCommand(Intent intent, StartCommandFlags flags, int startId) => StartCommandResult.Sticky;

        public override IBinder OnBind(Intent intent) => _musicBinder;

        public override void Play(File path)
        {
            _player.Reset();
            SetWhatPlays(path);
            _player.SetDataSource(path.AbsolutePath);
            _player.PrepareAsync();
        }

        public override void Play() => _player.Start();

        public override void Pause() => _player.Pause();
        
        public override void OnPrepared(MediaPlayer mp)
        {
            var notificationIntent = new Intent(this, typeof(MainActivity));
            var pendingIntent = PendingIntent.GetActivity(this, 0, notificationIntent, 0);
            var notification = new Notification.Builder(this)
                .SetContentTitle(GetText(Resource.String.MusicService_NotificationTitle))
                .SetContentIntent(pendingIntent)
                .Build();
            
            StartForeground(NotificationId, notification);
            mp.Start();
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