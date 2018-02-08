using Android.App;
using Android.Media;
using Java.IO;

namespace MobilePlayer.Services
{
    public abstract class MusicServiceAbstract : Service, MediaPlayer.IOnPreparedListener
    {
        public abstract File WhatPlays { get; }
        public abstract bool IsPlaying { get; }
        
        public abstract void Play(File path);

        public abstract void Play();

        public abstract void Pause();
        public abstract void OnPrepared(MediaPlayer mp);
    }
}