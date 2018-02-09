using System.Collections.Generic;
using System.Threading.Tasks;
using Android.App;
using Android.Media;
using Java.IO;
using MobilePlayer.Models.Music;

namespace MobilePlayer.Services
{
    public abstract class MusicServiceAbstract : Service, MediaPlayer.IOnPreparedListener
    {
        public abstract IList<Song> Playlist { get; }
        public abstract int Current { get; }
        public Song CurrentSong => IsPlaying ? Playlist[Current] : null;
        
        public abstract bool IsPlaying { get; }

        public abstract Task PlayList(IList<Song> playlist, int index = 0);
        public abstract Task Play(int index);

        public abstract void Play();

        public abstract void Pause();

        public abstract Task Next();
        public abstract Task Previous();

        public abstract void OnPrepared(MediaPlayer mp);
    }
}