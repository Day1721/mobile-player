using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;
using MobilePlayer.Models.Music;
using MobilePlayer.Services;
using Environment = Android.OS.Environment;

namespace MobilePlayer
{
    [Activity(Label = "MobilePlayer", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        private readonly IMusicScanner _scanner;
        private readonly IMusicParser _parser;
        private readonly Type _musicServiceType;
        private MusicServiceAbstract _musicService;

        private ImageButton _playPauseButton;
        private TextView _currentSongView;
        private ImageView _currentSongCoverView;
        
        public MainActivity()
        {
            _scanner = App.Container.Resolve<IMusicScanner>();
            _parser = App.Container.Resolve<IMusicParser>();
            _musicServiceType = App.Container.Resolve<MusicServiceAbstract>().GetType();
        }

        private List<Song> _songs;
        
        
        
        
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            LoadSongs();

            
            var listView = FindViewById<ListView>(Resource.Id.SongList);
            listView.Adapter = new ArrayAdapter<Song>(this, Android.Resource.Layout.SimpleListItem1, _songs);
            listView.EmptyView = FindViewById(Resource.Id.EmptySongList);
            
            
            listView.ItemClick += ListViewOnItemClick;

            _currentSongView = FindViewById<TextView>(Resource.Id.CurrentSongView);
            _playPauseButton = FindViewById<ImageButton>(Resource.Id.PlayPauseBtn);
            _currentSongCoverView = FindViewById<ImageView>(Resource.Id.NavbarCover);
            
            _playPauseButton.Click += PlayPauseOnClick;
            _currentSongView.Click += CurrentSongOnClick;
            //FindViewById<ImageButton>(Resource.Id.Previous).Click += PreviousOnClick;
            //FindViewById<ImageButton>(Resource.Id.Next).Click += NextOnClick;

            InitMusicService();
        }

        private void LoadSongs() => _songs = _scanner
            .Scan(Environment.ExternalStorageDirectory)
            .Select(_parser.Parse)
            .ToList();
        
        private void InitMusicService()
        {
            StartService(MusicServiceIntent);
            var serviceConnection = new MusicService.Connection();
            serviceConnection.ServiceConnected += (name, binder) =>
            {
                _musicService = ((MusicService.Binder) binder).Service;
            };
            BindService(MusicServiceIntent, serviceConnection, Bind.AutoCreate);
        }

        

        private async void ListViewOnItemClick(object sender, AdapterView.ItemClickEventArgs e) 
            => await PlayList(_songs, e.Position);

        private void PlayPauseOnClick(object sender, EventArgs e)
        {
            if (_musicService.IsPlaying)
            {
                Pause();    
            }
            else
            {
                Play();
            }
        }

        //private async void NextOnClick(object sender, EventArgs e) => await _musicService.Next();
        //private async void PreviousOnClick(object sender, EventArgs e) => await _musicService.Previous();

        private void Pause()
        {
            _musicService.Pause();
            _playPauseButton.SetImageResource(Resource.Drawable.Play128);
        }

        private void Play()
        {
            _musicService.Play();
            _playPauseButton.SetImageResource(Resource.Drawable.Pause128);
        }

        private async Task PlayList(IList<Song> songs, int position)
        {
            await _musicService.PlayList(songs, position);
            _currentSongView.Text = songs[position].ToString();
            if (songs[position].Cover == null)
            {
                _currentSongCoverView.SetImageResource(Resource.Drawable.NoCover);
            }
            else
            {
                _currentSongCoverView.SetImageBitmap(songs[position].Cover);
            }
            _currentSongCoverView.SetAdjustViewBounds(true);
            
            _playPauseButton.SetImageResource(Resource.Drawable.Pause128);
        }

        private void CurrentSongOnClick(object sender, EventArgs e)
        {
            if (_musicService.IsSetPlaylist)
            {
                ShowSongInfo(_musicService.CurrentSong);
            }
        }

        private void ShowSongInfo(Song song)
        {
            //TODO
        }

        private Intent MusicServiceIntent => new Intent(this, _musicServiceType);
    }
}